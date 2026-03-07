using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

// namespace Turbo.MVC.Base3
namespace WDACC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("home");

            //routes.MapRoute(
            //    name: "Login",
            //    url: "Login/Login",
            //    defaults: new { controller = "Login", action = "Index" },
            //    constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            //);

            //routes.MapRoute(
            //    name: "Role",
            //    url: "Role",
            //    defaults: new { controller = "Login", action = "Role" }
            //);

            //routes.MapRoute(
            //   name: "report",
            //   url: "report/{action}/{id}/{subid}",
            //   defaults: new { controller = "report", action = "Index", id = UrlParameter.Optional },
            //   new[] { "Turbo.Controllers.ReportController" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{subid}",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional,
                    subid = UrlParameter.Optional
                }
            );

            //routes.MapRoute(
            //    name: "REST",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "REST", action = "Index", id = UrlParameter.Optional }
            //);

        }
    }
}
