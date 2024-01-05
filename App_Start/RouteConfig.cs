using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ERA_BMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //e.g. Bridges/Edit/A1-1-001 = Bridges/Edit?id=A1-1-001
            //     DamageInspMajor/Index/A1-1-001 = DamageInspMajor/Index?id=A2-10-006
            //     DamageInspMajor/Index/A1-1-001/2013 = DamageInspMajor/Index?id=A2-10-006&year=2006
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{val}", 
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, val = UrlParameter.Optional }
                //namespaces: new string[] { "MyApp.Controllers" }
            );

            //routes.MapRoute(
            //    name: "MajorInspection",
            //    url: "{controller}/{action}/{id}/{year}", 
            //    defaults: new { controller = "DamageInspMajor", action = "Index", id = UrlParameter.Optional, year = UrlParameter.Optional }
            //    //namespaces: new string[] { "MyApp.Controllers" }
            //);

            //routes.MapRoute(
            //    name: "Inventory",
            //    url: "BridgeInventory/{id}", 
            //    defaults: new { controller = "BridgeInventory", action = "Index", id = UrlParameter.Optional }
            //);

            //routes.MapRoute(
            //    name: "Inventory",
            //    url: "{id}",
            //    defaults: new { controller = "BridgeInventory", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
