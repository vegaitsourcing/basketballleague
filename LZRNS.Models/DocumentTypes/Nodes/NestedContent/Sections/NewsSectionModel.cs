using System.Collections.Generic;
using LZRNS.Common.Extensions;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections
{
	public class NewsSectionModel : SectionBaseModel
	{
		public NewsSectionModel(IPublishedContent content) : base(content)
		{
		}

		public IEnumerable<NewsBoxModel> NewsBoxes => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>().EmptyIfNull().AsType<NewsBoxModel>());
	}
}
