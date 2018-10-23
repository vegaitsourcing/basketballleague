using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using LZRNS.Models.MediaTypes;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes
{
	public class HeaderModel : CachedContentModel
	{
		public HeaderModel(IPublishedContent content) : base(content)
		{
		}

		public ImageModel Logo => this.GetCachedValue(() => Content.GetPropertyValue<IPublishedContent>()?.AsMediaType<ImageModel>());
		public IEnumerable<PageModel> NavigationItems { get; set; }
	}

}
