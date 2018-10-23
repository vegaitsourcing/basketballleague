using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class ManagementModel : ManagementPageModel
	{
		public ICollection<Game> ResultsRoundGames { get; set; }
		public ICollection<Game> ScheduleRoundGames { get; set; }
	}
}
