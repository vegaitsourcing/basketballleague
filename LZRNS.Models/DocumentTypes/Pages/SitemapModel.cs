using System.Globalization;
using System.Linq;
using Umbraco.Core.Models;
using LZRNS.Common;
using LZRNS.Models.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class SitemapModel : PageModel
	{
		public SitemapModel()
		{
		}

		public SitemapModel(IPublishedContent content) : base(content)
		{
		}

		public SitemapModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public int DepthLevel => this.GetCachedValue(() => (Content.HasValue() ? Content.GetPropertyValue<int>() : AppSettings.SitemapDepthLevelDefaultValue) + Constants.HomePageLevel);

		public bool ShouldIncludeChildrenInSitemap(PageModel page)
		{
			return (page.Content.Level < DepthLevel) && page.SitemapItems.Any();
		}
	}
}
