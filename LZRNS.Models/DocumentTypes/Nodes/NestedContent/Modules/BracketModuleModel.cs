using System.Web;
using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Modules
{
	public class BracketModuleModel : ModuleBaseModel
	{
		public BracketModuleModel(IPublishedContent content) : base(content)
		{
		}

		public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
		public ImageModel Bracket => this.GetCachedValue(() => 
			Content.GetPropertyValue<IPublishedContent>()
				.AsMediaType<ImageModel>());
	}
}
