using LZRNS.DomainModel.Models;
using LZRNS.Models.AdditionalModels;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class StatisticsModel : PageModel
	{
		public int SelectedSeasonYear { get; set; }
		public TopStatisticCategories Category { get; set; }
		public IReadOnlyList<Season> Seasons { get; set; }
	}
}
