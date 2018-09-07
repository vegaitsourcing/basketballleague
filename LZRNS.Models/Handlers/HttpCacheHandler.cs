using System;
using System.Web;
using System.Web.Caching;

namespace LZRNS.Models.Handlers
{
	public class HttpCacheHandler
	{
		/// <summary>
		/// Sets the value with absolute expiration.
		/// </summary>
		/// <param name="key">The key of object.</param>
		/// <param name="value">The value of object.</param>
		/// <param name="expirationTime">The expiration time.</param>
		public static void SetValueWithAbsoluteExpiration(string key, object value, DateTime expirationTime)
		{
			HttpContext.Current.Cache.Insert(key, value, null, expirationTime, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
		}

		/// <summary>
		/// Sets the value with absolute expiration.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="value">The value of cache object.</param>
		/// <param name="expirationMinutes">The expiration minutes.</param>
		public static void SetValueWithAbsoluteExpiration(string key, object value, int expirationMinutes)
		{
			HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(expirationMinutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
		}

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public static void SetValue(string key, object value)
		{
			HttpContext.Current.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="key">The key name of object in cache.</param>
		/// <returns>Gets object from cache.</returns>
		public static object GetValue(string key)
		{
			return HttpContext.Current.Cache[key];
		}

		/// <summary>
		/// Removes the value.
		/// </summary>
		/// <param name="key">The key.</param>
		public static void RemoveValue(string key)
		{
			HttpContext.Current.Cache.Remove(key);
		}
	}
}