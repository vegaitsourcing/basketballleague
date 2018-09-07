using Umbraco.Core.Models;
using LZRNS.Models.Extensions;

namespace LZRNS.Models.DocumentTypes.Nodes
{
	public class SettingsModel : CachedContentModel
	{
		public SettingsModel(IPublishedContent content) : base(content)
		{
		}

		public string SiteName => this.GetPropertyValue<string>();
		public string Robots => this.GetPropertyValue<string>();
		public string CanonicalDomain => this.GetPropertyValue<string>();
	}
}
