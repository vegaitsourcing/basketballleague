using LZRNS.Models.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace LZRNS.Models.DocumentTypes.Nodes.NestedContent.Sections
{
	public class TopStatsSectionModel : SectionBaseModel
	{
		public TopStatsSectionModel(IPublishedContent content) : base(content)
		{
			UmbracoHelper = new UmbracoHelper(UmbracoContext.Current);
		}

		protected UmbracoHelper UmbracoHelper { get; }

		public string RoundName => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.ResultsRound);

		public int? SeasonYearStart => this.GetCachedValue(() =>
			UmbracoHelper.GetSingleContentOfType<StatisticsSettingsModel>()?.SeasonYearStart);
	}
}
