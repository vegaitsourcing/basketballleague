using Umbraco.Core.Models;
using Umbraco.Web;
using LZRNS.Models.Extensions;

namespace LZRNS.Models.MediaTypes
{
	public class ImageModel : CachedMediaModel
	{
		public ImageModel(IPublishedContent content) : base(content)
		{
		}

		public string Url => Content.Url;
		public string AlternateText => this.GetPropertyWithDefaultValue(Name);

		public string GetCropUrl(string cropAlias)
		{
			return Content.GetCropUrl(cropAlias);
		}
	}
}
