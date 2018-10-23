using System.Globalization;
using Umbraco.Core.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
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

		public string PreBannerTitle => this.GetPropertyValue<string>();
		public string BannerTitle => this.GetPropertyValue<string>();
		public string PostBannerTitle => this.GetPropertyValue<string>();

		public bool BannerTitleExists =>
			PreBannerTitle.HasValue() && BannerTitle.HasValue() && PostBannerTitle.HasValue();

		public IEnumerable<SectionBaseModel> Sections => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>().EmptyIfNull()
				.AsDocumentTypeModel<SectionBaseModel>());
	}
}
