using Turbo.MVC.Base3.Commons;
using Turbo.MVC.Base3.Commons.Filter;
using System.Web;
using System.Web.Mvc;

// namespace Turbo.MVC.Base3
namespace WDACC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new CustomAuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomActionFilter());
            //filters.Add(new CustomActionFilter());
        }
    }
}
