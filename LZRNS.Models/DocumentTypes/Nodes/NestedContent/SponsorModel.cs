using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;
using RJP.MultiUrlPicker.Models;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
	public class SponsorModel : NestedContentBaseModel
	{
		public SponsorModel(IPublishedContent content) : base(content)
		{
		}

		public ImageModel Image => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>().AsMediaType<ImageModel>());
		public Link Link => this.GetPropertyValue<Link>();
	}
}
