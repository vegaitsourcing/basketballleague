using System.IO;
using System.Web;

namespace LZRNS.Web.Extensions
{
	/// <summary>
	/// HTTP Response extension methods.
	/// </summary>
	public static class HttpResponseExtensions
	{
		/// <summary>
		/// Writes content of the file to the HTTP response, ensuring that valid content type is specified.
		/// </summary>
		/// <param name="response">HTTP response.</param>
		/// <param name="path">Path to the file whose content will be written to the response.</param>
		public static void TransmitFileContent(this HttpResponse response, string path)
		{
			string filename = Path.GetFileName(path);
			if (filename == null)
			{
				return;
			}

			response.ContentType = MimeMapping.GetMimeMapping(filename);
			response.TransmitFile(path);
		}
	}
}
