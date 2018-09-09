using LZRNS.Models.DocumentTypes.Pages;
using System;
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

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class ImportController : RenderMvcController
    {
        public ActionResult Index(ImportModel model)
        {
            return CurrentTemplate(model);
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            var content = new ImportModel();
            if (files == null || files.Length == 0)
            {
                var currentPageId = UmbracoContext.PageId;
                

                return CurrentTemplate(content);
            }

            ExL.ExcelLoader loader = new ExL.ExcelLoader();
            string season = "2016";
            string league = "A";


            foreach (var file in files)
            {
                if(file != null)
                {
                    var memStr = GetFileAsMemoryStream(file);
                    
                    //file.FileName
                    loader.ProcessFile(memStr, file.FileName);
                   

                }
            }

            PopulateEntityModel(loader, season, league);

            return CurrentTemplate(content);
        }

        private void PopulateEntityModel (ExL.ExcelLoader loadedData, string seasonName, string leagueName)
        {
            Season season = new Season();
            season.Name = seasonName;

            

            League league = new League();
            league.Name = leagueName;

            LeagueSeason leagueSeason = new LeagueSeason();
            leagueSeason.League = league;


            leagueSeason.Teams = PopulateTeamEntityModel(loadedData);
            
            

            BasketballDbContext db = new BasketballDbContext();

            db.Seasons.Add(season);

            db.SaveChanges();

        }

        private List<Team> PopulateTeamEntityModel (ExL.ExcelLoader loadedData)
        {
            List<Team> teamsList = new List<Team>();

            foreach(KeyValuePair<string, HashSet<string>> keyValuePair in loadedData.TeamAndPlayers)
            {
                Team team = new Team();
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


        private Player PopulatePlayerEntityModel (List<ExL.PlayerScore> playerScores)
        {
            Player player = new Player();
            ExL.PlayerScore pScore = playerScores.First();

            player.Name = pScore.FirstName;
            player.LastName = pScore.LastName;
            player.MiddleName = pScore.MiddleName;

            foreach (ExL.PlayerScore playerScore in playerScores)
            {
                //TO DO (set Stats)
            }

            return player;
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