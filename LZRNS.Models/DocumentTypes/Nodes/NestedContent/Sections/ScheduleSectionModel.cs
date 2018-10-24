using System;
using System.Collections.Generic;
using System.Linq;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections
{
	public class ScheduleSectionModel : SectionBaseModel
	{
		public ScheduleSectionModel(IPublishedContent content) : base(content)
		{
			UmbracoHelper = new UmbracoHelper(UmbracoContext.Current);
		}

		protected UmbracoHelper UmbracoHelper { get; }

		public string RoundName => this.GetCachedValue(() => 
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.ScheduleRound);

		public int? SeasonYearStart => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.SeasonYearStart);

		public DateTime FirstDate => this.GetPropertyValue<DateTime>();
		public DateTime SecondDate => this.GetPropertyValue<DateTime>();

		public IEnumerable<DateTime> FirstDateRange => this.GetCachedValue(() => GetDateRange(FirstDate, true));
		public IEnumerable<DateTime> SecondDateRange => this.GetCachedValue(() => GetDateRange(SecondDate, false));

		private IEnumerable<DateTime> GetDateRange(DateTime date, bool dateLast)
		{
			return Enumerable.Range(0, 1 + (date.AddDays(4) - date).Days)
				.Select(offset => dateLast ? date.AddDays(offset - 4) : date.AddDays(offset));
		}

	}
}
