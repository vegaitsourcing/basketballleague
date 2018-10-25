using System.Globalization;
using Umbraco.Core.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using System.Linq;
using LZRNS.Models.Extensions;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections;
using LZRNS.Common.Extensions;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class HomeModel : PageModel
	{
		public HomeModel()
		{
		}

		public HomeModel(IPublishedContent content) : base(content)
		{
		}

		public HomeModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public BannerModel Banner => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.FirstOrDefault()
				.AsType<BannerModel>());

		public IEnumerable<SectionBaseModel> Sections => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>().EmptyIfNull()
				.AsDocumentTypeModel<SectionBaseModel>());

		public IEnumerable<string> Leagues => this.GetPropertyValue<IEnumerable<string>>();
		public string CurrentShownLeague { get; set; }
	}
}
