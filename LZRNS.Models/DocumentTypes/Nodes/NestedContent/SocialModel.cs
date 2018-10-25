using LZRNS.Models.Extensions;
using RJP.MultiUrlPicker.Models;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent
{
	public class SocialModel : NestedContentBaseModel
	{
		public SocialModel(IPublishedContent content) : base(content)
		{
		}

		public string Icon => this.GetPropertyValue<string>();
		public Link Link => this.GetPropertyValue<Link>();
	}
}
