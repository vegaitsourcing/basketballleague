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
using LZRNS.ExcelLoader.Model;
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
                //ExL.AbstractExcelLoader codingListLoader
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

            Season season = _db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName));


            LeagueSeason leagueSeason = null;
            League league = null;
            if (season == null)
            {
                season = new Season();
                season.Name = seasonName;
                season.SeasonStartYear = Int32.Parse(seasonName);
                //NOTE: remove guid when repo is activated
                season.Id = Guid.NewGuid();
                _db.Seasons.Add(season);
            }

            //List<League> leagues = _db.Leagues.Where(l => l.Name == leagueName).ToList();
            leagueSeason = _db.LeagueSeasons.Where(ls => ls.SeasonId == season.Id && ls.League.Name == leagueName).FirstOrDefault();

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

            List<Team> teams = PopulateTeamEntityModel(loadedData, playerListData, leagueSeason);
            PopulateRoundsAndGames(loadedData, leagueSeason);
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
                Team team = _db.Teams.Where(t => t.TeamName.Equals(keyValuePair.Key) && t.LeagueSeasonId == leagueSeason.Id).FirstOrDefault();
                //t.LeagueSeason.Season.Name.Equals(loadedData.SeasonName)).FirstOrDefault();
                //DAL
                //_teamRepo.FindTeam(keyValuePair.Key, loadedData.SeasonName);
                List<PlayerInfo> infoList = codingListData.PlayerInfoList.Where(pi => pi.NewTeamName == team.TeamName).ToList();
                if (team == null)
                {
                    team = CreateTeam(keyValuePair.Key, leagueSeason);
                }


                //team.Players = GeneratePlayers(loadedData, codingListData, keyValuePair.Value, team, leagueSeason);
                teamsList.Add(team);
            }

            return teamsList;

        }

        #region Helper methods
        //finish this

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
                LeagueSeason = leagueSeason,

            };

            _db.Teams.Add(team);
            return team;
        }

        private void FindTeamsByNames(string teamNameA, string teamNameB, LeagueSeason leagueSeason, out Team teamA, out Team teamB)
        {
            teamA = _db.Teams.FirstOrDefault(t => t.TeamName == teamNameA && t.LeagueSeasonId == leagueSeason.Id);
            teamB = _db.Teams.FirstOrDefault(t => t.TeamName == teamNameB && t.LeagueSeason.Id == leagueSeason.Id);

            //maybe this can be removed when complete file is uploaded
            if (teamA == null)
            {
                teamA = CreateTeam(teamNameA, leagueSeason);
            }

            if(teamB == null)
            {
                teamB = CreateTeam(teamNameB, leagueSeason);
            }

        }
        private Player CreatePlayer()
        {
            Player player = null;
            return player;
        }

        private Round CreateOrGetRound(Guid leagueSeasonId, string roundName)
        {
            Round round = _db.Rounds.FirstOrDefault(r => r.LeagueSeasonId == leagueSeasonId && r.RoundName == roundName);
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

            return round;
        }


        private Game CreateOrGetGame(Guid seasonId, Guid roundId, Guid teamAId, Guid teamBId, DateTime dt)
        {
            Game game = _db.Games.FirstOrDefault(g => g.SeasonId == seasonId && g.RoundId == roundId && g.TeamAId == teamAId && g.TeamBId == teamBId && g.DateTime == dt);
            if (game == null)
            {
                game = new Game()
                {
                    Id = Guid.NewGuid(),
                    SeasonId = seasonId,
                    RoundId = roundId,
                    TeamAId = teamAId,
                    TeamBId = teamBId,
                    DateTime = dt
                };
                _db.Games.Add(game);

            }

            return game;
        }
        #endregion

        private void PopulateRoundsAndGames(ExL.ExcelLoader loader, LeagueSeason leagueSeason)
        {
            Round round = null;
            Game game = null;
            string teamName = null;
            string opponentTeamName = null;
            DateTime matchDate;
            List<TeamStatistic> statistic = null;
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
                    //statistic = score.
                    //create round
                    round = CreateOrGetRound(leagueSeason.Id, roundName);
                    FindTeamsByNames(teamName, opponentTeamName, leagueSeason, out teamA, out teamB);

                    //create game
                    game = CreateOrGetGame(leagueSeason.SeasonId, round.Id, teamA.Id, teamB.Id, matchDate);


                }
            }
        }

        private List<Player> GeneratePlayers(ExL.ExcelLoader loadedData, ExL.CodingListLoader codingListData, HashSet<string> playersIds, Team team, LeagueSeason leagueSeason)
        {
            List<Player> playerList = new List<Player>(playersIds.Count);
            Player player;
            foreach (PlayerInfo info in codingListData.PlayerInfoList)
            {
                player = _db.Players.FirstOrDefault(pl => pl.UId == info.UId);
                //playersIds.FirstOrDefault(p => p.Equals(info.NameAndLastName));
                if (player == null)
                {
                    player = new Player();
                    player.Name = info.FirstName;
                    player.LastName = info.LastName;
                    player.MiddleName = info.MiddleName;
                    player.UId = info.UId;
                    player.Stats = player.Stats ?? new List<Stats>();
                    //player.Team = team;
                    Stats stats = null;
                    List<PlayerScore> scores = loadedData.GetPlayerScoreList(info.NameAndLastName);
                    foreach (PlayerScore playerScore in scores)
                    {
                        //TO DO (set Stats)
                        stats = PopulateStats(playerScore, info.OnLoan);
                        player.Stats.Add(stats);
                    }

                    _db.Players.Add(player);

                }

                PlayerPerTeam playerPerTeam = PopulatePlayerPerTeam(player, team, leagueSeason);


            }
            /*
            foreach (string playerId in playersIds)
            {
                List<PlayerScore> scores = loadedData.GetPlayerScoreList(playerId);
                if (scores != null && scores.Count > 0)
                {
                    Player p = PopulatePlayerEntityModel(scores, teamId);
                    playerList.Add(p);
                }
            }*/
            return playerList;
        }

        private PlayerPerTeam PopulatePlayerPerTeam(Player player, Team team, LeagueSeason leagueSeason)
        {

            PlayerPerTeam playerPerTeam = _db.PlayersPerTeam.FirstOrDefault(ppt => ppt.PlayerId == player.Id && ppt.TeamId == team.Id && ppt.LeagueSeason_Id == leagueSeason.Id);
            if (playerPerTeam == null)
            {
                playerPerTeam = new PlayerPerTeam();
                playerPerTeam.Player = player;
                playerPerTeam.Team = team;
                playerPerTeam.LeagueSeason = leagueSeason;
                playerPerTeam.Id = Guid.NewGuid();
                _db.PlayersPerTeam.Add(playerPerTeam);

            }
            return playerPerTeam;
        }
        /*
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

        //return player;
        //}

       

        private Stats PopulateStats(PlayerScore ps, bool onLoan)
        {
            Stats stats = new Stats()
            {
                Id = Guid.NewGuid(),
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
                OnLoan = onLoan,



            };
            _db.Stats.Add(stats);
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