using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.Common.Extensions
{
	/// <summary>
	/// Enumerable extension methods.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Return empty enumerable collection if source is null.
		/// </summary>
		/// <typeparam name="T">The type.</typeparam>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
		{
			return source ?? Enumerable.Empty<T>();
		}

		/// <summary>
		/// Distinct by selector.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="selector">The selector.</param>
		/// <returns></returns>
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
		{
			return source.GroupBy(selector).Select(x => x.First());
		}

		/// <summary>
		/// Same as FirstOrDefault, with the null-check for the source.
		/// </summary>
		public static T FirstOrDefaultWithNullCheck<T>(this IEnumerable<T> source)
		{
			return source != null ? source.FirstOrDefault() : default(T);
		}

		/// <summary>
		/// Splits the source.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The source.</param>
		/// <param name="elementsInGroup">The elements in group.</param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<T>> SplitSource<T>(this IEnumerable<T> source, int elementsInGroup)
		{
			return source?.Select((x, i) => new { Index = i, Value = x })
				.GroupBy(x => x.Index / elementsInGroup)
				.Select(x => x.Select(v => v.Value));
		}

		/// <summary>
		/// Surround source items with brackets.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public static string ToStringWithBrackets(this IEnumerable<string> source)
		{
			return "(" + string.Join(")(", source) + ")";
		}
	}
}
