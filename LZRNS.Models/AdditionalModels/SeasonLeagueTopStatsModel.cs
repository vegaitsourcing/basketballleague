using System;
using System.Collections.Generic;

namespace LZRNS.Models.AdditionalModels
{
	public class SeasonLeagueTopStatsModel
	{
		public SeasonLeagueTopStatsModel(IReadOnlyList<LeagueTopStatsModel> leagues)
		{
			if (leagues == null) throw new ArgumentNullException(nameof(leagues));

			Leagues = leagues;
		}

		public IReadOnlyList<LeagueTopStatsModel> Leagues { get; }
	}
}
