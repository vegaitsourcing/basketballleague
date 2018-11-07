using System.Globalization;
using Umbraco.Core.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using System.Linq;
using LZRNS.Models.Extensions;
using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Nodes.NestedContent;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class HistoryModel : PageModel
	{
		public HistoryModel()
		{
		}

		public HistoryModel(IPublishedContent content) : base(content)
		{
		}

		public HistoryModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}
		
		public IEnumerable<HistorySeasonModel> Seasons => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.AsType<HistorySeasonModel>());
	}
}
