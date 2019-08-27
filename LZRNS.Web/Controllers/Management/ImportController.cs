using LZRNS.Models.DocumentTypes.Pages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using ExL = LZRNS.ExcelLoader;

using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.AdditionalModels.Forms;
using System;

namespace LZRNS.Web.Controllers.Management
{
    [MemberAuthorize]
    public class ImportController : RenderMvcController
    {

        private ITeamRepository _teamRepo;
        private ISeasonRepository _seasonRepo;

        public ImportController(ITeamRepository teamRepo)
        {
            _teamRepo = teamRepo;

        }

        public ImportController(ITeamRepository teamRepo, ISeasonRepository seasonRepo)
        {
            _teamRepo = teamRepo;
            _seasonRepo = seasonRepo;

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

            ExL.ExcelLoader loader = new ExL.ExcelLoader(Server.MapPath("~/App_Data/TableMapper.config"));
            TimeTableLoader.Converter.Converter converter = new TimeTableLoader.Converter.Converter();
            //hardcoded
            //string season = "2014";
            //string league = "Liga A";
            //string season = loader.SeasonName;
            //string league = loader.LeagueName;


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
                        //file.FileName
                        loader.ProcessFile(memStr, file.FileName);
                    }
                }
            }

            PopulateEntityModel(loader, loader.SeasonName, loader.LeagueName);

            return View(new ImportModel(CurrentPage));
        }

        private void PopulateEntityModel(ExL.ExcelLoader loadedData, string seasonName, string leagueName)
        {
            BasketballDbContext db = new BasketballDbContext();

            bool ToUpdate = true;
            Season season = db.Seasons.FirstOrDefault(s => s.Name.Equals(seasonName)) ?? new Season();
           if(season.Name == null)
            {
                season.Name = seasonName;
                ToUpdate = false;
            }
            if(season.SeasonStartYear == 0)
            {
                season.SeasonStartYear = 2014;
                ToUpdate = false;
            }

            if (!ToUpdate) _seasonRepo.Add(season);
            else _seasonRepo.Update(season);

            League league = db.Leagues.FirstOrDefault(l => l.Name.Equals(leagueName)) ?? new League();

            if(league.Name == null)
            {
                league.Name = leagueName;
                db.Leagues.Add(league);
            }
            
            //League league = new League();
            //league.Name = leagueName;

            LeagueSeason leagueSeason = new LeagueSeason();
            leagueSeason.League = league;


            leagueSeason.Teams = PopulateTeamEntityModel(loadedData);


            db.LeagueSeasons.Add(leagueSeason);

            //var player = db.Players.FirstOrDefault(x => x.Name.Equals("something"));
            //player.LastName = "new lastname";
            //db.SaveChanges()


            //db.Seasons.Add(season);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private List<Team> PopulateTeamEntityModel(ExL.ExcelLoader loadedData)
        {
            List<Team> teamsList = new List<Team>();

            foreach (KeyValuePair<string, HashSet<string>> keyValuePair in loadedData.TeamAndPlayers)
            {
                Team team = new Team();
                //fetch team by team name or create and save
                team.TeamName = keyValuePair.Key;

                team.Players = GeneratePlayers(loadedData, keyValuePair.Value);

                teamsList.Add(team);
            }

            return teamsList;

        }

        private List<Player> GeneratePlayers(ExL.ExcelLoader loadedData, HashSet<string> playersIds)
        {
            List<Player> playerList = new List<Player>(playersIds.Count);
            foreach (string playerId in playersIds)
            {
                List<ExL.PlayerScore> scores = loadedData.GetPlayerScoreList(playerId);
                if (scores.Count > 0)
                {
                    Player p = PopulatePlayerEntityModel(scores);
                    playerList.Add(p);
                }
            }
            return playerList;
        }


        private Player PopulatePlayerEntityModel(List<ExL.PlayerScore> playerScores)
        {
            Player player = new Player();
            ExL.PlayerScore pScore = playerScores.First();

            player.Name = pScore.FirstName;
            player.LastName = pScore.LastName;
            player.MiddleName = pScore.MiddleName;

            foreach (ExL.PlayerScore playerScore in playerScores)
            {
                //TO DO (set Stats)
                Stats stats = new Stats();

            }

            return player;
        }

        //private Game PopulateGame()
        //{
        //    Game game = new Game() {

        //    };

        //}

        private Stats PopulateStats(ExL.PlayerScore ps)
        {
            Stats stats = new Stats()
            {
                Ast = ps.Assistance,
                Blk = ps.Block,
                DReb = ps.DefensiveReb,
                FtMade = ps.FreeThrowsMade,
                FtMissed = ps.FreeThrowsAttempt
                //Gam
            };


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