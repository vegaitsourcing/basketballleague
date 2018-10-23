using System.Linq;
using System.Web;
using System.Web.Routing;
using Umbraco.Core;

namespace LZRNS.Web
{
	public class ExistingLanguagesRouteConstraint : IRouteConstraint
	{
		public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
		{
			string language = values[parameterName].ToString();
			if (string.IsNullOrWhiteSpace(language))
			{
				return false;
			}

			// Checks if there is appropriate Umbraco domain for specified language, based on current request URL
			return ApplicationContext.Current.Services.DomainService.GetAll(false)
									 .Where(d => d.DomainName.Contains($"{httpContext.Request.Url.Host}/{language}"))
									 .Any();
		}
	}
}
