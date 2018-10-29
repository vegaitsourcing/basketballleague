using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Nodes;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class TeamsModel : PageModel
	{
		public TeamsModel()
		{
		}

		public TeamsModel(IPublishedContent content) : base(content)
		{
		}

		public TeamsModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public BannerModel Banner => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.FirstOrDefault()
				.AsType<BannerModel>());

		public IEnumerable<string> Leagues => this.GetPropertyValue<IEnumerable<string>>();
		public string CurrentShownLeague { get; set; }

		public int SeasonYearStart => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.SeasonYearStart) ?? DateTime.Now.Year;
	}
}
