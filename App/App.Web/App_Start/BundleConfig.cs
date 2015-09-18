using System.Web;
using System.Web.Optimization;

namespace App.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.IgnoreList.Clear();

            bundles.Add(new StyleBundle("~/bundles/main").Include(
                        "~/Content/luxen/css/bootstrap.min.css",
                        "~/Content/luxen/css/flexslider.css",
                        "~/Content/luxen/css/prettyPhoto.css",
                        "~/Content/luxen/css/datepicker.css",
                        "~/Content/luxen/css/selectordie.css",
                        "~/Content/luxen/css/2035-reset.css",
                        "~/Content/luxen/css/slicknav.css",
                        "~/Content/luxen/css/mainApp.css",
                        "~/Content/luxen/css/2035.responsive.css",
                        "~/Content/font-awesome.css",
                        "~/Content/main-extensions.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/administration").Include(
                        "~/Content/luxen/css/bootstrap.min.css",
                        "~/Content/font-awesome.css",
                        "~/Content/administration.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/mainScripts").Include(
                        "~/Content/luxen/js/vendor/jquery-1.11.1.min.js",
                        "~/Content/luxen/js/vendor/bootstrap.min.js",
                        "~/Content/luxen/js/retina-1.1.0.min.js",
                        "~/Content/luxen/js/jquery.flexslider-min.js",
                        "~/Content/luxen/js/superfish.pack.1.4.1.js",
                        "~/Content/luxen/js/jquery.prettyPhoto.js",
                        "~/Content/luxen/js/bootstrap-datepicker.js",
                        "~/Content/luxen/js/selectordie.min.js",
                        "~/Content/luxen/js/jquery.slicknav.min.js",
                        "~/Content/luxen/js/jquery.parallax-1.1.3.js",
                        "~/Content/luxen/js/main.js"
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
