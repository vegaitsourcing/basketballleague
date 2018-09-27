using Umbraco.Core.Models;
using LZRNS.Models.Extensions;
using System.Web;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
    public class NewsSliderModel : NestedContentBaseModel
    {
        public NewsSliderModel(IPublishedContent content) : base(content)
        {
        }

        public IHtmlString Text => this.GetPropertyValue<IHtmlString>();
    }
}