﻿using System.Web.Configuration;
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



			CompilationSection compilationSection = (CompilationSection)System.Configuration.ConfigurationManager.GetSection(@"system.web/compilation");

			BundleTable.EnableOptimizations = !compilationSection.Debug;
		}
	}
}