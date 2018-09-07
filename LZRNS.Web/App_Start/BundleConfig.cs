using System.Web.Configuration;
using System.Web.Optimization;

namespace LZRNS.Web
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;   //enable CDN support

			//add link to jquery on the CDN
			//const string jqueryCdnPath = "http://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js";

			//bundle styles example
			//bundles.Add(new StyleBundle("~/styles/").Include(
			//	"~/css/style.css"
			//));

			//bundle scripts example
			//bundles.Add(new ScriptBundle("~/bundles/scripts/head/").Include(
			//	"~/scripts/libs/modernizr-2.8.3-respond-1.4.2.min.js",
			//	"~/scripts/libs/detectizr.min.js"
			//));

			//bundle scripts use cdn example
			//bundles.Add(new ScriptBundle("~/bundles/scripts/jquery/", jqueryCdnPath).Include(
			//	"~/scripts/main/jquery-1.11.2.min.js"
			//));

			CompilationSection compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");

			BundleTable.EnableOptimizations = !compilationSection.Debug;
		}
	}
}