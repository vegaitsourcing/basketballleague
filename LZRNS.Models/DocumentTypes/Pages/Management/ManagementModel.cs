using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class ManagementModel : ManagementPageModel
	{
		public ManagementModel(IPublishedContent content) : base(content) { }

		public ICollection<Game> ResultsRoundGames { get; set; }
		public ICollection<Game> ScheduleRoundGames { get; set; }
	}
}
