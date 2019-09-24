using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.ExcelLoaderModels;
using LZRNS.DomainModels.Models;
using LZRNS.ExcelLoader;
using LZRNS.ExcelLoader.ExcelReader;
using LZRNS.Models.AdditionalModels.Forms;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private readonly BasketballDbContext _db = new BasketballDbContext();

        private League CurrentLeagueCache { get; set; }
        private LeagueSeason CurrentLeagueSeasonCache { get; set; }
        private Season CurrentSeasonCache { get; set; }
        private HashSet<Game> GamesCache { get; set; } = new HashSet<Game>();
        private Dictionary<string, Player> PlayerByUIdCache { get; set; } = new Dictionary<string, Player>();
        private HashSet<PlayerPerTeam> PlayersPerTeamCache { get; set; } = new HashSet<PlayerPerTeam>();
        private HashSet<Stats> PlayerStatsCache { get; set; } = new HashSet<Stats>();
        private Dictionary<string, Round> RoundByRoundNameCache { get; set; } = new Dictionary<string, Round>();
        private Dictionary<string, Team> TeamByTeamNameCache { get; set; } = new Dictionary<string, Team>();

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

        private static bool IsFilePlayersCodeList(string fileName)
        {
            return fileName.Split('-').Last().Contains("codelist");
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

        private void CreateSeasonLeagueAndLeagueSeason(string seasonName, string leagueName)
        {
            if (CurrentSeasonCache is null)
            {
                CurrentSeasonCache = CreateOrGetSeason(seasonName);
            }

            if (CurrentLeagueCache == null)
            {
                CurrentLeagueCache = CreateLeague(leagueName);
            }

            if (CurrentLeagueSeasonCache == null)
            {
                CurrentLeagueSeasonCache = CreateLeagueSeason(CurrentSeasonCache.Id, CurrentLeagueCache.Id);
            }
        }

        private void CreateTeamsRoundsAndGames(ExL.ExcelLoader loader, LeagueSeason leagueSeason)
        {
            foreach (var teamScore in loader.TeamStatisticByTeamName)
            {
                string teamName = teamScore.Key;
                foreach (var score in teamScore.Value.TeamScores)
                {
                    var round = CreateOrGetRound(leagueSeason.Id, score.RoundName);
                    var teamA = CreateOrGetTeamByName(teamName, leagueSeason);
                    var teamB = CreateOrGetTeamByName(score.AgainstTeam, leagueSeason);
                    CreateOrGetGame(leagueSeason.Season, round, teamA, teamB, score.MatchDate);
                }
            }
        }

        private List<Player> GeneratePlayersData(ExL.ExcelLoader loadedData, List<PlayerInfo> playerInfoList, HashSet<string> playersIds, Team team, LeagueSeason leagueSeason)
        {
            var playerList = new List<Player>(playersIds.Count);
            var games = GamesCache.Where(g => AreEqualById(g.TeamA, team) || AreEqualById(g.TeamB, team)).ToList();
            foreach (var playerInfo in playerInfoList)
            {
                var player = CreateOrGetPlayer(playerInfo);

                foreach (var playerScore in loadedData.GetPlayerScoreList(team.TeamName, playerInfo.NameAndLastName))
                {
                    string againstTeam = playerScore.AgainstTeam;
                    var game = games.Find(g => IsTeamInGame(g, againstTeam));
                    if (game != null)
                    {
                        var stats = CreateOrGetStatsForPlayer(playerScore, player, game, playerInfo.OnLoan);
                        player.Stats.Add(stats);
                    }
                }
                playerList.Add(player);
                CreateOrGetPlayerPerTeam(player, team, leagueSeason);
            }
            return playerList;
        }

        private MemoryStream GetFileAsMemoryStream(HttpPostedFileBase uploadedFile)
        {
            byte[] buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);
            return new MemoryStream(buf);
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

            Log4NetLogger.Log.Info($"Importing data for season {loader.SeasonName} and league {loader.LeagueName}");
            LoadDataToCache(loader.SeasonName, loader.LeagueName);
            PopulateEntityModel(loader, codingListLoader, loader.SeasonName, loader.LeagueName);
            return View(new ImportModel(CurrentPage));
        }

        private void PopulateEntityModel(ExL.ExcelLoader loadedData, CodingListLoader playerListData, string seasonName, string leagueName)
        {
            CreateSeasonLeagueAndLeagueSeason(seasonName, leagueName);
            _db.SaveChanges();

            CreateTeamsRoundsAndGames(loadedData, CurrentLeagueSeasonCache);
            _db.SaveChanges();

            PopulateTeamEntityModel(loadedData, playerListData, CurrentLeagueSeasonCache);
            _db.SaveChanges();
        }

        private void PopulateTeamEntityModel(ExL.ExcelLoader loadedData, CodingListLoader codingListData, LeagueSeason leagueSeason)
        {
            foreach (var keyValuePair in loadedData.PlayerNamesByTeamName)
            {
                string teamName = keyValuePair.Key;
                var playerNames = keyValuePair.Value;
                var playerInfoList = codingListData.PlayerInfoList.Where(pi => pi.NewTeamName.Equals(teamName)).ToList();
                var team = CreateOrGetTeamByName(teamName, leagueSeason);
                team.Players = GeneratePlayersData(loadedData, playerInfoList, playerNames, team, leagueSeason);
            }
        }

        #region CacheLoading

        private void LoadCurrentLeagueSeasonCache(Guid seasonId, Guid leagueId)
        {
            CurrentLeagueSeasonCache = _db.LeagueSeasons.Include(ls => ls.Season).Include(ls => ls.League)
                            .FirstOrDefault(ls => seasonId.Equals(ls.SeasonId) && leagueId.Equals(ls.LeagueId));
        }

        private void LoadDataToCache(string seasonName, string leagueName)
        {
            CurrentSeasonCache = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
            CurrentLeagueCache = _db.Leagues.FirstOrDefault(l => l.Name.Equals(leagueName));

            if (CurrentSeasonCache == null || CurrentLeagueCache == null)
            {
                return;
            }

            LoadCurrentLeagueSeasonCache(CurrentSeasonCache.Id, CurrentLeagueCache.Id);

            if (CurrentLeagueSeasonCache == null)
            {
                return;
            }

            var leagueSeasonId = CurrentLeagueSeasonCache.Id;

            LoadGamesCache(leagueSeasonId);
            LoadPlayerRelatedCache(leagueSeasonId);
            LoadRoundByRoundNameCache(leagueSeasonId);
            LoadTeamByTeamNameCache(leagueSeasonId);
        }

        private void LoadGamesCache(Guid leagueSeasonId)
        {
            var games = _db.Games.Include(g => g.Round).Where(g => g.Round.LeagueSeasonId != leagueSeasonId).ToList();
            GamesCache = new HashSet<Game>(games);
        }

        private void LoadPlayerRelatedCache(Guid leagueSeasonId)
        {
            LoadPlayersPerTeamCache(leagueSeasonId);

            PlayerByUIdCache = _db.Players.ToDictionary(keySelector: player => player.UId);

            var playerIds = PlayerByUIdCache.Values.Select(player => player.Id).Distinct().ToArray();
            LoadPlayerStatsCache(playerIds);
        }

        private void LoadPlayersPerTeamCache(Guid leagueSeasonId)
        {
            var playerPerTeams = _db.PlayersPerTeam.Where(ppt => ppt.LeagueSeason_Id == leagueSeasonId).ToList();
            PlayersPerTeamCache = new HashSet<PlayerPerTeam>(playerPerTeams);
        }

        private void LoadPlayerStatsCache(Guid[] playerIds)
        {
            var playerStats = _db.Stats.Where(stat => playerIds.Contains(stat.PlayerId)).ToList();
            PlayerStatsCache = new HashSet<Stats>(playerStats);
        }

        private void LoadRoundByRoundNameCache(Guid leagueSeasonId)
        {
            var rounds = _db.Rounds.Where(r => r.LeagueSeasonId.Equals(leagueSeasonId));
            RoundByRoundNameCache = rounds.ToDictionary(keySelector: round => FormatRoundName(round.RoundName));
        }

        private void LoadTeamByTeamNameCache(Guid leagueSeasonId)
        {
            var teams = _db.Teams.Where(t => t.LeagueSeasonId.Equals(leagueSeasonId));
            TeamByTeamNameCache = teams.ToDictionary(keySelector: team => FormatTeamName(team.TeamName));
        }

        #endregion CacheLoading

        #region Entity creation helper methods

        private static bool AreEqualById(Round lhs, Round rhs)
        {
            return lhs?.Id.Equals(rhs?.Id) == true;
        }

        private static bool AreEqualById(Season lhs, Season rhs)
        {
            return lhs?.Id.Equals(rhs?.Id) == true;
        }

        private static bool AreEqualById(Team lhs, Team rhs)
        {
            return lhs?.Id.Equals(rhs?.Id) == true;
        }

        private static bool AreInSameGame(Team teamA, Team teamB, Game g)
        {
            return (AreEqualById(g.TeamA, teamA) && AreEqualById(g.TeamB, teamB))
                || (AreEqualById(g.TeamA, teamB) && AreEqualById(g.TeamB, teamA));
        }

        private static string DefaultIfNullOrWhiteSpace(string str, string @default = "?")
        {
            return string.IsNullOrWhiteSpace(str) ? @default : str;
        }

        private static DateTime FormatGameDate(DateTime gameDateTime)
        {
            //if date time format is bad and initial datetime value is set, it will produce SQL exception, so change it
            var defaultDate = new DateTime(1, 1, 1, 0, 0, 0);
            if (DateTime.Compare(gameDateTime, defaultDate) == 0)
            {
                gameDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            }

            return gameDateTime;
        }

        private static string FormatRoundName(string roundName)
        {
            return (roundName.Contains("GAME") && roundName.Length >= 5) ? roundName.Substring(4) : roundName;
        }

        private static string FormatTeamName(string teamName)
        {
            return teamName?.ToLower().Trim() ?? "";
        }

        private static bool GetGameComparison(Game game, Season season, Round round, Team teamA, Team teamB, DateTime gameDateTime)
        {
            return AreEqualById(game.Season, season)
                && AreEqualById(game.Round, round)
                && AreInSameGame(teamA, teamB, game)
                && DateTime.Compare(game.DateTime, gameDateTime) == 0;
        }

        private static bool IsTeamInGame(Game game, string teamName)
        {
            string formattedTeamName = FormatTeamName(teamName);
            string teamAName = FormatTeamName(game?.TeamA?.TeamName);
            string teamBName = FormatTeamName(game?.TeamB?.TeamName);
            return formattedTeamName.Equals(teamAName) || formattedTeamName.Equals(teamBName);
        }

        private League CreateLeague(string leagueName)
        {
            var league = new League()
            {
                Id = Guid.NewGuid(),
                Name = leagueName
            };

            _db.Leagues.Add(league);

            return league;
        }

        private LeagueSeason CreateLeagueSeason(Guid seasonId, Guid leagueId)
        {
            var leagueSeason = new LeagueSeason()
            {
                Id = Guid.NewGuid(),
                SeasonId = seasonId,
                LeagueId = leagueId,
                Rounds = new List<Round>()
            };

            _db.LeagueSeasons.Add(leagueSeason);

            return leagueSeason;
        }

        private Game CreateOrGetGame(Season season, Round round, Team teamA, Team teamB, DateTime gameDate)
        {
            var formattedGameDate = FormatGameDate(gameDate);

            var game = GamesCache.FirstOrDefault(g => GetGameComparison(g, season, round, teamA, teamB, formattedGameDate));

            if (game != null)
            {
                return game;
            }

            game = new Game()
            {
                Id = Guid.NewGuid(),
                Season = season,
                Round = round,
                TeamA = teamA,
                TeamB = teamB,
                DateTime = formattedGameDate
            };

            GamesCache.Add(game);

            _db.Games.Add(game);

            return game;
        }

        private Player CreateOrGetPlayer(PlayerInfo info)
        {
            if (PlayerByUIdCache.TryGetValue(info.UId, out var player))
            {
                return player;
            }

            player = new Player()
            {
                Id = Guid.NewGuid(),
                Name = DefaultIfNullOrWhiteSpace(info.FirstName),
                MiddleName = DefaultIfNullOrWhiteSpace(info.MiddleName),
                LastName = DefaultIfNullOrWhiteSpace(info.LastName),
                UId = info.UId,
                Stats = new List<Stats>(),
            };

            PlayerByUIdCache.Add(player.UId, player);

            _db.Players.Add(player);

            return player;
        }

        private PlayerPerTeam CreateOrGetPlayerPerTeam(Player player, Team team, LeagueSeason leagueSeason)
        {
            var playerPerTeam = PlayersPerTeamCache.FirstOrDefault(ppt => ppt.Player.Id == player.Id && ppt.Team.Id == team.Id && ppt.LeagueSeason_Id == leagueSeason.Id);

            if (playerPerTeam != null)
            {
                return playerPerTeam;
            }

            playerPerTeam = new PlayerPerTeam()
            {
                Id = Guid.NewGuid(),
                Player = player,
                Team = team,
                LeagueSeason = leagueSeason
            };

            PlayersPerTeamCache.Add(playerPerTeam);

            _db.PlayersPerTeam.Add(playerPerTeam);

            return playerPerTeam;
        }

        private Round CreateOrGetRound(Guid leagueSeasonId, string roundName)
        {
            string formattedRoundName = FormatRoundName(roundName);
            RoundByRoundNameCache.TryGetValue(formattedRoundName, out var round);

            if (round != null)
            {
                return round;
            }

            round = new Round()
            {
                Id = Guid.NewGuid(),
                RoundName = formattedRoundName,
                LeagueSeasonId = leagueSeasonId,
                Games = new List<Game>()
            };

            RoundByRoundNameCache.Add(formattedRoundName, round);

            _db.Rounds.Add(round);

            return round;
        }

        private Season CreateOrGetSeason(string seasonName)
        {
            var season = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
            return season ?? CreateSeason(seasonName);
        }

        private Stats CreateOrGetStatsForPlayer(PlayerScore playScore, Player player, Game game, bool onLoan)
        {
            var stats = PlayerStatsCache.FirstOrDefault(st => st.Player.Id == player.Id && st.Game.Id == game.Id);

            if (stats != null)
            {
                return stats;
            }

            stats = new Stats()
            {
                Id = Guid.NewGuid(),
                Player = player,
                Game = game,
                Ast = playScore.Assistance,
                Blk = playScore.Block,
                DReb = playScore.DefensiveReb,
                FtMade = playScore.FreeThrowsMade,
                FtMissed = playScore.FreeThrowsAttempt,
                MinutesPlayed = playScore.Minutes,
                JerseyNumber = playScore.Number.ToString(),
                OReb = playScore.OffensiveReb,
                TwoPtMissed = playScore.PointAttempt2,
                TwoPtMade = playScore.PointMade2,
                ThreePtMissed = playScore.PointAttempt3,
                ThreePtMade = playScore.PointMade3,
                Stl = playScore.Steal,
                To = playScore.TurnOver,
                OnLoan = onLoan,
            };

            PlayerStatsCache.Add(stats);

            _db.Stats.Add(stats);

            return stats;
        }

        private Team CreateOrGetTeamByName(string teamName, LeagueSeason leagueSeason)
        {
            string formattedTeamName = FormatTeamName(teamName);
            if (TeamByTeamNameCache.TryGetValue(formattedTeamName, out var team))
            {
                return team;
            }
            team = CreateTeam(teamName, leagueSeason);
            TeamByTeamNameCache.Add(formattedTeamName, team);
            return team;
        }

        private Season CreateSeason(string seasonName)
        {
            var season = new Season()
            {
                Id = Guid.NewGuid(),
                Name = seasonName,
                SeasonStartYear = seasonName.ExtractNumber()
            };

            _db.Seasons.Add(season);
            _db.SaveChanges();

            return season;
        }

        private Team CreateTeam(string teamName, LeagueSeason leagueSeason)
        {
            var team = new Team()
            {
                Id = Guid.NewGuid(),
                TeamName = teamName,
                LeagueSeason = leagueSeason
            };

            _db.Teams.Add(team);

            return team;
        }

        #endregion Entity creation helper methods
    }
}