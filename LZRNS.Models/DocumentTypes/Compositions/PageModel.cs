using LZRNS.Common;
using LZRNS.Models.DocumentTypes.Nodes;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Models.Extensions;
using LZRNS.Models.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace LZRNS.Models.DocumentTypes.Compositions
{
	public class PageModel : RenderModel, IUmbracoCachedModel
	{
		public PageModel() : this(new UmbracoHelper(UmbracoContext.Current).TypedContent(UmbracoContext.Current.PageId))
		{
		}

		public PageModel(IPublishedContent content) : base(content)
		{
		}

		public PageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		#region [Content]

		public string Title => this.GetPropertyWithDefaultValue(Name);

		public BannerModel Banner => this.GetCachedValue(() =>
			Content.GetPropertyValue<IEnumerable<IPublishedContent>>()
				.AsSingle<BannerModel>());

		#endregion

		#region [SEO]

		public string SeoTitle => this.GetPropertyWithDefaultValue(Title);
		public string SeoDescription => this.GetPropertyValue<string>();
		public string SeoKeywords => this.GetPropertyValue<string>();
		public string SeoAuthor => this.GetPropertyValue<string>();

		#endregion

		#region [Page Settings]

		public bool HideFromSiteNavigation => UmbracoNaviHide;
		public bool HideFromSiteSearch => this.GetPropertyValue<bool>();
		public bool HideFromGoogleSearch => this.GetPropertyValue<bool>();
		public bool HideFromSitemap => this.GetPropertyValue<bool>();
		public string ExternalRedirect => this.GetPropertyValue<string>();

		#endregion

		#region [Additional]

		public IEnumerable<PageModel> NavigationItems => this.GetCachedValue(() => Content.GetNavigationItems());
		public IEnumerable<PageModel> SideBarNavigationItems => this.GetCachedValue(() => Content.GetSideBarNavigationItems(Constants.HomePageLevel + 1));
		public IEnumerable<PageModel> SitemapItems => this.GetCachedValue(() => Content.GetSitemapItems());
		public IEnumerable<PageModel> SitemapXMLItems => this.GetCachedValue(() => Content.GetSitemapXMLItems());
		public HomeModel Home => this.GetCachedValue(GetHomeModel);
		public SettingsModel Settings => this.GetCachedValue(GetSettingsModel);

		public bool IsActivePage => this.GetCachedValue(GetIsActivePage);
		public bool HasNavigationItems => this.GetCachedValue(() => NavigationItems.Any());
		public string FullUrl => this.GetCachedValue(() => Content.UrlAbsolute());
		public string Url => Content.Url;
		public string CanonicalLink => this.GetCachedValue(GetCanonicalLink);
		public bool RobotsNoindexNofollow => this.GetCachedValue(() => AppSettings.RobotsNoindexNofollow);
		public string MetaTitle => this.GetCachedValue(() => $"{SeoTitle} | {Settings?.SiteName ?? string.Empty}");

		#endregion

		IDictionary<string, object> IUmbracoCachedModel.CachedProperties { get; } = new Dictionary<string, object>();

		protected readonly UmbracoHelper UmbracoHelper = new UmbracoHelper(UmbracoContext.Current);
		protected string Name => Content.Name;

        public HeaderModel Header => this.GetCachedValue(() => UmbracoHelper.TypedContentSingleAtXPath("//" + nameof(HeaderModel).RemoveModelSuffix()).AsType<HeaderModel>());
        public FooterModel Footer => this.GetCachedValue(() => UmbracoHelper.TypedContentSingleAtXPath("//" + nameof(FooterModel).RemoveModelSuffix()).AsType<FooterModel>());


        private bool UmbracoNaviHide => this.GetPropertyValue<bool>();

		private HomeModel GetHomeModel()
		{
			return Content.AncestorOrSelf(Constants.HomePageLevel).AsType<HomeModel>();
		}

		private SettingsModel GetSettingsModel()
		{
			return UmbracoHelper.GetSingleContentOfType<SettingsModel>();
		}

		private bool GetIsActivePage()
		{
			string currentPath = UmbracoContext.Current.PublishedContentRequest.PublishedContent.Path;

			return Utility.ContainsId(currentPath, Content.Id);
		}

		private string GetCanonicalLink()
		{
			if (string.IsNullOrWhiteSpace(Settings?.CanonicalDomain))
			{
				return Content.UrlAbsolute();
			}

			return $"//{Settings.CanonicalDomain}{Url}";
		}
	}
}
