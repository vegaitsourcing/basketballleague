using System.Web.Routing;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Core.Models;
using LZRNS.Web.Extensions;

namespace LZRNS.Web
{
	/// <summary>
	/// Provides root node for the current request
	/// </summary>
	public class DomainRootRouteHandler : UmbracoVirtualNodeRouteHandler
	{
		public DomainRootRouteHandler()
		{ }

		protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext) => umbracoContext.TypedContentAtDomainRoot();
	}
}
