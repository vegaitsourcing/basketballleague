using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;
using LZRNS.Common.Extensions;
using LZRNS.Models.DocumentTypes.Compositions;
using LZRNS.Models.MediaTypes;

namespace LZRNS.Models.Extensions
{
	/// <summary>
	/// Published Content extension methods.
	/// </summary>
	public static class PublishedContentExtensions
	{
		/// <summary>
		/// Returns value of specified property from the source.
		/// </summary>
		/// <remarks>
		/// This method is exactly the same as Umbraco's own <c>GetPropertyValue<T></c>, except this one will deduce property name from the caller's context, if name is omitted.
		/// </remarks>
		/// <typeparam name="T">Expected type of the property value.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>Value of the property.</returns>
		public static T GetPropertyValue<T>(this IPublishedContent source, [CallerMemberName] string propertyName = null)
		{
			return Umbraco.Web.PublishedContentExtensions.GetPropertyValue<T>(source, propertyName);
		}

		/// <summary>
		/// Returns value of specified property from the cache. If value is not in the cache it will be retrieved from Umbraco, cached and returned.
		/// </summary>
		/// <remarks>
		/// This method is similar to Umbraco's own <c>GetPropertyValue<T></c>, except this one will deduce property name from the caller's context, if name is omitted.
		/// </remarks>
		/// <typeparam name="T">Expected type of the property value. must be cached media model</typeparam>
		/// <param name="source">The cache.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>Value of the property.</returns>
		public static T GetMediaPropertyValue<T>(this IUmbracoCachedModel source, [CallerMemberName] string propertyName = null) where T : CachedMediaModel
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			return source.GetCachedValue(() => source.Content.GetPropertyValue<IPublishedContent>(propertyName).AsMediaType<T>(), propertyName);
		}

		/// <summary>
		/// Returns value of specified property from the source, or provided default value if property is not found or value is not assigned to it.
		/// </summary>
		/// <remarks>
		/// This method is exactly the same as Umbraco's own <c>GetPropertyValue<T></c>, except this one will deduce property name from the caller's context, if name is omitted.
		/// </remarks>
		/// <typeparam name="T">Expected type of the property value.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="defaultValue">Default value that will be returned if property or its value are not found.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>Value of the property or default value if property or its value are not found.</returns>
		public static T GetPropertyWithDefaultValue<T>(this IPublishedContent source, T defaultValue, [CallerMemberName] string propertyName = null)
		{
			return source.GetPropertyValue<T>(propertyName, defaultValue);
		}

		/// <summary>
		/// Checks if source has a value for the specified property.
		/// </summary>
		/// <remarks>
		/// This method is exactly the same as Umbraco's own <c>HasValue</c>, except this one will deduce property name from the caller's context, if name is omitted.
		/// </remarks>
		/// <param name="source">The source.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns><c>true</c> if source has a value for the specified property, <c>false</c> otherwise.</returns>
		public static bool HasValue(this IPublishedContent source, [CallerMemberName] string propertyName = null)
		{
			return Umbraco.Web.PublishedContentExtensions.HasValue(source, propertyName);
		}

		/// <summary>
		/// Creates instance of specified type, based on the source.
		/// </summary>
		/// <typeparam name="T">Type to create instance of.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="culture">The culture. Note: Should be used when creating a model that inherits RenderModel.</param>
		/// <returns>Instance of specified type.</returns>
		public static T AsType<T>(this IPublishedContent source, CultureInfo culture = null) where T : class
		{
			if (source == null) return default(T);

			return culture != null ? (T)Activator.CreateInstance(typeof(T), source, culture) :
									 (T)Activator.CreateInstance(typeof(T), source);
		}

		/// <summary>
		/// Creates instances of specified type, based on the source enumeration.
		/// </summary>
		/// <typeparam name="T">Type to create instances of.</typeparam>
		/// <param name="source">The source enumeration.</param>
		/// <param name="culture">The culture. Note: Should be used when creating a model that inherits RenderModel.</param>
		/// <returns>Enumeration of specified type instances.</returns>
		public static IEnumerable<T> AsType<T>(this IEnumerable<IPublishedContent> source, CultureInfo culture = null) where T : class
		{
			return source.EmptyIfNull().Where(c => c != null).Select(c => c.AsType<T>(culture));
		}

		/// <summary>
		/// Creates single instance of specified type, from the source enumeration (takes first non-<c>null</c> element).
		/// </summary>
		/// <typeparam name="T">Type to create instance of.</typeparam>
		/// <param name="source">The source enumeration.</param>
		/// <param name="culture">The culture. Note: Should be used when creating a model that inherits RenderModel.</param>
		/// <returns>Instance of specified type.</returns>
		public static T AsSingle<T>(this IEnumerable<IPublishedContent> source, CultureInfo culture = null) where T : class
		{
			return source.EmptyIfNull().FirstOrDefault(c => c != null).AsType<T>(culture);
		}

		/// <summary>
		/// Creates instance of document model type, based on the source. Created object is returned as specified base document model type.
		/// </summary>
		/// <typeparam name="TBaseDocumentTypeModel">Type of the base document model to return instance of.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="classSuffix">The suffix to append to document type alias in order to match specific document model type.</param>
		/// <returns>Instance of specified base document model type.</returns>
		public static TBaseDocumentTypeModel AsDocumentTypeModel<TBaseDocumentTypeModel>(this IPublishedContent source, string classSuffix = "Model") where TBaseDocumentTypeModel : class
		{
			if (source == null) return default(TBaseDocumentTypeModel);

			Type baseType = typeof(TBaseDocumentTypeModel);
			string modelType = $"{baseType.Namespace}.{source.DocumentTypeAlias.UppercaseFirst()}{classSuffix}";

			return (TBaseDocumentTypeModel)Activator.CreateInstance(Assembly.GetAssembly(baseType).GetType(modelType), source);
		}

