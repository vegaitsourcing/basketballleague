using LZRNS.Models.DocumentTypes.Nodes;
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
	public class ManagementPageModel : RenderModel, IUmbracoCachedModel
	{
		public ManagementPageModel() : this(new UmbracoHelper(UmbracoContext.Current).TypedContent(UmbracoContext.Current.PageId))
		{
		}

		public ManagementPageModel(IPublishedContent content) : base(content)
		{
		}

		public ManagementPageModel(IPublishedContent content, CultureInfo culture) : base(content, culture)
		{
		}

		#region [Content]

		public string PageTitle => this.GetPropertyWithDefaultValue(Name);
		public string MenuIconStyle => this.GetPropertyValue<string>();

		#endregion

		#region [Additional]

		public IEnumerable<ManagementPageModel> NavigationItems => this.GetCachedValue(() => Content.GetManagementNavigationItems());
		public SettingsModel Settings => this.GetCachedValue(() => GetSettingsModel());

		public bool IsActivePage => this.GetCachedValue(() => GetIsActivePage());
		public bool HasNavigationItems => this.GetCachedValue(() => NavigationItems.Any());
		public string FullUrl => this.GetCachedValue(() => Content.UrlAbsolute());
		public string Url => Content.Url;
		#endregion

		IDictionary<string, object> IUmbracoCachedModel.CachedProperties { get; } = new Dictionary<string, object>();

		protected readonly UmbracoHelper UmbracoHelper = new UmbracoHelper(UmbracoContext.Current);
		protected string Name => Content.Name;

		private SettingsModel GetSettingsModel()
		{
			return UmbracoHelper.GetSingleContentOfType<SettingsModel>();
		}

		private bool GetIsActivePage()
		{
			string currentPath = UmbracoContext.Current.PublishedContentRequest.PublishedContent.Path;

			return Utility.ContainsId(currentPath, Content.Id);
		}
	}
}
