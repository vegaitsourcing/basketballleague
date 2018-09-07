using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;
using LZRNS.Models.Extensions;

namespace LZRNS.Models.MediaTypes
{
	public class FolderModel : CachedMediaModel
	{
		public FolderModel(IPublishedContent content) : base(content)
		{
		}

		public IEnumerable<ImageModel> Images => this.GetCachedValue(() => GetImageModels());
		public IEnumerable<FileModel> Files => this.GetCachedValue(() => GetFileModels());

		private IEnumerable<ImageModel> GetImageModels()
		{
			return Content.Children(c => c.DocumentTypeAlias.Equals("Image")).AsMediaType<ImageModel>();
		}

		private IEnumerable<FileModel> GetFileModels()
		{
			return Content.Children(c => c.DocumentTypeAlias.Equals("File")).AsMediaType<FileModel>();
		}
	}
}
