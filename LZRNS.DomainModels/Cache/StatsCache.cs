using LZRNS.DomainModel.Context;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.ExcelLoaderModels;
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

        public Stats CreateOrGetStatsForPlayer(PlayerScore playScore, Player player, Game game, bool onLoan)
        {
            var stats = PlayerStatsCache.FirstOrDefault(st => st.Player.Id == player.Id && st.Game.Id == game.Id);

            if (stats != null)
            {
                return stats;
            }

            stats = new Stats
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
    }
}