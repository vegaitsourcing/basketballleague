using Umbraco.Core.Models;

namespace LZRNS.Models.MediaTypes
{
	public class CachedMediaModel : CachedContentModel
	{
		public CachedMediaModel(IPublishedContent content) : base(content)
		{
		}
	}
}
