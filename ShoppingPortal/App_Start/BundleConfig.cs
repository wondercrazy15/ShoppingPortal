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
                      "~/Content/TemplateCSS/font-awesome.css",
                      "~/Content/TemplateCSS/nprogress.css",
                      "~/Content/TemplateCSS/bootstrap-progressbar-3.3.4.min.css",
                      "~/Content/TemplateCSS/daterangepicker.css",
                       "~/Content/TemplateCSS/sweetalert.min.css",
                      "~/Content/TemplateCSS/switchery.min.css",
                      "~/Content/TemplateCSS/custom.css",
                      "~/Content/TemplateCSS/common.css"
                      //"~/Content/TemplateCSS/green.css",
                      //"~/Content/TemplateCSS/_reboot.scss"
                      ));

            bundles.Add(new StyleBundle("~/Content/DatatableCSS").Include(
                   
                     "~/Content/TemplateCSS/dataTables.bootstrap.min.css",
                     "~/Content/TemplateCSS/buttons.bootstrap.min.css",
                     "~/Content/TemplateCSS/fixedHeader.bootstrap.min.css",
                     "~/Content/TemplateCSS/responsive.bootstrap.min.css",
                     "~/Content/TemplateCSS/scroller.bootstrap.min.css"
                   ));



            bundles.Add(new ScriptBundle("~/bundles/Templatejquery").Include(
                        "~/Scripts/TemplateScript/bootstrap.bundle.js",
                        "~/Scripts/TemplateScript/fastclick.js",
                        "~/Scripts/TemplateScript/nprogress.js",
                         "~/Scripts/TemplateScript/sweetalert.min.js",
                         "~/Scripts/TemplateScript/switchery.min.js",
                         "~/Scripts/TemplateScript/custom.js",
                         "~/Scripts/TemplateScript/common.js"                      
                        //"~/Common/JS/CommonAjaxCall.js"
                        //"~/Scripts/TemplateScript/Chart.min.js",
                        //"~/Scripts/TemplateScript/jquery.sparkline.min.js",
                        //"~/Scripts/TemplateScript/morris.min.js",
                        //"~/Scripts/TemplateScript/gauge.min.js",
                        //"~/Scripts/TemplateScript/bootstrap-progressbar.min.js",
                        //"~/Scripts/TemplateScript/skycons.js",
                        //"~/Scripts/TemplateScript/jquery.flot.js",
                        //"~/Scripts/TemplateScript/jquery.flot.pie.js",
                        //"~/Scripts/TemplateScript/jquery.flot.time.js",
                        //"~/Scripts/TemplateScript/jquery.flot.stack.js",
                        //"~/Scripts/TemplateScript/jquery.flot.resize.js"                                            
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Templatebootstrap").Include(
                      "~/Scripts/bootstrap.js"));


            bundles.Add(new ScriptBundle("~/bundles/Datatable").Include(
                         "~/Scripts/TemplateScript/jquery.dataTables.min.js",
                         "~/Scripts/TemplateScript/dataTables.bootstrap.min.js",
                            "~/Scripts/TemplateScript/dataTables.buttons.min.js",
                            "~/Scripts/TemplateScript/buttons.bootstrap.min.js",
                            "~/Scripts/TemplateScript/buttons.flash.min.js",
                            "~/Scripts/TemplateScript/buttons.html5.min.js",
                            "~/Scripts/TemplateScript/buttons.print.min.js",
                            "~/Scripts/TemplateScript/dataTables.fixedHeader.min.js",
                            "~/Scripts/TemplateScript/dataTables.keyTable.min.js",
                            "~/Scripts/TemplateScript/dataTables.responsive.min.js",
                            "~/Scripts/TemplateScript/responsive.bootstrap.js",
                            "~/Scripts/TemplateScript/dataTables.scroller.min.js",
                            "~/Scripts/TemplateScript/jszip.min.js",
                            "~/Scripts/TemplateScript/pdfmake.min.js",
                            "~/Scripts/TemplateScript/vfs_fonts.js"
                     ));


        }
    }
}
