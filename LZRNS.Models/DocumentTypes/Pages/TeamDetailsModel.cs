using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Models;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.Extensions;
using Umbraco.Core.Models;

namespace LZRNS.Models.DocumentTypes.Pages
{
	public class TeamDetailsModel : PageModel
	{
		public TeamDetailsModel()
		{
		}

		public TeamDetailsModel(IPublishedContent content) : base(content)
		{
		}

		public TeamDetailsModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		public BannerModel Banner => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.EmptyIfNull()
				.FirstOrDefault()
				.AsType<BannerModel>());

		public Team Team { get; set; }
	}
}
