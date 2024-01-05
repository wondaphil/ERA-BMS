using System.Web;
using System.Web.Optimization;

namespace ERA_BMS
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
                      "~/Content/erabms-bootstrap.css", // I added this for modified bootstrap of ERA-BMS 
                      "~/Content/dropdown-submenu.css")); // I added this for dropdown-submenu 
                      //"~/Content/leftmenu.css")); // I added this for leftmenu

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
                        "~/Content/buttons.dataTables.css",
                        "~/Content/select.dataTables.css",
                        "~/Content/editor.dataTables.css"));

            bundles.Add(new StyleBundle("~/Content/dropdownsubmenu").Include(
                        "~/Content/dropdown-submenu.css"));

            //bundles.Add(new StyleBundle("~/Content/leftmenu").Include(
            //            "~/Content/leftmenu.css"));

            //I added the following bundles for jQuery UI
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/core.css",
              "~/Content/themes/base/resizable.css",
              "~/Content/themes/base/selectable.css",
              "~/Content/themes/base/accordion.css",
              "~/Content/themes/base/autocomplete.css",
              "~/Content/themes/base/button.css",
              "~/Content/themes/base/dialog.css",
              "~/Content/themes/base/slider.css",
              "~/Content/themes/base/tabs.css",
              "~/Content/themes/base/datepicker.css",
              "~/Content/themes/base/progressbar.css",
              "~/Content/themes/base/theme.css"));

            //I added the following bundles for jEditable jQuery plugin
            bundles.Add(new ScriptBundle("~/bundles/jeditable").Include(
                        "~/Scripts/jquery.jeditable.js"));

            //I added the following bundles for Canvasjs
            bundles.Add(new ScriptBundle("~/bundles/canvasjs").Include(
                        "~/Scripts/canvasjs.min.js"));
            
            //I added the following bundles for chartjs plugin
            bundles.Add(new ScriptBundle("~/bundles/chartjs").Include(
                        "~/Scripts/chart.js"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                    "~/Content/themes/base/jquery-ui.css"));

            //I added the following bundles for Microsoft.jQuery.Unobtrusive.Ajax
            bundles.Add(new ScriptBundle("~/bundles/unobtrusive-ajax").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.js"));
        }
    }
}