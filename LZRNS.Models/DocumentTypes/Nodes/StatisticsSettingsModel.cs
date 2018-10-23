using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes
{
	public class StatisticsSettingsModel : CachedContentModel
	{
		public StatisticsSettingsModel(IPublishedContent content) : base(content)
		{
		}

		public int SeasonYearStart => this.GetPropertyValue<int>();
		public string ResultsRound => this.GetPropertyValue<string>();
		public string ScheduleRound => this.GetPropertyValue<string>();
	}
}
