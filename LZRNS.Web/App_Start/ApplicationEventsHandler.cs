using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Umbraco.Core;
using LZRNS.Web.RazorViewEngines;
using LZRNS.Models.DocumentTypes.Compositions;
using Umbraco.Web.Routing;
using LZRNS.Models.Extensions;

namespace LZRNS.Web
{
	public class ApplicationEventsHandler : ApplicationEventHandler
	{
		protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			base.ApplicationStarting(umbracoApplication, applicationContext);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			ViewEngines.Engines.Add(new PartialViewEngine());
			PublishedContentRequest.Prepared += PublishedContentRequest_Prepared;
		}

		protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			base.ApplicationStarted(umbracoApplication, applicationContext);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		private void PublishedContentRequest_Prepared(object sender, EventArgs e)
		{
			PublishedContentRequest request = sender as PublishedContentRequest;
			if (request == null || !request.HasPublishedContent)
			{
				return;
			}

			string externalRedirect = request.PublishedContent.GetPropertyValue<string>(nameof(PageModel.ExternalRedirect));
			if (!string.IsNullOrWhiteSpace(externalRedirect))
			{
				request.SetRedirect(externalRedirect);
			}
		}
	}
}