namespace LZRNS.DomainModels.Models
{
	public class PlayerTotalStats : PlayerStats
	{
		public PlayerTotalStats(string playerName, Stats[] stats) : base(playerName, stats, v => v)
		{
		}
	}
}
