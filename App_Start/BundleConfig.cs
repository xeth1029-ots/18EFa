using System.Web;
using System.Web.Optimization;

// namespace Turbo.MVC.Base3
namespace WDACC 
{
    public class BundleConfig
    {
        // 如需「搭配」的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // 將 EnableOptimizations 設為 false 以進行偵錯。如需詳細資訊，
            // 請造訪 http://go.microsoft.com/fwlink/?LinkId=301862
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery-migrate-3.3.0.js",
                      "~/Scripts/jquery-confirm.min.js",
                      "~/Scripts/jquery.blockUI.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-treeview.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/globaljs").Include(
                      "~/Scripts/global.js",
                      "~/Scripts/print.js"));

            bundles.Add(new StyleBundle("~/Content/jquery").Include(
                    "~/Content/jquery-confirm.min.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                      "~/Content/bootstrap3-3-6.min.css",
                      "~/Content/bootstrap-treeview.css",
                      "~/Content/font-awesome-4.7.0.min.css"));

            bundles.Add(new StyleBundle("~/css/base")
                .Include("~/css/base.css", new CssRewriteUrlTransform())
            );

            // 內外網環境: 套不同的 CSS 樣式
            //if ("2".Equals(Models.ConfigModel.NetID))
            //{
            //    // 外網
            //    bundles.Add(new StyleBundle("~/css/base")
            //        .Include("~/css/base2.css", new CssRewriteUrlTransform())
            //    );
            //}
            //else
            //{
            //    // 內網
            //    bundles.Add(new StyleBundle("~/css/base")
            //        .Include("~/css/base.css", new CssRewriteUrlTransform())
            //    );
            //}

        }
    }
}
