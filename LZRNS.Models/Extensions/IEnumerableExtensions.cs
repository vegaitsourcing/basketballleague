using LZRNS.DomainModels.Extensions;
using LZRNS.DomainModels.Models;
using LZRNS.Models.AdditionalModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.Models.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<LeagueTopStatsModel> GetTopStatsPerLeague(this IEnumerable<LeagueSeason> source,
			TopStatisticCategories category, int topStatsCount)
		{
			if (source == null || !Enum.IsDefined(typeof(TopStatisticCategories), category) || topStatsCount <= 0)
			{
				yield break;
			}

			foreach (var league in source)
			{
				var stats = league.Rounds?.Where(r => r.Games != null && r.Games.Any())
					.SelectMany(r => r.Games)
					.Where(g => g.TeamAPlayerStats != null && g.TeamBPlayerStats != null)
					.Select(g => g.TeamAPlayerStats.Concat(g.TeamBPlayerStats))
					.SelectMany(s => s)
					.GroupBy(s => s.PlayerId)
					.ToArray();

				if (stats == null || !stats.Any()) continue;

				TopStatsModel[] topStats = GetTopStats(stats, category, topStatsCount);

				yield return new LeagueTopStatsModel(topStats);
			}
		}

		private static TopStatsModel[] GetTopStats(IGrouping<Guid, Stats>[] stats, TopStatisticCategories category, int topStatsCount)
		{
			switch (category)
			{
				case TopStatisticCategories.Total:
					return stats.GetTopStats<PlayerTotalStats>(sg => sg.Sum, topStatsCount);
				case TopStatisticCategories.Average:
					return stats.GetTopStats<PlayerAverageStats>(sg => sg.Average, topStatsCount);
				case TopStatisticCategories.PerMinute:
					return stats.GetTopStats<PlayerPerMinuteStats>(sg => sg.GetPerMinuteValue, topStatsCount);
				default:
					return new TopStatsModel[] { };
			}
		}
	}
}
