using System.Globalization;
using Umbraco.Core.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using LZRNS.Models.Extensions;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent;
using System.Linq;

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

        public IEnumerable<NewsSliderModel> NewsSlider => this.GetCachedValue(() => Content
       .GetPropertyValue<IEnumerable<IPublishedContent>>()
       .AsType<NewsSliderModel>().ToList());

        public IEnumerable<SponsorshipModel> Sponsorship => this.GetCachedValue(() => Content
       .GetPropertyValue<IEnumerable<IPublishedContent>>()
       .AsType<SponsorshipModel>().ToList());

        public IEnumerable<TableWidgetModel> TableWidget => this.GetCachedValue(() => Content
       .GetPropertyValue<IEnumerable<IPublishedContent>>()
       .AsType<TableWidgetModel>().ToList());
    }
}
