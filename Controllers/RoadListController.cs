using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class RoadListController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: RoadList
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _GetDistrictList()
        {
            return PartialView(db.Districts.ToList());
        }

        public ActionResult _GetSectionList()
        {
            return PartialView(db.Sections.ToList());
        }

        public ActionResult _GetSegmentList()
        {
            return PartialView(db.Segments.ToList());
        }
    }
}