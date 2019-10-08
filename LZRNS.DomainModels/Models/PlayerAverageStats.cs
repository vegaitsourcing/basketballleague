using System;

namespace LZRNS.DomainModels.Models
{
	public class PlayerAverageStats : PlayerStats
	{
		public PlayerAverageStats(string playerName, Stats[] stats) 
			: base(playerName, stats, v => (stats.Length == 0) ? 0 : Math.Round((double) v / stats.Length, 1))
		{
		}
	}
}
