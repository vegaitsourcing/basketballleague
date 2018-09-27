using Umbraco.Core.Models;
using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
    public class SponsorshipModel : NestedContentBaseModel
    {
        public SponsorshipModel(IPublishedContent content) : base(content)
        {
        }
        public ImageModel Logo => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>().AsMediaType<ImageModel>());
    }
}