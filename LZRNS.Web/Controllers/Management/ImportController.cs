using LZRNS.Models.DocumentTypes.Pages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using ExL = LZRNS.ExcelLoader.ExcelReader;

using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.AdditionalModels.Forms;
using System;
using LZRNS.ExcelLoader;
//using LZRNS.ExcelLoader.ExcelReader;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class ImportController : RenderMvcController
    {

        //add dependency injection principle
        private BasketballDbContext _db = new BasketballDbContext();

        public ActionResult Index(ImportModel model)
        {
            return View(model);
        }


        //helper
        public bool IsFilePlayersCodeList(string fileName)
        {
            return fileName.Split('-').Last().Contains("codelist");
        }

        [HttpPost]
        public ActionResult Index([Bind(Prefix = nameof(ImportModel.FormModel))]ImportFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Niste odabrali dokument za import.");
                return View(new ImportModel(CurrentPage));
            }
            //ExL.AbstractExcelLoader = null;
            ExL.AbstractExcelLoader loader = null;

            TimeTableLoader.Converter.Converter converter = new TimeTableLoader.Converter.Converter();
            ActionResult response = null;

            //here, there is two paths to execute 
            //1) if player code list is in files - importing
            //2) if there is not - analyzing
            bool doImport = false;
            if (model.Files.Select(file => IsFilePlayersCodeList(file.FileName)).Contains(true))
            {
                //importer
                loader = new ExL.ExcelLoader(Server.MapPath("~/App_Data/TableMapper.config"));
                doImport = true;
            }
            else
            {
                //analyzer
                loader = new ExL.ExcelAnalyzer(Server.MapPath("~/App_Data/TableMapper_analyzer.config"));
            }
            foreach (var file in model.Files)
            {
                if (file != null)
                {
                    var memStr = GetFileAsMemoryStream(file);

                    if (file.FileName.Contains("txt"))
                    {
                        converter.Convert(memStr);
                        converter.SaveToDb();
                    }
                    else
                    {                          
                        loader.ProcessFile(memStr, file.FileName);
                    }
                }
            }

            if (doImport)
            {
                PopulateEntityModel((ExL.ExcelLoader)loader, loader.SeasonName, loader.LeagueName);
                response = View(new ImportModel(CurrentPage));
            }
            else
            {
                ExL.DataParser writer = new ExL.DataParser(_db, loader);
                var playerInfoList = writer.GetPlayersInfoList();
                ExcelLoader.ExcelWriter excelWriter = new ExcelLoader.ExcelWriter();
                excelWriter.CreateHeader();

                excelWriter.WritePlayerInfoList(playerInfoList);
                string codingListsDirectory = Server.MapPath("~/coding-lists/");
                if (!Directory.Exists(codingListsDirectory))
                {
                    Directory.CreateDirectory(codingListsDirectory);
                }
                string fileName = loader.SeasonName + "-" + loader.LeagueName + "-" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "-" + "codelist.xlsx";
                string filePath = codingListsDirectory + fileName;

                excelWriter.SaveAndRelease(filePath);
                response = File(filePath,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml",
                fileName);

            }
            return response;
        }

        //private 
        private void PopulateEntityModel(ExL.ExcelLoader loadedData, string seasonName, string leagueName)
        {
            /*use season repo instead of context*/

            bool ToUpdate = true;

            Season season = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
            // DAL layer should be used
            //_seasonRepo.GetSeasonByName(seasonName);


            LeagueSeason leagueSeason = null;
            League league = null;
            if (season == null)
            {
                season = new Season();
                season.Name = seasonName;
                season.SeasonStartYear = Int32.Parse(seasonName);
                //NOTE: remove guid
                season.Id = Guid.NewGuid();
                _db.Seasons.Add(season);
            }


            //NOTE: DAL 
            //_seasonRepo.Add(season);

            /*use league repo instead of context*/
            List<League> leagues = _db.Leagues.Where(l => l.Name == leagueName).ToList();

            // DAL
            //_leagueRepo.GetLeaguesByName(leagueName).ToList();
            if (leagues.Count == 0)
            {
                league = new League();
                //NOTE:remove guid
                league.Id = Guid.NewGuid(); ;
                league.Name = leagueName;
                //league = 
                _db.Leagues.Add(league);
                //_db.Leagues.
                // DAL
                //_leagueRepo.CreateLeague(leagueName);
            }
            else
            {
                //List<Guid> leaguesIds = leagues.Select(l => l.Id).ToList();
                //this must be changed - only for testing added

                leagueSeason = _db.LeagueSeasons.Where(ls => ls.SeasonId == season.Id && ls.League.Name == leagueName).FirstOrDefault();
            }
            if (leagueSeason != null)
            {
                league = _db.Leagues.Where(l => l.Id == leagueSeason.LeagueId).FirstOrDefault();
            }
            else
            {

                //DAL 
                //_seasonRepo.GetLeagueSeasonsBySeasonAndLeague(season.Id, leaguesIds);
                //

                //league = _leagueRepo.CreateLeague(leagueName);
                leagueSeason = new LeagueSeason();
                //NOTE:remove guid
                leagueSeason.Id = Guid.NewGuid(); ;
                leagueSeason.SeasonId = season.Id;
                leagueSeason.LeagueId = league.Id;
                _db.LeagueSeasons.Add(leagueSeason);


            }
            if (leagueSeason.Rounds == null)
            {
                leagueSeason.Rounds = new List<Round>();
            }

            List<Team> teams = PopulateTeamEntityModel(loadedData, leagueSeason);

            if (leagueSeason.Teams != null)
            {
                Round round = null;
                int roundCounter = leagueSeason.Rounds.Count;
                foreach (Team team in teams)
                {
                    if (!leagueSeason.Teams.Contains(team))
                    {
                        leagueSeason.Teams.Add(team);
                        round = new Round();
                        round.Id = Guid.NewGuid();
                        round.RoundName = (roundCounter + 1).ToString();
                        roundCounter++;
                        round.LeagueSeason = leagueSeason;
                        _db.Rounds.Add(round);

                    }
                }
            }
            else
            {
                leagueSeason.Teams = teams;
            }
            //DAL
            //_seasonRepo.AddLeagueToSeason(leagueSeason);

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private List<Team> PopulateTeamEntityModel(ExL.ExcelLoader loadedData, LeagueSeason leagueSeason)
        {
            List<Team> teamsList = new List<Team>();
            bool isAdded = false;
            foreach (KeyValuePair<string, HashSet<string>> keyValuePair in loadedData.TeamAndPlayers)
            {
                isAdded = false;
                Team team = _db.Teams.Where(t => t.TeamName.Equals(keyValuePair.Key) && t.LeagueSeason == leagueSeason).FirstOrDefault();
                //t.LeagueSeason.Season.Name.Equals(loadedData.SeasonName)).FirstOrDefault();
                //DAL
                //_teamRepo.FindTeam(keyValuePair.Key, loadedData.SeasonName);
                if (team == null)
                {
                    team = new Team();
                    //NOTE:remove guid
                    team.Id = Guid.NewGuid(); ;
                    team.TeamName = keyValuePair.Key;
                    team.LeagueSeason = leagueSeason;
                    isAdded = true;
                }


                team.Players = GeneratePlayers(loadedData, keyValuePair.Value, team.Id);

                if (isAdded)
                {
                    _db.Teams.Add(team);
                    //DAL
                    //_teamRepo.Add(team);
                }
                else
                {
                    //handle update
                    //DAL
                    //_teamRepo.Update(team);
                }
                teamsList.Add(team);
            }

            return teamsList;

        }

        private List<Player> GeneratePlayers(ExL.ExcelLoader loadedData, HashSet<string> playersIds, Guid teamId)
        {
            List<Player> playerList = new List<Player>(playersIds.Count);

            foreach (string playerId in playersIds)
            {
                List<PlayerScore> scores = loadedData.GetPlayerScoreList(playerId);
                if (scores != null && scores.Count > 0)
                {
                    Player p = PopulatePlayerEntityModel(scores, teamId);
                    playerList.Add(p);
                }
            }
            return playerList;
        }


        private Player PopulatePlayerEntityModel(List<PlayerScore> playerScores, Guid teamId)
        {
            Player player = null;
            //BasketballDbContext db = new BasketballDbContext();
            //_playerRepo.GetPlayerByName()
            ///_teamRepo.
            ///

            PlayerScore pScore = playerScores.First();

            player = _db.Players.Where(p => p.Name == pScore.FirstName && p.LastName == pScore.LastName && p.MiddleName == pScore.MiddleName && p.Team_Id == teamId).FirstOrDefault();
            //_playerRepo.GetPlayerByName(pScore.FirstName, pScore.LastName, pScore.MiddleName);
            if (player == null)
            {
                player = new Player();
                //NOTE:remove guid
                player.Id = Guid.NewGuid(); ;
                player.Name = pScore.FirstName;
                player.LastName = pScore.LastName;
                player.MiddleName = pScore.MiddleName;
                //player.Team_Id = pScore.
                //_playerRepo.Add(player);
                _db.Players.Add(player);

            }
            //player.PlayersPerSeason.
            /*
            Stats stats;
            foreach (ExL.PlayerScore playerScore in playerScores)
            {
                //TO DO (set Stats)
                stats = PopulateStats(playerScore);
                //stats.
                if (player.Stats == null) player.Stats = new List<Stats>();
                player.Stats.Add(stats);
            }*/

            return player;
        }

        //private Game PopulateGame()
        //{
        //    Game game = new Game() {

        //    };

        //}

        private Stats PopulateStats(PlayerScore ps)
        {
            Stats stats = new Stats()
            {
                Ast = ps.Assistance,
                Blk = ps.Block,
                DReb = ps.DefensiveReb,
                FtMade = ps.FreeThrowsMade,
                FtMissed = ps.FreeThrowsAttempt,
                MinutesPlayed = ps.Minutes,
                JerseyNumber = ps.Number.ToString(),
                OReb = ps.OffensiveReb,
                TwoPtMissed = ps.PointAttempt2,
                TwoPtMade = ps.PointMade2,
                ThreePtMissed = ps.PointAttempt3,
                ThreePtMade = ps.PointMade3,
                Stl = ps.Steal,
                To = ps.TurnOver,


            };
            //db.Stats.Add(stats);
            return stats;
        }


        private MemoryStream GetFileAsMemoryStream(HttpPostedFileBase uploadedFile)
        {
            var buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);
            MemoryStream memStr = new MemoryStream(buf);
            return memStr;
        }
    }
}