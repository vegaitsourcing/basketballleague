using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
    public class HistorySeasonModel : NestedContentBaseModel
    {
        public HistorySeasonModel(IPublishedContent content) : base(content)
        {
        }

	    public string Title => this.GetPropertyValue<string>();
    }
}
