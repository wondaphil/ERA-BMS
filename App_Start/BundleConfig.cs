using System.Web;
using System.Web.Optimization;

namespace ERA_BCMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/dropdown-submenu.csss")); // I added this for dropdown-submenu

            //I added the following bundles for dataTables
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                      "~/Scripts/jquery.dataTables.js",
                      "~/Scripts/dataTables.buttons.js",
                      "~/Scripts/buttons.flash.js",
                      "~/Scripts/jszip.js",
                      "~/Scripts/pdfmake.js",
                      "~/Scripts/vfs_fonts.js",
                      "~/Scripts/buttons.html5.js",
                      "~/Scripts/buttons.print.js"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                        "~/Content/jquery.dataTables.css",
                        "~/Content/buttons.dataTables.css"));

            bundles.Add(new StyleBundle("~/Content/dropdownsubmenu").Include(
                        "~/Content/dropdown-submenu.css"));

        }
    }
}
