using System;
using System.Linq;

namespace LZRNS.DomainModels.Models
{
	public class PlayerPerMinuteStats : PlayerStats
	{
		public PlayerPerMinuteStats(string playerName, Stats[] stats) 
			: base(playerName, stats, v => Math.Round((double) v / (stats.Length * stats.Sum(s => s.MinutesPlayed)), 1))
		{
		}
	}
}
