using Microsoft.Win32;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using LZRNS.Common.Extensions;

namespace LZRNS.Models.Helpers
{
	/// <summary>
	/// Contains utility methods
	/// </summary>
	public static class Utility
	{
		/// <summary>
		/// Gets the MIME type from file extension.
		/// </summary>
		/// <param name="extension">The file extension.</param>
		/// <returns></returns>
		public static string GetMimeTypeFromExtension(string extension)
		{
			if (!extension.StartsWith("."))
				extension = "." + extension;

			RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, false);
			string mimeType = key?.GetValue("Content Type")?.ToString();

			return !string.IsNullOrWhiteSpace(mimeType) ? mimeType : "application/octet-stream";
		}

		/// <summary>
		/// Normilizes the req parameter.
		/// </summary>
		/// <param name="param">The parameter.</param>
		/// <returns></returns>
		public static string NormilizeReqParameter(string param)
		{
			return param.Replace("%20", " ");
		}


		/// <summary>
		/// Checks if path contains identifier.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public static bool ContainsId(string path, int id)
		{
			return path.Split(',').Any(pathId => pathId.EqualsInt(id));
		}


		/// <summary>
		/// Adds the links to tweets.
		/// </summary>
		/// <param name="inputText">The input text.</param>
		/// <returns>Replaced text</returns>
		public static string AddLinksToTweets(string inputText)
		{
			string replacedText = inputText;
			string replacePattern1 = @"(http:\/\/[^ ]+)";
			replacedText = Regex.Replace(replacedText, replacePattern1, "<a href=\"$1\" target=\"_blank\">$1</a>", RegexOptions.Compiled);

			string replacePattern2 = @"@([a-z0-9_]+)";
			replacedText = Regex.Replace(replacedText, replacePattern2, "<a href=\"http://twitter.com/@$1\" target=\"_blank\">@$1</a>", RegexOptions.Compiled);

			string replacePattern3 = @"#([a-zA-Z0-9_]*)";
			replacedText = Regex.Replace(replacedText, replacePattern3, "<a href=\"http://search.twitter.com/search?q=%23$1\" target=\"_blank\">#$1</a>", RegexOptions.Compiled);

			return replacedText;
		}

		/// <summary>
		/// Gets the full URL path.
		/// </summary>
		/// <param name="relativePath">The relative path.</param>
		/// <returns>Ful path url.</returns>
		public static string GetFullUrlPath(string relativePath)
		{
			StringBuilder retVal = new StringBuilder();
			string host = HttpContext.Current.Request.Url.Host;

			if (HttpContext.Current.Request.IsSecureConnection)
			{
				retVal.Append("https://");
			}
			else
			{
				retVal.Append("http://");
			}

			retVal.Append(host).Append(relativePath);

			return retVal.ToString();
		}

		/// <summary>
		/// Returns the media file size expressed in KB or MB.
		/// </summary>
		/// <param name="mediaSize">The media file size.</param>
		/// <returns>Media file size expressed in KB or MB.</returns>
		public static string GetFormattedSize(string mediaSize)
		{
			const int bytesInMegabyte = 1048576;
			const int bytesInKilobyte = 1024;

			if (string.IsNullOrEmpty(mediaSize))
			{
				return string.Empty;
			}

			decimal tmpMb = decimal.Parse(mediaSize) / bytesInMegabyte;
			if (tmpMb >= 1)
			{
				return $"{Math.Round(tmpMb, 2).ToString(CultureInfo.InvariantCulture)}MB";
			}

			decimal tmpKb = decimal.Parse(mediaSize) / bytesInKilobyte;
			return $"{Math.Round(tmpKb, 2).ToString(CultureInfo.InvariantCulture)}KB";
		}
	}
}
