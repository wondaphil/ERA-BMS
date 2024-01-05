using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;


namespace ERA_BMS.Controllers
{
    // Location Drop Down List (District => Section => Road Segment => Bridge)
    public class LocationsDDLController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: LocationsDDL
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _GetSections(string districtid /* drop down value */)
        {
            // SelectList item in Json format
            // e.g. [{"Text":"100 - Sodo", "Value":"100", "Disabled":false, "Group":null, "Selected":false},
            //      {"Text":"111 - Konso", "Value":"111", "Disabled":false, "Group":null, "Selected":false}]
            return this.Json(new SelectList(LocationModel.GetSectionList(districtid).ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetSegments(string sectionid /* drop down value */)
        {
            // SelectList item in Json format
            // e.g. [{"Text":"1001 - Alaba-Sodo", "Value":"1001", "Disabled":false, "Group":null, "Selected":false},
            //      {"Text":"1002 - Areka-Sodo", "Value":"1002", "Disabled":false, "Group":null, "Selected":false}]
            return this.Json(new SelectList(LocationModel.GetSegmentList(sectionid).ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetBridges(string segmentid /* drop down value */)
        {
            // SelectList item in Json format
            // e.g. [{"Text":"Yae", "Value":"B42-1-001", "Disabled":false, "Group":null, "Selected":false},
            //      {"Text":"Gerba", "Value":"B42-1-005", "Disabled":false, "Group":null, "Selected":false}]
            return this.Json(new SelectList(LocationModel.GetBridgeNameList(segmentid).ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetCulverts(string segmentid /* drop down value */)
        {
            // SelectList item in Json format
            // e.g. [{"Text":"Yae", "Value":"B42-1-001", "Disabled":false, "Group":null, "Selected":false},
            //      {"Text":"Gerba", "Value":"B42-1-005", "Disabled":false, "Group":null, "Selected":false}]
            return this.Json(new SelectList(LocationModel.GetCulvertNameList(segmentid).ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetSubRoutes(string mainrouteid /* drop down value */)
        {
            // SelectList item in Json format
            // e.g. [{"Text":"Alaba-Sodo", "Value":"1001", "Disabled":false, "Group":null, "Selected":false},
            //      {"Text":"Areka-Sodo", "Value":"1002", "Disabled":false, "Group":null, "Selected":false}]
            //return this.Json(new SelectList(SubRouteList.ToArray(), "SubRouteId", "SubRouteName"), JsonRequestBehavior.AllowGet);
            return this.Json(new SelectList(LocationModel.GetSubRouteNameList(mainrouteid).ToArray(), "Value", "Text"), JsonRequestBehavior.AllowGet);
        }
    }
}