using System;
using System.Text.RegularExpressions;

namespace LZRNS.Common.Extensions
{
    /// <summary>
    /// String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Check is source string null or whitespace.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Return empty string if source is null.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string EmptyIfNull(this string source)
        {
            return source ?? string.Empty;
        }

        /// <summary>
        /// Split string by separator, with null check.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string[] SplitWithNullCheck(this string source, char separator)
        {
            return source.EmptyIfNull().Split(separator);
        }

        /// <summary>
        /// Check if source string is equal to some integer value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="i">The integer value.</param>
        /// <returns></returns>
        public static bool EqualsInt(this string source, int i)
        {
            return source.Equals(i.ToString());
        }

        /// <summary>
        /// Strips the HTML tags from specified source.
        /// </summary>
        /// <param name="source">The input text.</param>
        /// <returns>Text without HTML tags.</returns>
        public static string StripHtml(this string source)
        {
            return Regex.Replace(source, @"<(.|\n)*?>", string.Empty);
        }

        /// <summary>
        /// Trim empty spaces.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string TrimEmptySpaces(this string source)
        {
            return Regex.Replace(source, @"\s", " ");
        }

        /// <summary>
        /// Remove suffix from the word.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        public static string RemoveSuffix(this string source, string suffix)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (suffix == null) throw new ArgumentNullException(nameof(suffix));

            if (!source.Contains(suffix)) return source;

            return source.Substring(0, source.Length - suffix.Length);
        }

        /// <summary>
        /// Replace line breaks in text with <br/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ReplaceLineBreaksForHtml(this string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.Replace(Environment.NewLine, "<br/>" + Environment.NewLine).Replace("\n", "<br/>");
        }

        /// <summary>
        /// Returns the given string with the first character uppercased.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>String with the first character uppercased.</returns>
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