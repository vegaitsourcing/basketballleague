using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
    public class NestedContentBaseModel : CachedContentModel
    {
        public NestedContentBaseModel(IPublishedContent content) : base(content)
        {
        }
    }
}
