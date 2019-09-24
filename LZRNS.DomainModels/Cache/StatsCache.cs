using LZRNS.DomainModel.Context;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Cache
{
    public class StatsCache
    {
        public HashSet<Stats> PlayerStatsCache { get; set; } = new HashSet<Stats>();

        private readonly BasketballDbContext _db;

        public StatsCache(BasketballDbContext context)
        {
            _db = context;
        }

        public void LoadPlayerStatsCache(Guid[] playerIds)
        {
            var playerStats = _db.Stats.Where(stat => playerIds.Contains(stat.PlayerId)).ToList();
            PlayerStatsCache = new HashSet<Stats>(playerStats);
        }
    }
}