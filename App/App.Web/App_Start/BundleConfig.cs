using System.Web.Optimization;

namespace App.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{

			bundles.IgnoreList.Clear();

			bundles.Add(new StyleBundle("~/bundles/administration").Include(
						"~/Content/bootstrap.min.css",
						"~/Content/font-awesome.css",
						"~/Content/administration.css"
				));

			bundles.Add(new ScriptBundle("~/bundles/unitegallery-scripts").Include(
						"~/Content/unitegallery/js/unitegallery.min.js",
						"~/Content/unitegallery/themes/tiles/ug-theme-tiles.js"
				));

			bundles.Add(new StyleBundle("~/bundles/unitegallery-styles").Include(
						"~/Content/unitegallery/css/unite-gallery.css"
				));

			bundles.Add(new ScriptBundle("~/bundles/dropzone-scripts").Include(
						"~/Content/dropzone/dropzone.js"));

			bundles.Add(new StyleBundle("~/bundles/dropzone-styles").Include(
						"~/Content/dropzone/basic.css",
						"~/Content/dropzone/dropzone.css"));


			BundleTable.EnableOptimizations = false;
		}
	}
}