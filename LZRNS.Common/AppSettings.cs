using LZRNS.Common.Extensions;
using System.ComponentModel;
using System.Configuration;

namespace LZRNS.Common
{
    public static class AppSettings
    {
        public static int SitemapDepthLevelDefaultValue => Get<int>("sitemapDepthLevelDefaultValue");
        public static int StatisticsTableTopStatsToShow => Get("statisticsTableTopStatsToShow", 5);

        public static bool RobotsNoindexNofollow => Get<bool>("robotsNoindexNofollow");
        public static bool BundleEnabled => Get<bool>("bundle-enabled");

        public static T Get<T>(string key)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(setting))
            {
                throw new ConfigurationErrorsException($"Key '{key}' not found in the configuration file!");
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(setting);
        }

        public static T Get<T>(string key, T defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting.IsNullOrWhitespace())
            {
                return defaultValue;
            }

            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(setting);
        }
    }
}