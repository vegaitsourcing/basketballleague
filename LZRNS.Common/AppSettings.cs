using System.ComponentModel;
using System.Configuration;

namespace LZRNS.Common
{
	public static class AppSettings
	{
		public static int SitemapDepthLevelDefaultValue => Get<int>("sitemapDepthLevelDefaultValue");
		public static bool RobotsNoindexNofollow => Get<bool>("robotsNoindexNofollow");
		public static string MediaDirectoryPath => Get<string>("media-location");
		public static bool BundleEnabled => Get<bool>("bundle-enabled");

		public static T Get<T>(string key)
		{
			string setting = ConfigurationManager.AppSettings[key];
			if (string.IsNullOrWhiteSpace(setting))
			{
				throw new ConfigurationErrorsException($"Key '{key}' not found in the configuration file!");
			}

			TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
			return (T)converter.ConvertFromInvariantString(setting);
		}
	}
}
