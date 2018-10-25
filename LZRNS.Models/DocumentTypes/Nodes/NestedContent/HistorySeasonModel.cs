using System.Collections.Generic;
using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent.Modules;
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

		public IEnumerable<NestedContentBaseModel> Modules => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.AsDocumentTypeModel<ModuleBaseModel>());
	}
}
