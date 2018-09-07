using System.Text;

namespace LZRNS.Common.Extensions
{
	/// <summary>
	/// String Builder extension methods.
	/// </summary>
	public static class StringBuilderExtensions
	{
		/// <summary>
		/// Append property value.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="value">The value.</param>
		public static void AppendPropertyValue(this StringBuilder source, object value)
		{
			if (!string.IsNullOrEmpty(value?.ToString()))
			{
				source.AppendLine(value.ToString());
			}
		}
	}
}