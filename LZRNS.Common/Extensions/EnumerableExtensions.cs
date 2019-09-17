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
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>
        /// Distinct by selector.
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.GroupBy(selector).Select(x => x.First());
        }

        /// <summary>
		/// Splits the source.
		/// </summary>
		public static IEnumerable<IEnumerable<T>> SplitSource<T>(this IEnumerable<T> source, int elementsInGroup)
        {
            return source?.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / elementsInGroup)
                .Select(x => x.Select(v => v.Value));
        }

        /// <summary>
        /// Surround source items with brackets.
        /// </summary>
        public static string ToStringWithBrackets(this IEnumerable<string> source)
        {
            return "(" + string.Join(")(", source) + ")";
        }
    }
}