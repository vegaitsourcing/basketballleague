using Umbraco.Core.Models;
using LZRNS.Models.Extensions;
using LZRNS.Models.Helpers;

namespace LZRNS.Models.MediaTypes
{
	public class FileModel : CachedMediaModel
	{
		public FileModel(IPublishedContent content) : base(content)
		{
		}

		public string Url => Content.Url;
		public string Type => UmbracoExtension;
		public string FormattedSize => this.GetCachedValue(() => Utility.GetFormattedSize(UmbracoBytes));

		private string UmbracoExtension => this.GetPropertyValue<string>();
		private string UmbracoBytes => this.GetPropertyValue<string>();
	}
}
