using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LZRNS.DomainModels.Models
{
	public enum TableTypes
	{
		Points,
		Assists,
		Steals,
		Rebounds,
		Blocks,
		Minutes,
		Efficiency,
		FieldGoals,
		TwoPoints,
		ThreePoints,
		FreeThrows,
		OffensiveRebounds,
		DefensiveRebounds,
		Turnovers
	}

	public class TopStatsModel
	{
		private readonly Func<PlayerStats, double> _criteria;
		private readonly int _topItemsCount;
		
		public TopStatsModel(IReadOnlyList<PlayerStats> stats, TableTypes tableType, Func<PlayerStats, double> criteria, int topItemsCount)
		{
			if (stats == null) throw new ArgumentNullException(nameof(stats));
			if (criteria == null) throw new ArgumentNullException(nameof(criteria));
			if (!Enum.IsDefined(typeof(TableTypes), tableType)) throw new InvalidEnumArgumentException(nameof(tableType), (int) tableType, typeof(TableTypes));
			if (topItemsCount <= 0) throw new ArgumentOutOfRangeException(nameof(topItemsCount));

			_criteria = criteria;
			_topItemsCount = topItemsCount;
			TableType = tableType;
			Stats = stats;
		}

		public TableTypes TableType { get; }
		public IReadOnlyList<PlayerStats> Stats { get; }

		public double GetValue(PlayerStats stats)
		{
			return _criteria.Invoke(stats);
		}
		
		public IReadOnlyList<PlayerStats> GetTopStats()
		{
			return Stats?.Take(_topItemsCount).ToList() ?? new List<PlayerStats>();
		}
	}
}
