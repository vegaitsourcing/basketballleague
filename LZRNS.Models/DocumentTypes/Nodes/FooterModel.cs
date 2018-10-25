using System.Collections.Generic;
using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent;
using LZRNS.Models.Extensions;
using RJP.MultiUrlPicker.Models;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes
{
	public class FooterModel : CachedContentModel
	{
		public FooterModel(IPublishedContent content) : base(content)
		{
		}

		public IEnumerable<Link> Links => this.GetPropertyValue<IEnumerable<Link>>();
		public IEnumerable<SocialModel> SocialLinks=> this.GetCachedValue(() => Content.GetPropertyValue<IEnumerable<IPublishedContent>>().EmptyIfNull().AsType<SocialModel>());
		public string CopyrightText => this.GetPropertyValue<string>();
	}
}
