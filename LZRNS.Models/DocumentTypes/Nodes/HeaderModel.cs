using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes
{
    public class HeaderModel : CachedContentModel
    {
        public HeaderModel(IPublishedContent content) : base(content)
        {
        }

        public ImageModel Logo => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>()?.AsMediaType<ImageModel>());
    }

}
