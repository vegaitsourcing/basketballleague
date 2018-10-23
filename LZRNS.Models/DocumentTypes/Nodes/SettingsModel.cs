using Umbraco.Core.Models;
using LZRNS.Models.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LZRNS.Models.DocumentTypes.Nodes
{
	public class SettingsModel : CachedContentModel
	{
		public SettingsModel(IPublishedContent content) : base(content)
		{
		}

		public string SiteName => this.GetPropertyValue<string>();
		public string Robots => this.GetPropertyValue<string>();
		public string CanonicalDomain => this.GetPropertyValue<string>();

		public IHtmlString GoogleTagManagerScriptCode => this.GetCachedValue(() => new MvcHtmlString(Content.GetPropertyValue<string>()));
		public IHtmlString GoogleTagManagerNonScriptCode => this.GetCachedValue(() => new MvcHtmlString(Content.GetPropertyValue<string>()));
		public IHtmlString GoogleAnalyticsScriptCode => this.GetCachedValue(() => new MvcHtmlString(Content.GetPropertyValue<string>()));
		public IHtmlString CookiesText => this.GetCachedValue(() => new MvcHtmlString(Content.GetPropertyValue<string>()));
		public string GoogleMapKey => this.GetPropertyValue<string>();

		public ManagementPageModel ManagementPage => this.GetCachedValue(() => Content
		.GetPropertyValue<IEnumerable<IPublishedContent>>()
			.FirstOrDefault()
			.AsType<ManagementPageModel>());
	}
}
