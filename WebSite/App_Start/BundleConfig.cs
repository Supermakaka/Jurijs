using System.Web;
using System.Web.Optimization;

namespace WebSite
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/main-scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/validate-unobtrusive-bootstrap/jquery.validate.unobtrusive.bootstrap.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/plugins/handlebars/handlebars.js",
                        "~/Scripts/plugins/momentjs/moment.js",
                        "~/Scripts/plugins/debounce/debounce.js",
                        "~/Scripts/plugins/icheck/icheck.min.js",
                        "~/Scripts/plugins/pnotify/pnotify.custom.min.js",
                        "~/Scripts/plugins/daterangepicker/daterangepicker.js",
                        "~/Scripts/plugins/datatables/jquery.dataTables.js",
                        "~/Scripts/datatables/datatables-extended.js",
                        "~/Scripts/datatables/datatables-filters.js",
                        "~/Scripts/datatables/datatables-filters-daterangepicker.js",
                        "~/Scripts/datatables/datatables-filters-iCheck.js",
                        "~/Scripts/site/ajax-loader.js",
                        "~/Scripts/site/checkboxes.js",
                        "~/Scripts/site/modals.js",
                        "~/Scripts/site/site.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/admin-company-scripts").Include(
                "~/Scripts/pages/admin/company-list.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/admin-user-scripts").Include(
                "~/Scripts/pages/admin/user-list.js"
            ));

            bundles.Add(new StyleBundle("~/bundles/main-styles")
                .Include("~/Content/bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/plugins/spinkit/spinkit.css", new CssRewriteUrlTransform())
                .Include("~/Content/plugins/datatables/dataTables.bootstrap.css", new CssRewriteUrlTransform())
                .Include("~/Content/plugins/icheck/square/red.css", new CssRewriteUrlTransform())
                .Include("~/Content/plugins/pnotify/pnotify.custom.min.css", new CssRewriteUrlTransform())
                .Include("~/Content/plugins/daterangepicker/daterangepicker.css", new CssRewriteUrlTransform())
                .Include("~/Content/style.css", new CssRewriteUrlTransform())
            );

            bundles.Add(new StyleBundle("~/bundles/social-buttons")
                .Include("~/Content/plugins/bootstrap-social/font-awesome.css", new CssRewriteUrlTransform())
                .Include("~/Content/plugins/bootstrap-social/bootstrap-social.css", new CssRewriteUrlTransform())
            );
        }
    }
}