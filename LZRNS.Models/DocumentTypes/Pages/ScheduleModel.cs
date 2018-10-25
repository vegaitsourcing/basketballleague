using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Nodes;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class ScheduleModel : PageModel
	{
		public ScheduleModel()
		{
		}

		public ScheduleModel(IPublishedContent content) : base(content)
		{
		}

		public ScheduleModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public BannerModel Banner => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.FirstOrDefault()
				.AsType<BannerModel>());

		public IEnumerable<ScheduleItemModel> Schedule => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.AsType<ScheduleItemModel>());

		public int? SeasonYearStart => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.SeasonYearStart);
		public IEnumerable<string> Leagues => this.GetPropertyValue<IEnumerable<string>>();
		public string CurrentShownLeague { get; set; }
	}
}
