using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.ExcelLoader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class DataParser
    {

        private BasketballDbContext _db;
        private ExcelAnalyzer _analyzer;

        #region[properties]

        private ExcelAnalyzer Analyzer { get => _analyzer; set => _analyzer = value; }
        private BasketballDbContext Db { get { return _db; } }
        #endregion

        #region[constructors]

        public DataParser(BasketballDbContext db, AbstractExcelLoader extractor)
        {
            _db = db;
            _analyzer = (ExcelAnalyzer)extractor;

        }
        #endregion
        #region [database fetch methods]
        public ICollection<Player> GetPlayersByName(string completeName)
        {
            //bug
            ICollection<Player> players = Db.Players.Where(pl => (pl.Name + " " + pl.LastName) == completeName).ToList();
            return players;
        }



        public void GetPlayerInfo(Player player, out string leagueSeasonName, out string teamName)
        {
            /*get ppt record from latest leagueseason*/
            PlayerPerTeam playerPerTeam = null;
            ICollection<PlayerPerTeam> playersPerTeam = Db.PlayersPerTeam.Where(ppt => ppt.PlayerId == player.Id).ToList();
            LeagueSeason latestLeagueSeason = GetLatestLeagueSeason(playersPerTeam.Select(ppt => ppt.LeagueSeason).ToList());
            if (latestLeagueSeason != null)
            {
                playerPerTeam = playersPerTeam.Where(ppt => ppt.LeagueSeason == latestLeagueSeason).FirstOrDefault();
            }
            leagueSeasonName = "-";
            teamName = "-";

            //PlayerInfo playerInfo = 
            if (playerPerTeam != null)
            {
                /*NOTE: get latest league season, not first*/
                //LeagueSeason leagueSeason = Db.LeagueSeasons.Where(ls => ls.Id == playerPerTeam.LeagueSeason_Id).FirstOrDefault();


                leagueSeasonName = latestLeagueSeason.FullName;
                //team cannot be null, safe statement
                teamName = playerPerTeam.Team.TeamName;


            }
        }


        public LeagueSeason GetLatestLeagueSeason(ICollection<LeagueSeason> lSeasons)
        {
            LeagueSeason leagueSeason = null;
            Dictionary<Guid, int> seasonsYears = lSeasons.ToDictionary(ls => ls.Id, ls => ls.Season.SeasonStartYear);
            if (seasonsYears.Count > 0)
            {
                Guid maxSeasonId = seasonsYears.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                leagueSeason = lSeasons.Where(ls => ls.Id == maxSeasonId).FirstOrDefault();
            }


            return leagueSeason;
        }
        #endregion

        public PlayerInfo CreatePlayerInfo(Player p, bool onLoan)
        {

            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.NameAndLastName = p.GetFullNameWithMiddleName;
            string leagueSeasonName;
            string teamName;
            GetPlayerInfo(p, out leagueSeasonName, out teamName);
            playerInfo.PreviousLeagueSeasonName = leagueSeasonName;
            playerInfo.PreviousTeamName = teamName;
            playerInfo.OnLoan = onLoan;
            playerInfo.UId = (p.UId != null) ? p.UId.ToString() : Guid.NewGuid().ToString();
            //create Player Info constructor with parameters
            return playerInfo;



        }

        public PlayerInfo CreateNewPlayerInfo(string fullName)
        {

            PlayerInfo playerInfo = new PlayerInfo();
            playerInfo.NameAndLastName = fullName;
            playerInfo.PreviousLeagueSeasonName = "-";
            playerInfo.PreviousTeamName = "-";
            playerInfo.OnLoan = false;
            playerInfo.UId = Guid.NewGuid().ToString();
            //create Player Info constructor with parameters
            return playerInfo;



        }
        
        public List<PlayerInfo> GetPlayerInfosForTeam(Dictionary<string, List<PlayerInfo>> playerInfoList)
        {
            List<PlayerInfo> playersInfoList = new List<PlayerInfo>();

            PlayerInfo plInfo = null;
            foreach (KeyValuePair<string, List<PlayerInfo>>kvp in playerInfoList)
            {
                string playerName = kvp.Key;
                bool onLoan = false;
                PlayerInfo info = kvp.Value.Where(val => val.OnLoan == true).FirstOrDefault();
                if (info != null)
                {
                    onLoan = true;
                }

                List<Player> players = GetPlayersByName(playerName).ToList();
                //non-existing player
                if (players.Count == 0)
                {
                    //skip situation, we assume person who wrote down data to excel mada a mistake
                    //onLoan = (onLoan) ? !onLoan : onLoan;
                   playersInfoList.Add(CreateNewPlayerInfo(playerName));

                    
                }
                //existing players with the same name
                else
                {
                    foreach (Player player in players)
                    {
                        playersInfoList.Add(CreatePlayerInfo(player, onLoan));

                    }
                }

            }
            return playersInfoList;


        }
        public List<PlayerInfo> GetPlayersInfoList()
        {
            List<PlayerInfo> playersInfoList = new List<PlayerInfo>();
            
            //foreach (KeyValuePair<string, List<PlayerInfo>> kvp in Analyzer.TeamPlayerInfos)
            foreach(KeyValuePair<string, Dictionary<string, List<PlayerInfo>>> kvp in Analyzer.TeamPlayerInfos)
            {
                string teamName = kvp.Key;
                playersInfoList.AddRange(GetPlayerInfosForTeam(kvp.Value));

            }

            return playersInfoList;

        }

    }
}
