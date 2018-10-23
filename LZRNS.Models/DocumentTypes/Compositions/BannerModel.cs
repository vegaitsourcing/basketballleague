using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;
using System.Web;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Compositions
{
    public class BannerModel : CachedContentModel
    {
        public BannerModel(IPublishedContent content) : base(content)
		{
        }

        public ImageModel Image => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>().AsMediaType<ImageModel>());
        public IHtmlString BannerTitle => this.GetPropertyValue<IHtmlString>();
    }
}
