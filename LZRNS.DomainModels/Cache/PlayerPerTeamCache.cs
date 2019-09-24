using LZRNS.DomainModel.Context;
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
    }
}