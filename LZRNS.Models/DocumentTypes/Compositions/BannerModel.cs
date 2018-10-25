using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Compositions
{
    public class BannerModel : CachedContentModel
    {
        public BannerModel(IPublishedContent content) : base(content)
		{
        }

	    public string PreBannerTitle => this.GetPropertyValue<string>();
	    public string BannerTitle => this.GetPropertyValue<string>();
	    public string PostBannerTitle => this.GetPropertyValue<string>();

	    public bool BannerTitleExists =>
		    PreBannerTitle.HasValue() && BannerTitle.HasValue() && PostBannerTitle.HasValue();
    }
}
