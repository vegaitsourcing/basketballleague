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
using LZRNS.Models.AdditionalModels.Forms;
using System;
using LZRNS.ExcelLoader;
using LZRNS.ExcelLoader.Model;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class ImportController : RenderMvcController
    {

        //TODO: refactor - use repositories instead of context
        private BasketballDbContext _db = new BasketballDbContext();

        private Dictionary<string, Team> _createdTeams = new Dictionary<string, Team>();

        private Dictionary<string, Team> NewTeams { get { return _createdTeams; } set { _createdTeams = value; } }
        private HashSet<Game> NewGames { get; set; } = new HashSet<Game>();
        private Dictionary<string, Round> NewRounds { get; set; } = new Dictionary<string, Round>();
        private Dictionary<string, Player> NewPlayers { get; set; } = new Dictionary<string, Player>();
        private HashSet<PlayerPerTeam> NewPlayerPerTeamSet { get; set; } = new HashSet<PlayerPerTeam>();
        private HashSet<Stats> NewPlayersStats { get; set; } = new HashSet<Stats>();



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
            ExL.AbstractExcelLoader loader = null;
            ExL.CodingListLoader codingListLoader = null;
            TimeTableLoader.Converter.Converter converter = new TimeTableLoader.Converter.Converter();
            ActionResult response = null;

            //here, there are two paths to execute 
            //1) if player code list is in files - importing
            //2) if there is not - analyzing
            bool doImport = false;
            if (model.Files.Select(file => IsFilePlayersCodeList(file.FileName)).Contains(true))
            {
                //importer
                loader = new ExL.ExcelLoader(Server.MapPath("~/App_Data/TableMapper.config"));
                doImport = true;
                codingListLoader = new ExL.CodingListLoader(Server.MapPath("~/App_Data/TableMapper_coding_list.config"));

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
                    else if (file.FileName.Contains("codelist"))
                    {
                        codingListLoader.ProcessFile(memStr, file.FileName);

                    }
                    else
                    {
                        loader.ProcessFile(memStr, file.FileName);
                    }
                }
            }

            if (doImport)
            {
                PopulateEntityModel((ExL.ExcelLoader)loader, codingListLoader, loader.SeasonName, loader.LeagueName);
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

        private void PopulateEntityModel(ExL.ExcelLoader loadedData, ExL.CodingListLoader playerListData, string seasonName, string leagueName)
        {
            //use season repo instead of context; DAL layer should be used

            Season season = CreateOrGetSeason(seasonName);
            LeagueSeason leagueSeason = null;
            League league = null;

            leagueSeason = _db.LeagueSeasons.Where(ls => ls.Season.Id == season.Id && ls.League.Name.Equals(leagueName)).FirstOrDefault();

            if (leagueSeason != null)
            {
                league = _db.Leagues.Where(l => l.Id == leagueSeason.LeagueId).FirstOrDefault();
            }
            else
            {
                league = CreateLeague(leagueName);
                leagueSeason = CreateLeagueSeason(season.Id, league.Id);
                _db.LeagueSeasons.Add(leagueSeason);

            }
            leagueSeason.Rounds = leagueSeason.Rounds ?? new List<Round>();
            CreateTeamsRoundsAndGames(loadedData, leagueSeason);
            PopulateTeamEntityModel(loadedData, playerListData, leagueSeason);
            //transactional
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private List<Team> PopulateTeamEntityModel(ExL.ExcelLoader loadedData, ExL.CodingListLoader codingListData, LeagueSeason leagueSeason)
        {
            List<Team> teamsList = new List<Team>();
            foreach (KeyValuePair<string, HashSet<string>> keyValuePair in loadedData.TeamAndPlayers)
            {
                Team team = null;
                NewTeams.TryGetValue(keyValuePair.Key, out team);
                List<PlayerInfo> infoList = codingListData.PlayerInfoList.Where(pi => pi.NewTeamName.Equals(team.TeamName)).ToList();
                team.Players = GeneratePlayersData(loadedData, infoList, keyValuePair.Value, team, leagueSeason);
                teamsList.Add(team);
            }

            return teamsList;

        }

        private List<Player> GeneratePlayersData(ExL.ExcelLoader loadedData, List<PlayerInfo> playerInfoList, HashSet<string> playersIds, Team team, LeagueSeason leagueSeason)
        {
            List<Player> playerList = new List<Player>(playersIds.Count);
            Player player;
            List<Game> games = NewGames.Where(g => g.TeamA.Id == team.Id || g.TeamB.Id == team.Id).ToList();
            Game game = null;
            foreach (PlayerInfo info in playerInfoList)
            {
                player = CreateOrGetPlayer(info);
                //player.Team = team;

                
                List<PlayerScore> scores = loadedData.GetPlayerScoreList(team.TeamName, info.NameAndLastName);
                foreach (PlayerScore playerScore in scores)
                {
                    game = games.FirstOrDefault(g => g.TeamA.TeamName.Equals(playerScore.AgainstTeam) || g.TeamB.TeamName.Equals(playerScore.AgainstTeam));
                    if (game != null)
                    {
                        Stats stats = CreateOrGetStatsForPlayer(playerScore, player, game, info.OnLoan);
                        player.Stats.Add(stats);
                    }

                }
                playerList.Add(player);
                PlayerPerTeam playerPerTeam = CreateOrGetPlayerPerTeam(player, team, leagueSeason);
            }
            return playerList;
        }


        #region Entity creation helper methods
        private Season CreateOrGetSeason(string seasonName)
        {
            Season season = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));
            if (season == null)
            {
                season = new Season()
                {
                    Id = Guid.NewGuid(),
                    Name = seasonName,
                    SeasonStartYear = Int32.Parse(seasonName)

                };
                _db.Seasons.Add(season);

            }

            return season;
        }
        private League CreateLeague(string leagueName)
        {
            League league = new League()
            {
                Id = Guid.NewGuid(),
                Name = leagueName

            };
            _db.Leagues.Add(league);
            return league;

        }
        private LeagueSeason CreateLeagueSeason(Guid seasonId, Guid leagueId)
        {
            LeagueSeason leagueSeason = new LeagueSeason()
            {
                Id = Guid.NewGuid(),
                SeasonId = seasonId,
                LeagueId = leagueId,
                Rounds = new List<Round>()
            };
            _db.LeagueSeasons.Add(leagueSeason);
            return leagueSeason;
        }


        private Team CreateTeam(string teamName, LeagueSeason leagueSeason)
        {
            Team team = new Team()
            {
                Id = Guid.NewGuid(),
                TeamName = teamName,
                LeagueSeason = leagueSeason
            };

            _db.Teams.Add(team);

            return team;
        }

        private void CreateOrGetTeamsByNames(string teamNameA, string teamNameB, LeagueSeason leagueSeason, out Team teamA, out Team teamB)
        {
            //check in cache first
            NewTeams.TryGetValue(teamNameA, out teamA);
            NewTeams.TryGetValue(teamNameB, out teamB);
            //then in database
            if (teamA == null)
            {
                teamA = _db.Teams.FirstOrDefault(t => t.TeamName.Equals(teamNameA));
                if (teamA == null)
                {
                    teamA = CreateTeam(teamNameA, leagueSeason);// && t.LeagueSeason.Id == leagueSeason.Id);
                    //_db.Teams.Add(teamA);
                }
                else
                {
                    teamA.LeagueSeason = leagueSeason;
                }
                NewTeams.Add(teamNameA, teamA);
            }

            if (teamB == null)
            {
                teamB = _db.Teams.FirstOrDefault(t => t.TeamName.Equals(teamNameB));
                if (teamB == null)
                {
                    teamB = CreateTeam(teamNameB, leagueSeason);
                   // _db.Teams.Add(teamA);
                }
                else
                {
                    teamB.LeagueSeason = leagueSeason;
                }

                NewTeams.Add(teamNameB, teamB);
            }

        }
        private Player CreateOrGetPlayer(PlayerInfo info)
        {
            //check in cache
            Player player = null;
            NewPlayers.TryGetValue(info.UId, out player);
            if (player == null)
            {
                //check in database - previous season import or double import
                player = _db.Players.FirstOrDefault(pl => pl.UId == info.UId);
                if (player == null)
                {
                    player = new Player()
                    {
                        Id = Guid.NewGuid(),
                        Name = info.FirstName,
                        LastName = (String.IsNullOrEmpty(info.LastName) || String.IsNullOrWhiteSpace(info.LastName)) ? info.MiddleName : info.LastName,
                        //in some examples, middlename has value of lastname, check it later, temporary hack
                        MiddleName = (String.IsNullOrEmpty(info.LastName) || String.IsNullOrWhiteSpace(info.LastName)) ? info.LastName : info.MiddleName,
                        UId = info.UId,
                        Stats = new List<Stats>(),
                    }
                    ;
                    _db.Players.Add(player);

                }
                //put object in cache
                NewPlayers.Add(player.UId, player);
            }
            return player;
        }

        private PlayerPerTeam CreateOrGetPlayerPerTeam(Player player, Team team, LeagueSeason leagueSeason)
        {
            PlayerPerTeam playerPerTeam = NewPlayerPerTeamSet.FirstOrDefault(ppt => ppt.Player.Id == player.Id && ppt.Team.Id == team.Id && ppt.LeagueSeason_Id == leagueSeason.Id);

            if (playerPerTeam == null)
            {
                playerPerTeam = _db.PlayersPerTeam.FirstOrDefault(ppt => ppt.Player.Id == player.Id && ppt.Team.Id == team.Id && ppt.LeagueSeason_Id == leagueSeason.Id);
                if (playerPerTeam == null)
                {
                    playerPerTeam = new PlayerPerTeam()
                    {
                        Id = Guid.NewGuid(),
                        Player = player,
                        Team = team,
                        LeagueSeason = leagueSeason
                    };
                    _db.PlayersPerTeam.Add(playerPerTeam);

                }
                NewPlayerPerTeamSet.Add(playerPerTeam);

            }
            return playerPerTeam;
        }

        private Round CreateOrGetRound(Guid leagueSeasonId, string roundName)
        {
            Round round = null;
            //roundname is determined by sheet name - usually GAMEx where x is round number, so use only number part
            roundName = (roundName.Contains("GAME") && roundName.Length >= 5) ? roundName.Substring(4) : roundName;
            NewRounds.TryGetValue(roundName, out round);

            if (round == null)
            {
                round = _db.Rounds.FirstOrDefault(r => r.LeagueSeason.Id == leagueSeasonId && r.RoundName.Equals(roundName));
                if (round == null)
                {
                    round = new Round()
                    {
                        Id = Guid.NewGuid(),
                        RoundName = roundName,
                        LeagueSeasonId = leagueSeasonId,
                        Games = new List<Game>()
                    };
                    _db.Rounds.Add(round);
                }
                NewRounds.Add(roundName, round);

            }
            return round;
        }


        private Game CreateOrGetGame(Season season, Round round, Team teamA, Team teamB, DateTime gameDateTime)
        {
            //if date time format is bad and initial datetime value is set, it will produce SQL exception, so change it
            DateTime defaultDate = new DateTime(1, 1, 1, 0, 0, 0);
            if (DateTime.Compare(gameDateTime, defaultDate) == 0)
            {
                gameDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            }

            Game game = null;
            game = NewGames.FirstOrDefault(g => g.Season.Id == season.Id && g.Round.Id == round.Id && (g.TeamA.Id == teamA.Id && g.TeamB.Id == teamB.Id) ||
                    (g.TeamA.Id == teamB.Id && g.TeamB.Id == teamA.Id) && DateTime.Compare(g.DateTime, gameDateTime) == 0);
            if (game == null)
            {
                game = _db.Games.FirstOrDefault(g => g.Season.Id == season.Id && g.Round.Id == round.Id && (g.TeamA.Id == teamA.Id && g.TeamB.Id == teamB.Id) ||
                        (g.TeamA.Id == teamB.Id && g.TeamB.Id == teamA.Id) && DateTime.Compare(g.DateTime, gameDateTime) == 0);
                if (game == null)
                {
                    game = new Game()
                    {
                        Id = Guid.NewGuid(),
                        Season = season,
                        Round = round,
                        TeamA = teamA,
                        TeamB = teamB,
                        DateTime = gameDateTime
                    };
                    _db.Games.Add(game);
                }
                NewGames.Add(game);

            }

            return game;
        }
        #endregion

        private void CreateTeamsRoundsAndGames(ExL.ExcelLoader loader, LeagueSeason leagueSeason)
        {
            Round round = null;
            string teamName = null;
            string opponentTeamName = null;
            DateTime matchDate;
            string roundName = null;
            Team teamA = null, teamB = null;
            foreach (KeyValuePair<string, TeamStatistic> teamScore in loader.Teams)
            {
                teamName = teamScore.Key;
                foreach (TeamScore score in teamScore.Value.TeamScores)
                {
                    opponentTeamName = score.AgainstTeam;
                    matchDate = score.MatchDate;
                    roundName = score.RoundName;
                    //create round
                    round = CreateOrGetRound(leagueSeason.Id, roundName);
                    CreateOrGetTeamsByNames(teamName, opponentTeamName, leagueSeason, out teamA, out teamB);

                    //create game
                    CreateOrGetGame(leagueSeason.Season, round, teamA, teamB, matchDate);
                    //NewGames.Add(game);

                }
            }
        }




        private Stats CreateOrGetStatsForPlayer(PlayerScore playScore, Player player, Game game, bool onLoan)
        {

            Stats stats = null;
            stats = NewPlayersStats.FirstOrDefault(st => st.Player.Id == player.Id && st.Game.Id == game.Id);

            if (stats == null)
            {
                stats = _db.Stats.FirstOrDefault(st => st.Player.Id == player.Id && st.Game.Id == game.Id);
                if (stats == null)
                {
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
                    _db.Stats.Add(stats);

                }
                NewPlayersStats.Add(stats);

            }
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