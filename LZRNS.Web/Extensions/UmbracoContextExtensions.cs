using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace LZRNS.Web.Extensions
{
	/// <summary>
	/// Umbraco Context extension methods.
	/// </summary>
	public static class UmbracoContextExtensions
	{
		/// <summary>
		/// Returns content of the root node of the domain provided Umbraco Context is associated with.
		/// </summary>
		/// <param name="context">Umbraco Context.</param>
		/// <returns>Content of the domain root node or the first node in the content tree.</returns>
		public static IPublishedContent TypedContentAtDomainRoot(this UmbracoContext context)
		{
			UmbracoHelper umbracoHelper = new UmbracoHelper(context);

			// Finds appropriate Umbraco domain for current request URL
			IEnumerable<IDomain> validDomains = ApplicationContext.Current.Services.DomainService.GetAll(false)
																  .Where(d => context.HttpContext.Request.Url.AbsoluteUri.Contains(d.DomainName));
			IDomain domain = validDomains.Any() ? validDomains.Aggregate((mostSpecific, d) => d.DomainName.Length > mostSpecific.DomainName.Length ? d : mostSpecific) : null;

			// Retrieves either the current root node for the domain or the first root node in the content tree
			return (domain != null && domain.RootContentId.HasValue) ? umbracoHelper.TypedContent(domain.RootContentId) : umbracoHelper.TypedContentAtRoot().FirstOrDefault();
		}
	}
}
