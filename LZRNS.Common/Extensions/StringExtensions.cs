using System;
using System.Text.RegularExpressions;

namespace LZRNS.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Check is source string null or whitespace.
        /// </summary>
        public static bool IsNullOrWhitespace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Check if source string is equal to some integer value.
        /// </summary>
        public static bool EqualsInt(this string source, int i)
        {
            return source.Equals(i.ToString());
        }

        /// <summary>
        /// Strips the HTML tags from specified source.
        /// </summary>
        public static string StripHtml(this string source)
        {
            return Regex.Replace(source, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Remove suffix from the word.
        /// </summary>
        public static string RemoveSuffix(this string source, string suffix)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (suffix == null) throw new ArgumentNullException(nameof(suffix));

            if (!source.Contains(suffix)) return source;

            return source.Substring(0, source.Length - suffix.Length);
        }

        /// <summary>
        /// Returns the given string with the first character upper-cased.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>String with the first character upper-cased.</returns>
        public static string UppercaseFirst(this string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            return source[0].ToString().ToUpper() + source.Substring(1);
        }

        /// <summary>
        /// Extracts first number in a string (ex. "--20-GAME-4" will return 20)
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Non-negative number upon success, negative number upon error</returns>
        public static int ExtractNumber(this string source)
        {
            var match = Regex.Match(source, @"\d+");
            return !match.Success ? -1 : int.Parse(match.Value);
        }
    }
}