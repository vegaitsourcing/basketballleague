using System.Web;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Modules
{
    public class TextModuleModel : ModuleBaseModel
	{
        public TextModuleModel(IPublishedContent content) : base(content)
        {
        }

	    public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
    }
}
