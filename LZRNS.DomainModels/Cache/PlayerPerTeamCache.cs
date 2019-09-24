using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class PlayerPerTeamCache
    {
        public HashSet<PlayerPerTeam> PlayersPerTeamCache { get; set; } = new HashSet<PlayerPerTeam>();

        private readonly BasketballDbContext _db;

        public PlayerPerTeamCache(BasketballDbContext context)
        {
            _db = context;
        }

        public void LoadPlayersPerTeamCache(Guid leagueSeasonId)
        {
            var playerPerTeams = _db.PlayersPerTeam.Where(ppt => ppt.LeagueSeason_Id == leagueSeasonId).ToList();
            PlayersPerTeamCache = new HashSet<PlayerPerTeam>(playerPerTeams);
        }

        public PlayerPerTeam CreateOrGetPlayerPerTeam(Player player, Team team, LeagueSeason leagueSeason)
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
    }
}