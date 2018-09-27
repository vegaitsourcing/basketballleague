using Umbraco.Web;

namespace LZRNS.Web.Dictionary
{
    public class UmbracoDictionary
    {
        private static UmbracoHelper Helper => new UmbracoHelper(UmbracoContext.Current);
        public static string PrijateljiLige => Helper.GetDictionaryValue("Home.PrijateljiLige");

    }
}