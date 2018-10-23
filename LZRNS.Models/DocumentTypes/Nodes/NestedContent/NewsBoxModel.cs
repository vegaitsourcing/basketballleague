using System.Web;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
    public class NewsBoxModel : NestedContentBaseModel
    {
        public NewsBoxModel(IPublishedContent content) : base(content)
        {
        }

	    public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
    }
}
