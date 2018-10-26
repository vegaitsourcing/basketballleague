using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Umbraco.Web;

namespace LZRNS.Models.Extensions
{
	public static class UmbracoHelperExtensions
	{
		public static T GetSingleContentOfType<T>(this UmbracoHelper helper, CultureInfo culture = null) where T : class, IUmbracoCachedModel
		{
			if (helper == null) return default(T);

			return helper.TypedContentSingleAtXPath(GetXpath(typeof(T))).AsType<T>(culture);
		}

		public static IEnumerable<T> GetContentOfType<T>(this UmbracoHelper helper) where T : class, IUmbracoCachedModel
		{
			if (helper == null) return Enumerable.Empty<T>();

			return helper.TypedContentAtXPath(GetXpath(typeof(T))).AsType<T>();
		}

		private static string GetXpath(Type t)
		{
			return $"//*[@isDoc and translate(name(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '{t.Name.RemoveModelSuffix().ToLower()}']";
		}
	}
}
