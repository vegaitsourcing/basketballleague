using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace LZRNS.Models.Extensions
{
	/// <summary>
	/// Interface that should be implemented by classes that will use Umbraco property values caching
	/// </summary>
	public interface IUmbracoCachedModel
	{
		IPublishedContent Content { get; }
		IDictionary<string, object> CachedProperties { get; }
	}

	/// <summary>
	/// Umbraco Cached Model extension methods.
	/// </summary>
	public static class UmbracoCachedModelExtensions
	{
		/// <summary>
		/// Returns value of specified property from the cache. If value is not in the cache it will be retrieved from Umbraco, cached and returned.
		/// </summary>
		/// <remarks>
		/// This method is similar to Umbraco's own <c>GetPropertyValue<T></c>, except this one will deduce property name from the caller's context, if name is omitted.
		/// </remarks>
		/// <typeparam name="T">Expected type of the property value.</typeparam>
		/// <param name="source">The cache.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>Value of the property.</returns>
		public static T GetPropertyValue<T>(this IUmbracoCachedModel source, [CallerMemberName] string propertyName = null)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			return source.GetCachedValue(() => source.Content.GetPropertyValue<T>(propertyName), propertyName);
		}

		/// <summary>
		/// Returns value of specified property from the cache. If value is not in the cache it will be retrieved from Umbraco, cached and returned.
		/// If property is not found in Umbraco or value is not assigned to it, provided default value will be cached and returned.
		/// </summary>
		/// <remarks>
		/// This method is similar to Umbraco's own <c>GetPropertyValue<T></c>, except this one will deduce property name from the caller's context, if name is omitted.
		/// </remarks>
		/// <typeparam name="T">Expected type of the property value.</typeparam>
		/// <param name="source">The cache.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>Value of the property or default value if property or its value are not found.</returns>
		public static T GetPropertyWithDefaultValue<T>(this IUmbracoCachedModel source, T defaultValue, [CallerMemberName] string propertyName = null)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			return source.GetCachedValue(() => source.Content.GetPropertyValue<T>(propertyName, defaultValue), propertyName);
		}

		/// <summary>
		/// Returns value with specified key from the cache. If key is not in the cache, specified function will be invoked to retrieve a value which will be cached under provided key.
		/// </summary>
		/// <typeparam name="T">Type of the value.</typeparam>
		/// <param name="source">The cache.</param>
		/// <param name="retrieveValueFunc">Function that will be invoked if given key is not in the cache.</param>
		/// <param name="cacheKey">The key that the value is (will be) stored under.</param>
		/// <returns>Value from the cache</returns>
		public static T GetCachedValue<T>(this IUmbracoCachedModel source, Func<T> retrieveValueFunc, [CallerMemberName] string cacheKey = null)
		{
			return source.AddOrUpdateCache(cacheKey, retrieveValueFunc);
		}

		private static T AddOrUpdateCache<T>(this IUmbracoCachedModel source, string cacheKey, Func<T> retrieveValueFunc)
		{
			if (source == null) throw new ArgumentNullException(nameof(source));
			if (cacheKey == null) throw new ArgumentNullException(nameof(cacheKey));
			if (retrieveValueFunc == null) throw new ArgumentNullException(nameof(retrieveValueFunc));

			if (!source.CachedProperties.ContainsKey(cacheKey))
			{
				source.CachedProperties.Add(cacheKey, retrieveValueFunc.Invoke());
			}

			return (T)source.CachedProperties[cacheKey];
		}
	}
}
