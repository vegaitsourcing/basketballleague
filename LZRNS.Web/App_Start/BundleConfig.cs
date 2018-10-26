using System.Web.Configuration;
using System.Web.Optimization;
using LZRNS.Common;

namespace LZRNS.Web
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.UseCdn = true;   //enable CDN support

			bundles.Add(new StyleBundle("~/bundles/styles/main").Include(
				"~/css/style.min.css",
				new CssRewriteUrlTransform()
			));

			bundles.Add(new ScriptBundle("~/bundles/scripts/main").Include(
				"~/js/global.min.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/scripts/unobtrusive").Include(
				"~/scripts/jquery.unobtrusive-ajax.js"
			));

			bundles.Add(new StyleBundle("~/bundles/styles/management/").Include(
				"~/vendor/bootstrap/css/bootstrap.min.css",
				"~/Content/jquery-ui.min.css",
				"~/Content/tooltipster.bundle.min.css",
				"~/Content/tooltipster.bundle.min.css",
				"~/Content/tooltipster-sideTip-shadow.min.css",
				"~/Content/css/sb-admin.css",
				"~/Content/Site.css",
				"~/vendor/datatables/dataTables.bootstrap4.css"
			));


			bundles.Add(new StyleBundle("~/bundles/styles/fontawesome/").Include(
				"~/vendor/font-awesome/css/font-awesome.min.css",
				new CssRewriteUrlTransform()
			));

			bundles.Add(new ScriptBundle("~/bundles/scripts/libs/").Include(
				"~/vendor/jquery/jquery.min.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/scripts/unobtrusive/").Include(
				"~/Scripts/jquery-ui.min.js",
				"~/scripts/jquery.validate.min.js",
				"~/scripts/jquery.validate.unobtrusive.min.js",
				"~/scripts/jquery.unobtrusive-ajax.js"
			));

			bundles.Add(new ScriptBundle("~/bundles/scripts/management/").Include(
				"~/vendor/bootstrap/js/bootstrap.bundle.min.js",
				"~/vendor/jquery-easing/jquery.easing.min.js",
				"~/Scripts/js/sb-admin.min.js",
				"~/Scripts/tooltipster.bundle.min.js",
				"~/vendor/datatables/jquery.dataTables.js",
				"~/vendor/datatables/dataTables.bootstrap4.js",
				"~/Scripts/js/sb-admin-datatables.js",
				"~/js/management.js"
			));

			BundleTable.EnableOptimizations = AppSettings.BundleEnabled;
		}
	}
}