using System.Web.Routing;
using Umbraco.Web;

namespace LZRNS.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			// Routes for the XML sitemap functionality
			routes.MapUmbracoRoute(
				"SitemapXML",
				"XMLSitemap",
				new
				{
					controller = "XMLSitemap",
					action = "XMLSitemap"
				},
				new DomainRootRouteHandler()
			);

			routes.MapUmbracoRoute(
				"LanguageSpecificSitemapXML",
				"{language}/XMLSitemap",
				new
				{
					controller = "XMLSitemap",
					action = "XMLSitemap"
				},
				new DomainRootRouteHandler(),
				new { language = new ExistingLanguagesRouteConstraint() }
			);
		}
	}
}
