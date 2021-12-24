using System.Web;
using System.Web.Optimization;

namespace LabAccounting
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

            bundles.Add(new ScriptBundle("~/bundles/main_page")
                .Include("~/Scripts/Models/TemplateModel.js")
                .Include("~/Scripts/models/AutofillMenu.js")
                .Include("~/Scripts/models/Validator.js")
                .Include("~/Scripts/models/MainPage.js")
                );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/mainpage").Include(
                      "~/Content/mainpage.css"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/litepicker.js"));
        }
    }
}
