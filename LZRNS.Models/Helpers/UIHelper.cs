using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZRNS.Common.Extensions;

namespace LZRNS.Models.Helpers
{
	/// <summary>
	/// Provides methods for UI.
	/// </summary>
	public static class UIHelper
    {
        public static string GetSourceFromIframe(string iframe)
        {
	        string retVal = iframe.Replace("\"", "");
            int indexOfSrc = retVal.IndexOf("src=", StringComparison.Ordinal);
            if (indexOfSrc > 0)
            {
                retVal = retVal.Substring(indexOfSrc + 4, retVal.Substring(indexOfSrc + 4).IndexOf(" ", StringComparison.Ordinal));
            }

            return retVal;
        }
		
	    /// <summary>
	    /// Gets the shorter text.
	    /// </summary>
	    /// <param name="text">The text that need to be smaller.</param>
	    /// <param name="numberOfChars">The number of chars.</param>
	    /// <param name="roundLastWord">if set to <c>true</c> [round last word].</param>
	    /// <param name="addThreeDots">If set to <c>true</c> add three dots on the end.</param>
	    /// <returns>
	    /// Substring of text if text lenght is grater that numberOfChars, otherwise returns text.
	    /// </returns>
	    public static string GetShorterText(string text, int numberOfChars, bool roundLastWord, bool addThreeDots = true)
        {
            if (string.IsNullOrEmpty(text))
            {
	            return string.Empty;
            }

			text = text.StripHtml();
			if (text.Length < numberOfChars)
			{
				return text;
			}

			text = text.Substring(0, numberOfChars);

			if (roundLastWord)
			{
				int lastSpaceIndex = text.LastIndexOf(' ');
				if (lastSpaceIndex != -1)
				{
					text = text.Substring(0, lastSpaceIndex);
				}
			}

			return text + (addThreeDots ? "..." : string.Empty);
        }
		
        /// <summary>
        /// Converts the plain text to HTML.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>Html representation of plain text.</returns>
        public static string ConvertPlainTextToHtml(string plainText)
        {
            StringBuilder builder = new StringBuilder();
            bool previousWasASpace = false;
            foreach (char c in plainText)
            {
                if (c == ' ')
                {
                    if (previousWasASpace)
                    {
                        builder.Append("&nbsp;");
                        previousWasASpace = false;
                        continue;
                    }

                    previousWasASpace = true;
                }
                else
                {
                    previousWasASpace = false;
                }

                switch (c)
                {
                    case '<':
                        builder.Append("&lt;");
                        break;

                    case '>':
                        builder.Append("&gt;");
                        break;

                    case '&':
                        builder.Append("&amp;");
                        break;

                    case '"':
                        builder.Append("&quot;");
                        break;

                    case '\n':
                        builder.Append("<br>");
                        break;
                    //// We need Tab support here, because we print StackTraces as HTML
                    case '\t':
                        builder.Append("&nbsp; &nbsp; &nbsp;");
                        break;

                    default:
                        if (c < 128)
                        {
                            builder.Append(c);
                        }
                        else
                        {
                            builder.Append("&#").Append((int)c).Append(";");
                        }

                        break;
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the in between substring.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="markerStart">The marker start.</param>
        /// <param name="markerEnd">The marker end.</param>
        /// <returns>Gets inner text.</returns>
        public static string GetInBetweenSubstring(string text, string markerStart, string markerEnd)
        {
            int start = text.IndexOf(markerStart, StringComparison.Ordinal);
            int end = text.LastIndexOf(markerEnd, StringComparison.Ordinal);

            if (start == -1)
            {
                throw new Exception(string.Format("Marker for usercontrol start ('{0}') not found in '{1}'", markerStart, text));
            }

            if (end == -1)
            {
                throw new Exception(string.Format("Marker for usercontrol end ('{0}') not found in '{1}'", markerStart, text));
            }

            if (start > end)
            {
                throw new Exception(string.Format("Start and End markers not possitioned well in '{0}'", text));
            }

            start += markerStart.Length;

            string retVal = text.Substring(start, end - start);

            return retVal;
        }
		
        /// <summary>
        /// Removes the paragraph tag.
        /// </summary>
        /// <param name="text">The origin text.</param>
        /// <returns>Text without rounded paragraphs.</returns>
        public static string GetTextWithoutParagraphTag(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string pharTagOpen = "<p>";
                string pharTagClose = "</p>";
                text = text.Replace(Environment.NewLine, string.Empty);

                while (text.StartsWith(pharTagOpen) && text.EndsWith(pharTagClose))
                {
                    int start = text.IndexOf(pharTagOpen, StringComparison.Ordinal);
                    int end = text.LastIndexOf(pharTagClose, StringComparison.Ordinal);
                    start += pharTagOpen.Length;

                    text = text.Substring(start, end - start);
                }
            }

            return text;
        }

        /// <summary>
        /// Replaces the new line with paragraph.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>Text without br.</returns>
        public static string ReplaceNewLineWithParagraph(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace(Environment.NewLine, string.Empty);
                text = text.Replace("<br />", "</p><p>");
            }

            return text;
        }

        /// <summary>
        /// Gets the paragraphs.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>Paragraphs in text.</returns>
        public static List<string> GetParagraphs(string text)
        {
            List<string> paragraphs = new List<string>();

            if (!string.IsNullOrEmpty(text))
            {
                string[] stringSeparators = { "<p>", "</p>", "<br />" };
                string[] paragraphArray = text.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                paragraphs = paragraphArray.ToList();
            }

            return paragraphs;
        }

        /// <summary>
        /// Replaces the empty characters.
        /// </summary>
        /// <param name="text">The original text.</param>
        /// <returns>String text.</returns>
        public static string ReplaceNewLineCharacters(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\n", string.Empty).Replace("\r", string.Empty);
            }

            return text;
        }
		
		/// <summary>
		/// Highlight text between **,**
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
        public static string HighlightText(string text)
        {
            string retVal = text;
            StringBuilder builder = new StringBuilder();

            int firstIndex = text.IndexOf("**", StringComparison.Ordinal);
            if (firstIndex > -1)
            {
                string temp = text.Substring(firstIndex + 2);
                int secondIndex = temp.IndexOf("**", StringComparison.Ordinal);

                if (secondIndex > -1)
                {
                    builder.Append(text.Substring(0, firstIndex));
                    builder.Append("<strong>");
                    builder.Append(temp.Substring(0, secondIndex));
                    builder.Append("</strong>");
                    builder.Append(temp.Substring(secondIndex + 2));
                    retVal = builder.ToString();
                    retVal = HighlightText(retVal);
                }
            }

            return retVal;
        }
    }
}
