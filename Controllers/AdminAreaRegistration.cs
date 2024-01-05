using System.Web.Mvc;

namespace ERA_BCMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            //context.MapRoute(
            //    "Districts",
            //    "{controller}/{action}/{id}",
            //    new { id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "DistrictDetails",
            //    "{controller}/{id}",
            //    new { controller = "Districts", action = "Details", id = UrlParameter.Optional }
            //);


        }

        //public override void RegisterArea(AreaRegistrationContext context)
        //{
        //    context.MapRoute(
        //        "Admin_default",
        //        "Admin/{controller}/{action}/{id}",
        //        new { action = "Index", id = UrlParameter.Optional },
        //        new { controller = "AbutmentTypes|BearingTypes|BridgeTypes" },
        //        new[] { "MyApp.Areas.Admin.Controllers" }
        //    );
        //}
    }
}