using System.Collections.Generic;
using LZRNS.Common.Extensions;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections
{
	public class SponsorsSectionModel : SectionBaseModel
	{
		public SponsorsSectionModel(IPublishedContent content) : base(content)
		{
		}

		public IEnumerable<SponsorModel> Sponsors => this.GetCachedValue(() =>
			this.Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.AsType<SponsorModel>());
	}
}
