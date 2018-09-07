using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.DocumentTypes.Nodes;
using LZRNS.Models.Extensions;
using LZRNS.Web.Extensions;

namespace LZRNS.Web.HttpHandlers
{
	public class RobotsHandler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			string url = context.Request.RawUrl;
			string path = context.Server.MapPath(url);

			// Do we have a file that already exists on the file system?
			// If so, always serve that.
			if (File.Exists(path))
			{
				LogHelper.Debug<RobotsHandler>("Streaming specified robots file from disk.");

				context.Response.TransmitFileContent(path);
				return;
			}

			// Ensure there is an Umbraco context that is necessary for models binding.
			UmbracoContext umbracoContext = EnsureUmbracoContext(context);
			if (umbracoContext == null)
			{
				LogHelper.Debug<RobotsHandler>("Umbraco context is null even after ensuring we have a context.");
				throw new HttpException((int)HttpStatusCode.NotFound, "Page Not Found");
			}

			// Lets try and find the robots file contents from Umbraco.
			// NOTE: Explicitly specifying culture is necessary as UmbracoContext.PublishedContentRequest is null
			//       which will cause RenderModel constructor to fail trying to get request culture.
			SettingsModel settings = umbracoContext.TypedContentAtDomainRoot().AsType<PageModel>(CultureInfo.InvariantCulture)?.Settings;

			context.Response.Clear();
			context.Response.ContentType = "text/plain";
			context.Response.Write(settings != null ? settings.Robots : string.Empty);
			context.Response.End();
		}

		public bool IsReusable => true;

		private UmbracoContext EnsureUmbracoContext(HttpContext context)
		{
			if (UmbracoContext.Current != null)
			{
				return UmbracoContext.Current;
			}

			HttpContextWrapper contextWrapper = new HttpContextWrapper(context);

			return UmbracoContext.EnsureContext(
						contextWrapper,
						ApplicationContext.Current,
						new WebSecurity(contextWrapper, ApplicationContext.Current),
						UmbracoConfig.For.UmbracoSettings(),
						UrlProviderResolver.Current.Providers,
						true
					);
		}
	}
}
