using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.ExcelLoader;
using LZRNS.ExcelLoader.ExcelReader;
using LZRNS.ExcelLoader.Model;
using LZRNS.Models.AdditionalModels.Forms;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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

        private static bool ShouldImport(ImportFormModel model)
        {
            return model.Files.Any(file => IsFilePlayersCodeList(file.FileName));
        }

        private static bool IsFilePlayersCodeList(string fileName)
        {
            return fileName.Split('-').Last().Contains("codelist");
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
                var playerPerTeam = CreateOrGetPlayerPerTeam(player, team, leagueSeason);
            }
            return playerList;
        }

        private MemoryStream GetFileAsMemoryStream(HttpPostedFileBase uploadedFile)
        {
            byte[] buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);
            return new MemoryStream(buf);
        }

        private void LoadDataToCache(string seasonName, string leagueName)
        {
            var season = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
            var league = _db.Leagues.FirstOrDefault(l => l.Name.Equals(leagueName));

            if (season == null || league == null)
            {
                return;
            }

            var leagueSeason = _db.LeagueSeasons.FirstOrDefault(ls => season.Id.Equals(ls.SeasonId) && league.Id.Equals(ls.LeagueId));

            if (leagueSeason == null)
            {
                return;
            }
            var currLeagueSeasonGames = _db.Games.Include(g => g.Round).Where(g => g.Round.LeagueSeasonId != leagueSeason.Id).ToList();
            GamesCache = new HashSet<Game>(currLeagueSeasonGames);

            var playerPerTeams = _db.PlayersPerTeam.Where(ppt => ppt.LeagueSeason_Id == leagueSeason.Id).ToList();
            PlayersPerTeamCache = new HashSet<PlayerPerTeam>(playerPerTeams);

            PlayerByUIdCache = _db.Players.ToDictionary(keySelector: player => player.UId);

            var playerIds = PlayerByUIdCache.Values.Select(player => player.Id).Distinct().ToArray();
            var playerStats = _db.Stats.Where(stat => playerIds.Contains(stat.PlayerId)).ToList();
            PlayerStatsCache = new HashSet<Stats>(playerStats);

            var rounds = _db.Rounds.Where(r => r.LeagueSeasonId.Equals(leagueSeason.Id));
            RoundByRoundNameCache = rounds.ToDictionary(keySelector: round => FormatRoundName(round.RoundName));

            var teams = _db.Teams.Where(t => t.LeagueSeasonId.Equals(leagueSeason.Id));
            TeamByTeamNameCache = teams.ToDictionary(keySelector: team => FormatTeamName(team.TeamName));
        }

        private void PopulateEntityModel(ExL.ExcelLoader loadedData, CodingListLoader playerListData, string seasonName, string leagueName)
        {
            var season = CreateOrGetSeason(seasonName);

            var leagueSeason = _db.LeagueSeasons.Include(ls => ls.Season).Include(ls => ls.League)
                .FirstOrDefault(ls => ls.Season.Name == seasonName && ls.League.Name == leagueName);

            if (leagueSeason == null)
            {
                var league = CreateLeague(leagueName);
                leagueSeason = CreateLeagueSeason(season.Id, league.Id);
            }

            _db.SaveChanges();

            CreateTeamsRoundsAndGames(loadedData, leagueSeason);

            _db.SaveChanges();

            PopulateTeamEntityModel(loadedData, playerListData, leagueSeason);

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
            return teamName.ToLower().Trim();
        }

        private static bool GetGameComparison(Game game, Season season, Round round, Team teamA, Team teamB, DateTime gameDateTime)
        {
            return AreEqualById(game.Season, season)
                && AreEqualById(game.Round, round)
                && AreInSameGame(teamA, teamB, game)
                && DateTime.Compare(game.DateTime, gameDateTime) == 0;
        }

        private static bool IsTeamInGame(Game game, string team)
        {
            return game?.TeamA?.TeamName.Equals(team) == true || game?.TeamB?.TeamName.Equals(team) == true;
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

            if (season != null)
            {
                return season;
            }

            season = new Season()
            {
                Id = Guid.NewGuid(),
                Name = seasonName,
                SeasonStartYear = seasonName.ExtractNumber()
            };

            _db.Seasons.Add(season);
            _db.SaveChanges();

            return season;
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
            team = CreateTeam(formattedTeamName, leagueSeason);
            TeamByTeamNameCache.Add(formattedTeamName, team);
            return team;
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