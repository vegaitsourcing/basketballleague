using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using System.Globalization;
using System.Web;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
    public class AboutUsModel : PageModel
    {
        public AboutUsModel()
        {
            Banner = new BannerModel(Content);
        }

        public AboutUsModel(IPublishedContent content) : base(content)
		{
            Banner = new BannerModel(Content);
        }

        public AboutUsModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
            Banner = new BannerModel(Content);
        }

        public BannerModel Banner { get; }
        public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
        public string EmailAddress => this.GetPropertyValue<string>();
    }
}
