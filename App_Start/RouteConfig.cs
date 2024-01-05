using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERA_BCMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            ////I added this
            //routes.MapRoute(
            //    name: "Admin",
            //    url: "Admin/{controller}/{action}/{id}",
            //    defaults: new { action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "Bridge",
            //    url: "{controller}/{id}",
            //    defaults: new { controller = "Bridge", action = "Details", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "District",
            //    url: "Admin/{controller}/{id}",
            //    defaults: new { controller = "Districts", action = "Details", id = UrlParameter.Optional }
            //);



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                //namespaces: new string[] { "MyApp.Controllers" }
            );
        }
    }
}
