using System.Collections.Generic;
using System.Linq;
using RJP.MultiUrlPicker.Models;
using LZRNS.Common.Extensions;

namespace LZRNS.Models.Extensions
{
	/// <summary>
	/// MultiUrls Picker extension methods.
	/// </summary>
	public static class MultiUrlsPickerExtensions
	{
		/// <summary>
		/// Returns first item in the specified source as a single Link, or <c>null</c> if source is empty.
		/// </summary>
		/// <param name="source">The source (expected to be <c>MultiUrls</c> instance in most cases).</param>
		/// <returns>Single Link reference (or <c>null</c>).</returns>
		public static T AsSingle<T>(this IEnumerable<T> source) where T : Link
		{
			return source.EmptyIfNull().FirstOrDefault();
		}
	}
}