		/// <summary>
		/// Creates instances of document model type, based on the source. Created objects are returned as enumeration of specified base document model type objects.
		/// </summary>
		/// <typeparam name="TBaseDocumentTypeModel">Type of the base document model to return instances of.</typeparam>
		/// <param name="source">The source enumeration.</param>
		/// <param name="classSuffix">The suffix to append to document type alias in order to match specific document model type.</param>
		/// <returns>Enumeration of specified base document model type instances.</returns>
		public static IEnumerable<TBaseDocumentTypeModel> AsDocumentTypeModel<TBaseDocumentTypeModel>(this IEnumerable<IPublishedContent> source, string classSuffix = "Model") where TBaseDocumentTypeModel : class
		{
			return source.EmptyIfNull().Where(c => c != null).Select(c => c.AsDocumentTypeModel<TBaseDocumentTypeModel>(classSuffix));
		}

		/// <summary>
		/// Creates instance of specified cached media model type, based on the source.
		/// </summary>
		/// <typeparam name="T">Type of the cached media model to create instance of.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns>Instance of specified cached media model type.</returns>
		public static T AsMediaType<T>(this IPublishedContent source) where T : CachedMediaModel
		{
			if (source == null) return default(T);

			return (T)Activator.CreateInstance(typeof(T), source);
		}

		/// <summary>
		/// Creates instances of specified cached media model type, based on the source enumeration.
		/// </summary>
		/// <typeparam name="T">Type of the cached media model to create instances of.</typeparam>
		/// <param name="source">The source enumeration.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>Enumeration of specified cached media model type instances.</returns>
		public static IEnumerable<T> AsMediaType<T>(this IEnumerable<IPublishedContent> source, CultureInfo culture = null) where T : CachedMediaModel
		{
			return source.EmptyIfNull().Where(c => c != null).Select(c => c.AsType<T>(culture));
		}

		/// <summary>
		/// Returns child node model from the source, based on provided document type alias.
		/// </summary>
		/// <typeparam name="T">Type of the model to return.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="documentTypeAlias">The document type alias.</param>
		/// <returns>Child node model.</returns>
		public static T GetChildNode<T>(this IPublishedContent source, string documentTypeAlias = null) where T : class
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			return source.Descendant(documentTypeAlias ?? typeof(T).Name.RemoveModelSuffix()).AsType<T>();
		}

		/// <summary>
		/// Returns children from the source for the navigation.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Collection of navigation items.</returns>
		public static IEnumerable<PageModel> GetNavigationItems(this IPublishedContent source)
		{
			return source?.Children(c => c.TemplateId > 0 && c.IsVisible()).AsType<PageModel>() ?? Enumerable.Empty<PageModel>();
		}

		/// <summary>
		/// Returns children from the source for the navigation.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Collection of navigation items for management.</returns>
		public static IEnumerable<ManagementPageModel> GetManagementNavigationItems(this IPublishedContent source)
		{
			return source?.Children(c => c.IsVisible()).AsType<ManagementPageModel>() ?? Enumerable.Empty<ManagementPageModel>();
		}

		/// <summary>
		/// Returns sidebar navigation items for the source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="level">The level.</param>
		/// <returns>Collection of sidebar navigation items.</returns>
		public static IEnumerable<PageModel> GetSideBarNavigationItems(this IPublishedContent source, int level)
		{
			if (level < 1)
			{
				throw new ArgumentException($"'{nameof(level)}' value must be greater than 0!", nameof(level));
			}

			return source?.AncestorOrSelf(level).Children(c => c.TemplateId > 0 && c.IsVisible()).AsType<PageModel>() ?? Enumerable.Empty<PageModel>();
		}

		/// <summary>
		/// Checks if source should be included in the sitemap.
		/// </summary>
		/// <remarks>
		/// Value for property with alias "hideFromSitemap" is retrieved and checked.
		/// </remarks>
		/// <param name="source">The source.</param>
		/// <returns><c>true</c> if source should be included in the sitemap, <c>false</c> otherwise.</returns>
		public static bool IsIncludedInSitemap(this IPublishedContent source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			return !source.GetPropertyValue<bool>("hideFromSitemap");
		}

		/// <summary>
		/// Returns children from the source for the sitemap.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Collection of sitemap items.</returns>
		public static IEnumerable<PageModel> GetSitemapItems(this IPublishedContent source)
		{
			return source?.Children(c => c.TemplateId > 0 && c.IsIncludedInSitemap()).AsType<PageModel>() ?? Enumerable.Empty<PageModel>();
		}

		/// <summary>
		/// Checks if source should be included in the Google search (and consequently in Sitemap XML).
		/// </summary>
		/// <remarks>
		/// Value for property with alias "hideFromGoogleSearch" is retrieved and checked.
		/// </remarks>
		/// <param name="source">The source.</param>
		/// <returns><c>true</c> if source should be included in the Google search, <c>false</c> otherwise.</returns>
		public static bool IsIncludedInGoogleSearch(this IPublishedContent source)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));

			return !source.GetPropertyValue<bool>("hideFromGoogleSearch");
		}

		/// <summary>
		/// Returns children from the source for the Google Search i.e. Sitemap XML.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns>Collection of sitemap XML items.</returns>
		public static IEnumerable<PageModel> GetSitemapXMLItems(this IPublishedContent source)
		{
			return source?.Children(c => c.TemplateId > 0 && c.IsIncludedInGoogleSearch()).AsType<PageModel>() ?? Enumerable.Empty<PageModel>();
		}
	}
}
