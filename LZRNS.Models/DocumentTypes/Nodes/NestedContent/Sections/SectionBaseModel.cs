using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections
{
	public class SectionBaseModel : NestedContentBaseModel
	{
		public SectionBaseModel(IPublishedContent content) : base(content)
		{
		}

		public string Title => this.GetPropertyValue<string>();
		public string LeagueName { get; set; }
	}
}
