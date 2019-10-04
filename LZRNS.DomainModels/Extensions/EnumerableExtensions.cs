using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Extensions
{
    public static class EnumerableExtensions
    {
        public static List<T> GetTopStats<T>(this IEnumerable<IGrouping<Guid, Stats>> source, Func<IGrouping<Guid, Stats>, double> orderCriteria) where T : PlayerStats
        {
            if (orderCriteria == null) throw new ArgumentNullException(nameof(orderCriteria));

            if (source == null) return new List<T>();

            return source.OrderByDescending(orderCriteria).Select(g => (T)Activator.CreateInstance(typeof(T), g.First().Player.GetFullName, g.ToArray())).GroupBy(s => s.PlayerName).Select(g => g.First()).ToList();
        }

        public static TopStatsModel[] GetTopStats<T>(this IEnumerable<IGrouping<Guid, Stats>> source,
            Func<IGrouping<Guid, Stats>, Func<Func<Stats, double>, double>> criteria, int maxItems) where T : PlayerStats
        {
            if (source == null) return new TopStatsModel[] { };

            var sourceList = (source as List<IGrouping<Guid, Stats>>) ?? source.ToList();

            var points = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.Pts));
            var assists = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.Ast));
            var steals = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.Stl));
            var rebounds = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.Reb));
            var blocks = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.Blk));
            var mins = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.MinutesPlayed));
            var eff = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.Eff));
            var fg = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.FgM));
            var twoPts = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.TwoPtMade));
            var threePts = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.ThreePtMade));
            var ft = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.FtMade));
            var offReb = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.OReb));
            var defReb = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.DReb));
            var to = sourceList.GetTopStats<T>(sg => criteria.Invoke(sg).Invoke(s => s.To));

            return new[]
            {
                new TopStatsModel(points, TableTypes.Points, ps => ps.Points, maxItems),
                new TopStatsModel(assists, TableTypes.Assists, ps => ps.Assists, maxItems),
                new TopStatsModel(steals, TableTypes.Steals, ps => ps.Steals, maxItems),
                new TopStatsModel(rebounds, TableTypes.Rebounds, ps => ps.Rebounds, maxItems),
                new TopStatsModel(blocks, TableTypes.Blocks, ps => ps.Blocks, maxItems),
                new TopStatsModel(mins, TableTypes.Minutes, ps => ps.MinutesPlayed, maxItems),
                new TopStatsModel(eff, TableTypes.Efficiency, ps => ps.Efficiency, maxItems),
                new TopStatsModel(fg, TableTypes.FieldGoals, ps => ps.FieldGoals, maxItems),
                new TopStatsModel(twoPts, TableTypes.TwoPoints, ps => ps.TwoPoints, maxItems),
                new TopStatsModel(threePts, TableTypes.ThreePoints, ps => ps.ThreePoints, maxItems),
                new TopStatsModel(ft, TableTypes.FreeThrows, ps => ps.FreeThrows, maxItems),
                new TopStatsModel(offReb, TableTypes.OffensiveRebounds, ps => ps.OffensiveRebounds, maxItems),
                new TopStatsModel(defReb, TableTypes.DefensiveRebounds, ps => ps.DefensiveRebounds, maxItems),
                new TopStatsModel(to, TableTypes.Turnovers, ps => ps.To, maxItems)
            };
        }

        public static double GetPerMinuteValue(this IEnumerable<Stats> source, Func<Stats, double> criteria)
        {
            if (source == null) return default(double);

            var sourceArray = source.ToArray();

            if (sourceArray.Length == 0) return default(double);

            int totalMinutes = sourceArray.Sum(s => s.MinutesPlayed);

            return Math.Round(sourceArray.Sum(criteria) / (sourceArray.Length * totalMinutes), 1);
        }
    }
}