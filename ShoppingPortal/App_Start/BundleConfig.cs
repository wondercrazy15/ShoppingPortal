using System.Web;
using System.Web.Optimization;

namespace ShoppingPortal
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
                      "~/Content/site.css"));

            //Custom template bundles

            bundles.Add(new StyleBundle("~/Content/TemplateCSS").Include(
                      "~/Content/TemplateCSS/bootstrap.min.css",
                      "~/Content/TemplateCSS/font-awesome.min.css",
                      "~/Content/TemplateCSS/nprogress.css",
                      "~/Content/TemplateCSS/bootstrap-progressbar-3.3.4.min.css",
                      "~/Content/TemplateCSS/daterangepicker.css",
                      "~/Content/TemplateCSS/custom.min.css"));
            

            bundles.Add(new ScriptBundle("~/bundles/Templatejquery").Include(
                        "~/Scripts/TemplateScript/jquery.min.js",
                        "~/Scripts/TemplateScript/bootstrap.bundle.minjquery.min.js",
                        "~/Scripts/TemplateScript/fastclick.js",
                        "~/Scripts/TemplateScript/nprogress.js",
                        "~/Scripts/TemplateScript/Chart.min.js",
                        "~/Scripts/TemplateScript/jquery.sparkline.min.js",
                        "~/Scripts/TemplateScript/morris.min.js",
                        "~/Scripts/TemplateScript/gauge.min.js",
                        "~/Scripts/TemplateScript/bootstrap-progressbar.min.js",
                        "~/Scripts/TemplateScript/skycons.js",
                        "~/Scripts/TemplateScript/jquery.flot.js",
                        "~/Scripts/TemplateScript/jquery.flot.pie.js",
                        "~/Scripts/TemplateScript/jquery.flot.time.js",
                        "~/Scripts/TemplateScript/jquery.flot.stack.js",
                        "~/Scripts/TemplateScript/jquery.flot.resize.js"                                            
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Templatebootstrap").Include(
                      "~/Scripts/bootstrap.js"));

        }
    }
}
