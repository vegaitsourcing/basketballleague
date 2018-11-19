using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Pages;
using System.Collections.Generic;

namespace LZRNS.Models.ViewModel
{
	public class PlayerDetailsViewModel : PageModel
	{
		public PlayerDetailsModel PlayerDetailsModel { get; set; }
		public Player CurrentPlayer { get; set; }
		public IReadOnlyList<PlayerTotalStats> TotalStats { get; set; }
		public IReadOnlyList<PlayerAverageStats> AverageStats { get; set; }
		public IReadOnlyList<PlayerPerMinuteStats> PerMinuteStats { get; set; }
	}
}
