using System.Web;
using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Modules
{
	public class ImageModuleModel : ModuleBaseModel
	{
		public ImageModuleModel(IPublishedContent content) : base(content)
		{
		}

		public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
		public ImageModel Image => this.GetCachedValue(() => 
			Content.GetPropertyValue<IPublishedContent>()
				.AsMediaType<ImageModel>());
	}
}
