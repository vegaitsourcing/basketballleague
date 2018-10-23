using System;
using System.Collections.Generic;
using System.Globalization;
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

		public DateTime FirstDate => this.GetPropertyValue<DateTime>();
		public DateTime SecondDate => this.GetPropertyValue<DateTime>();

		public IEnumerable<DateTime> FirstDateRange => this.GetCachedValue(() => GetDateRange(FirstDate, true));
		public IEnumerable<DateTime> SecondDateRange => this.GetCachedValue(() => GetDateRange(SecondDate, false));

		private IEnumerable<DateTime> GetDateRange(DateTime date, bool dateLast)
		{
			var start = dateLast ? date.AddDays(-4) : date.AddDays(4);
			//: TODO MAKE THIS THING WORK PLEAZ I WANT ONE LINER HERE!
			return Enumerable.Range(0, 1 + (date - start).Days)
				.Select(offset =>start.AddDays(offset));
		}

	}
}
