using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.ExcelLoader.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.ExcelLoader.ExcelReader
{
    public class DataParser
    {
        public DataParser(BasketballDbContext db, ExcelAnalyzer extractor)
        {
            Db = db;
            Analyzer = extractor;
        }

        private ExcelAnalyzer Analyzer { get; }
        private BasketballDbContext Db { get; }

        private string LeagueSeasonName => Analyzer.SeasonName + "-" + Analyzer.LeagueName;

        public PlayerInfo CreateNewPlayerInfo(string fullName, string newTeamName)
        {
            return new PlayerInfo
            {
                NameAndLastName = fullName,
                PreviousLeagueSeasonName = "-",
                PreviousTeamName = "-",
                OnLoan = false,
                UId = Guid.NewGuid().ToString(),
                NewTeamName = newTeamName,
                NewLeagueSeasonName = LeagueSeasonName
            };
        }

        public PlayerInfo CreatePlayerInfo(Player p, bool onLoan, string newTeamName)
        {
            GetPlayerInfo(p, out string leagueSeasonName, out string teamName);

            return new PlayerInfo
            {
                NameAndLastName = p.GetFullNameWithMiddleName,
                PreviousLeagueSeasonName = leagueSeasonName,
                PreviousTeamName = teamName,
                OnLoan = onLoan,
                UId = p.UId ?? Guid.NewGuid().ToString(),
                NewTeamName = newTeamName,
                NewLeagueSeasonName = LeagueSeasonName
            };
        }

        public LeagueSeason GetLatestLeagueSeason(ICollection<LeagueSeason> lSeasons)
        {
            LeagueSeason leagueSeason = null;
            var seasonsYears = lSeasons.ToDictionary(ls => ls.Id, ls => ls.Season.SeasonStartYear);
            if (seasonsYears.Count > 0)
            {
                var maxSeasonId = seasonsYears.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                leagueSeason = lSeasons.FirstOrDefault(ls => ls.Id == maxSeasonId);
            }

            return leagueSeason;
        }

        public void GetPlayerInfo(Player player, out string leagueSeasonName, out string teamName)
        {
            var playersPerTeam = GetPlayersPerTeamByPlayerId(player.Id);
            var latestLeagueSeason = GetLatestLeagueSeason(playersPerTeam.Select(ppt => ppt.LeagueSeason).ToList());
            var playerPerTeam = GetPlayerPerTeamByLeagueSeason(playersPerTeam, latestLeagueSeason);

            if (playerPerTeam == null)
            {
                leagueSeasonName = "-";
                teamName = "-";
            }
            else
            {
                leagueSeasonName = latestLeagueSeason.FullName;
                teamName = playerPerTeam.Team.TeamName;
            }
        }

        public List<PlayerInfo> GetPlayerInfosForTeam(Dictionary<string, List<PlayerInfo>> PlayerInfoListByPlayerName, string teamName)
        {
            var playersInfoList = new List<PlayerInfo>();

            foreach (var keyValuePair in PlayerInfoListByPlayerName)
            {
                string playerName = keyValuePair.Key;
                var playerInfo = keyValuePair.Value?.FirstOrDefault(val => val.OnLoan);
                bool onLoan = playerInfo != null;

                var players = GetPlayersByName(playerName).ToList();

                if (players.Count == 0) // non existing player
                {
                    playersInfoList.Add(CreateNewPlayerInfo(playerName, teamName));
                }
                else //existing players with the same name
                {
                    playersInfoList.AddRange(players.Select(player => CreatePlayerInfo(player, onLoan, teamName)));
                }
            }
            return playersInfoList;
        }

        public ICollection<Player> GetPlayersByName(string completeName)
        {
            return Db.Players.Where(pl => (pl.Name + " " + pl.LastName) == completeName).ToList();
        }

        public List<PlayerInfo> GetPlayersInfoList()
        {
            var playersInfoList = new List<PlayerInfo>();

            foreach (var keyValuePair in Analyzer.PlayerInfoListByPlayerNameAndLastNameByTeamName)
            {
                string teamName = keyValuePair.Key;
                playersInfoList.AddRange(GetPlayerInfosForTeam(keyValuePair.Value, teamName));
            }

            return playersInfoList;
        }

        private static PlayerPerTeam GetPlayerPerTeamByLeagueSeason(ICollection<PlayerPerTeam> playersPerTeam, LeagueSeason latestLeagueSeason)
        {
            PlayerPerTeam playerPerTeam = null;
            if (latestLeagueSeason != null)
            {
                playerPerTeam = playersPerTeam.FirstOrDefault(ppt => ppt.LeagueSeason == latestLeagueSeason);
            }

            return playerPerTeam;
        }

        private ICollection<PlayerPerTeam> GetPlayersPerTeamByPlayerId(Guid playerId)
        {
            return Db.PlayersPerTeam.Where(ppt => ppt.PlayerId == playerId).ToList();
        }
    }
}