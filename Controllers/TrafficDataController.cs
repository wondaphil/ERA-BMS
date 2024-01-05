using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class TrafficDataController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: RoadList
        public ActionResult Index()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.Sections = new List<SelectListItem>();

            return View();
        }

        public ActionResult _GetTrafficData(string sectionid /* drop down value */)
        {
            List<Segment> segmentList = (from s in db.Segments
                                           where s.SectionId == sectionid
                                         select s).ToList();
            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(segmentList);
        }

        public string UpdateTrafficData(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return value;

            // If input value is not numeric (space, special character, etc), just keep the original value
            int result;
            if (!int.TryParse(value, out result))
                return origvalue;

            // If input value is a negative numeric value, just keep the original value
            if (result < 0)
                return origvalue;

            Segment seg = db.Segments.Find(id);

            seg.AverageDailyTraffic = int.Parse(value);
            db.SaveChanges();

            return value;
        }
    }
}