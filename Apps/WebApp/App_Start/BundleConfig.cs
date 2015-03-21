using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            
            
            bundles.Add(Vendor);

            bundles.Add(Css);
        }

        private static Bundle Vendor
        {
            get
            {
                return new ScriptBundle("~/bundles/vendor").Include(
                    "~/Scripts/jquery-{version}.js",    // DOM Manipulation (required by bootstrap)
                    "~/Scripts/bootstrap.js",           // Standard CSS Template                    
                    "~/Scripts/moment.js",              // Date/time display and manipulation                    
                    "~/Scripts/underscore.js"          // JavaScript helper library
                );
//                .IncludeDirectory("~/Scripts", "*.js", true);
            }
        }
        private static Bundle Css
        {
            get
            {
                return new StyleBundle("~/Content/css")
                    .Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css",
                        "~/Content/Blog.css")
                    .IncludeDirectory("~/Content", "*.css", false);
            }
        }
    }
}
