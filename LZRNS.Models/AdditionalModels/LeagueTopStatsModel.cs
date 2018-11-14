using LZRNS.Common;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;

namespace LZRNS.Models.AdditionalModels
{
	public enum TopStatisticCategories
	{
		Total,
		Average,
		PerMinute
	}

	public static class TopStatisticCategoriesExtensions
	{
		public static string ToQueryParameter(this TopStatisticCategories category)
		{
			return $"{Constants.RequestParameters.StatisticsCategory}={category}";
		}
	}

	public class LeagueTopStatsModel
	{
		public LeagueTopStatsModel(IReadOnlyList<TopStatsModel> topStats)
		{
			if (topStats == null) throw new ArgumentNullException(nameof(topStats));
			TopStats = topStats;
		}

		public IReadOnlyList<TopStatsModel> TopStats { get; }
		
	}
}
