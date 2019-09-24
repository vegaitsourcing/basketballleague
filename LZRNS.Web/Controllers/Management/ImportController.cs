using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Cache;
using LZRNS.DomainModels.ExcelLoaderModels;
using LZRNS.DomainModels.Models;
using LZRNS.ExcelLoader;
using LZRNS.ExcelLoader.ExcelReader;
using LZRNS.Models.AdditionalModels.Forms;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

using ExL = LZRNS.ExcelLoader.ExcelReader;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class ImportController : RenderMvcController
    {
        private readonly LeagueSeasonDataCache _cache;
        private readonly BasketballDbContext _db;
        private readonly IExcelLoaderCorrector _excelLoaderCorrector;

        public ImportController(BasketballDbContext dbContext, IExcelLoaderCorrector corrector)
        {
            _db = dbContext;
            _cache = new LeagueSeasonDataCache(_db);
            _excelLoaderCorrector = corrector;
        }

        public ActionResult Index(ImportModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Index([Bind(Prefix = nameof(ImportModel.FormModel))]ImportFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Niste odabrali dokument za import.");
                return View(new ImportModel(CurrentPage));
            }

            return !ShouldImport(model) ? Analyze(model) : Import(model);
        }

        private static string FormatTeamName(string teamName)
        {
            return teamName?.ToLower().Trim() ?? "";
        }

        private static MemoryStream GetFileAsMemoryStream(HttpPostedFileBase uploadedFile)
        {
            var buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);
            return new MemoryStream(buf);
        }

        private static bool IsFilePlayersCodeList(string fileName)
        {
            return fileName.Split('-').Last().Contains("codelist");
        }

        private static bool IsTeamInGame(Game game, string teamName)
        {
            string formattedTeamName = FormatTeamName(teamName);
            string teamAName = FormatTeamName(game?.TeamA?.TeamName);
            string teamBName = FormatTeamName(game?.TeamB?.TeamName);
            return formattedTeamName.Equals(teamAName) || formattedTeamName.Equals(teamBName);
        }

        private static bool ShouldImport(ImportFormModel model)
        {
            return model.Files.Any(file => IsFilePlayersCodeList(file.FileName));
        }

        private ActionResult Analyze(ImportFormModel model)
        {
            var converter = new TimeTableLoader.Converter.Converter();
            var analyzer = new ExcelAnalyzer(Server.MapPath("~/App_Data/TableMapper_analyzer.config"));

            foreach (var file in model.Files)
            {
                if (file == null) continue;

                var memStr = GetFileAsMemoryStream(file);

                if (file.FileName.Contains("txt"))
                {
                    converter.Convert(memStr);
                    converter.SaveToDb();
                }
                else
                {
                    analyzer.ProcessFile(memStr, file.FileName);
                }
            }

            return CreateCodeListFile(analyzer);
        }

        private ActionResult CreateCodeListFile(ExcelAnalyzer loader)
        {
            var writer = new DataParser(_db, loader);
            var playerInfoList = writer.GetPlayersInfoList();
            var excelWriter = new ExcelWriter();
            excelWriter.CreateHeader();

            excelWriter.WritePlayerInfoList(playerInfoList);
            string codingListsDirectory = Server.MapPath("~/coding-lists/");
            if (!Directory.Exists(codingListsDirectory))
            {
                Directory.CreateDirectory(codingListsDirectory);
            }
            string fileName = loader.SeasonName + "-" + loader.LeagueName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "-codelist.xlsx";
            string filePath = codingListsDirectory + fileName;

            excelWriter.SaveAndRelease(filePath);

            return File(filePath,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml",
                        fileName);
        }

        private void CreateTeamsRoundsAndGames(ExL.ExcelLoader loader, LeagueSeason leagueSeason)
        {
            foreach (var teamScore in loader.TeamStatisticByTeamName)
            {
                string teamName = teamScore.Key;
                foreach (var score in teamScore.Value.TeamScores)
                {
                    var round = _cache.RoundCache.CreateOrGetRound(leagueSeason.Id, score.RoundName);
                    var teamA = _cache.TeamCache.CreateOrGetTeamByName(teamName, leagueSeason);
                    var teamB = _cache.TeamCache.CreateOrGetTeamByName(score.AgainstTeam, leagueSeason);
                    _cache.GameCache.CreateOrGetGame(leagueSeason.Season, round, teamA, teamB, score.MatchDate);
                }
            }
        }

        private List<Player> GeneratePlayersData(ExL.ExcelLoader loadedData, List<PlayerInfo> playerInfoList, HashSet<string> playersIds, Team team, LeagueSeason leagueSeason)
        {
            var playerList = new List<Player>(playersIds.Count);
            var games = _cache.GameCache.GetGamesForTeam(team).ToList();
            foreach (var playerInfo in playerInfoList)
            {
                var player = _cache.PlayerCache.CreateOrGetPlayer(playerInfo);

                foreach (var playerScore in loadedData.GetPlayerScoreList(team.TeamName, playerInfo.NameAndLastName))
                {
                    string againstTeam = playerScore.AgainstTeam;
                    var game = games.Find(g => IsTeamInGame(g, againstTeam));
                    if (game != null)
                    {
                        var stats = _cache.StatsCache.CreateOrGetStatsForPlayer(playerScore, player, game, playerInfo.OnLoan);
                        player.Stats.Add(stats);
                    }
                }
                playerList.Add(player);
                _cache.PlayerPerTeamCache.CreateOrGetPlayerPerTeam(player, team, leagueSeason);
            }
            return playerList;
        }

        private ActionResult Import(ImportFormModel model)
        {
            var converter = new TimeTableLoader.Converter.Converter();
            var loader = new ExL.ExcelLoader(Server.MapPath("~/App_Data/TableMapper.config"));
            var codingListLoader = new CodingListLoader(Server.MapPath("~/App_Data/TableMapper_coding_list.config"));

            foreach (var file in model.Files)
            {
                if (file == null) continue;

                var memStr = GetFileAsMemoryStream(file);

                if (file.FileName.Contains("txt"))
                {
                    converter.Convert(memStr);
                    converter.SaveToDb();
                }
                else if (file.FileName.Contains("codelist"))
                {
                    codingListLoader.ProcessFile(memStr, file.FileName);
                }
                else
                {
                    loader.ProcessFile(memStr, file.FileName);
                }
            }

            _excelLoaderCorrector.CorrectInvalidTeamNames(loader);

            Log4NetLogger.Log.Info($"Importing data for season {loader.SeasonName} and league {loader.LeagueName}");

            _cache.LoadDataToCache(loader.SeasonName, loader.LeagueName);
            PopulateEntityModel(loader, codingListLoader, loader.SeasonName, loader.LeagueName);

            return View(new ImportModel(CurrentPage));
        }

        private void PopulateEntityModel(ExL.ExcelLoader loadedData, CodingListLoader playerListData, string seasonName, string leagueName)
        {
            var leagueSeason = _cache.LeagueSeasonCache.CreateOrGetCurrentLeagueSeason(seasonName, leagueName);

            CreateTeamsRoundsAndGames(loadedData, leagueSeason);
            _cache.SaveChanges();

            PopulateTeamEntityModel(loadedData, playerListData, leagueSeason);
            _cache.SaveChanges();
        }

        private void PopulateTeamEntityModel(ExL.ExcelLoader loadedData, CodingListLoader codingListData, LeagueSeason leagueSeason)
        {
            foreach (var keyValuePair in loadedData.PlayerNamesByTeamName)
            {
                string teamName = keyValuePair.Key;
                var playerNames = keyValuePair.Value;
                var playerInfoList = codingListData.PlayerInfoList.Where(pi => pi.NewTeamName.Equals(teamName)).ToList();
                var team = _cache.TeamCache.CreateOrGetTeamByName(teamName, leagueSeason);
                team.Players = GeneratePlayersData(loadedData, playerInfoList, playerNames, team, leagueSeason);
            }
        }
    }
}