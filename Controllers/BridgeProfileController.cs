using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using System.Net;
using ERA_BCMS.Models;
using System.Runtime.InteropServices;

namespace ERA_BCMS.Controllers
{
    public class BridgeProfileController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: BridgeProfile
        public ActionResult Index()
        {
            //var BridgeList = db.Bridges.Include(b => b.GirderType).Include(b => b.Route).Include(b => b.Segment);
            List<SelectListItem> districtList = db.Districts.Select(c => new SelectListItem
                                            {
                                                Text = c.DistrictName,
                                                Value = c.DistrictId.ToString()
                                            }
                                                            ).ToList();

            //Add the first unselected item
            districtList.Insert(0, new SelectListItem { Text = "--Select District--", Value = "0" });
            ViewBag.Districts = districtList;
            ViewBag.TotalRecords = districtList.Count();

            return View();
        }

        public ActionResult _GetSections(int districtid /* drop down value */)
        {
            List<Section> SectionList = db.Sections.Where(s => s.DistrictId == districtid).ToList();

            //return SelectList item in Json format
            //e.g. [{"Disabled":false,"Group":null,"Selected":false,"Text":"Sodo","Value":"100"},
            //      {"Disabled":false,"Group":null,"Selected":false,"Text":"Konso","Value":"111"}]
            return this.Json(new SelectList(SectionList.ToArray(), "SectionId", "SectionName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetSegments(int sectionid /* drop down value */)
        {
            List<Segment> SegmentList = db.Segments.Where(s => s.SectionId == sectionid).ToList();

            //return SelectList item in Json format
            //e.g. [{"Disabled":false,"Group":null,"Selected":false,"Text":"Alaba-Sodo","Value":"1001"},
            //      {"Disabled":false,"Group":null,"Selected":false,"Text":"Areka-Sodo","Value":"1002"}]
            return this.Json(new SelectList(SegmentList.ToArray(), "SegmentId", "SegmentName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetBridges(int segmentid /* drop down value */)
        {
            List<Bridge> BridgeList = db.Bridges.Where(s => s.SegmentId == segmentid).ToList();

            //return SelectList item in Json format
            //e.g. [{"Disabled":false,"Group":null,"Selected":false,"Text":"Yae","Value":"B42-1-001"},
            //      {"Disabled":false,"Group":null,"Selected":false,"Text":"Gerba","Value":"B42-1-005"}]
            return this.Json(new SelectList(BridgeList.ToArray(), "BridgeId", "BridgeName"), JsonRequestBehavior.AllowGet);
        }

        //GET
        public ActionResult BridgeDetail([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Find(bridgeid);
            if (bridge == null)
            {
                return HttpNotFound();
            }

            Segment segment = db.Segments.Find(bridge.SegmentId);

            //Get district name that encompasses the current bridge
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;

            ViewBag.SectionName = segment.Section.SectionName;

            return PartialView(bridge);
        }

        //GET
        public ActionResult GeneralInfo([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BridgeGeneralInfo bridgeGenInfo = db.BridgeGeneralInfoes.Find(bridgeid);
            if (bridgeGenInfo == null)
            {
                return HttpNotFound();
            }

            return PartialView(bridgeGenInfo);
        }

        //GET
        public ActionResult SuperStructure([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuperStructure superStructure = db.SuperStructures.Find(bridgeid);
            if (superStructure == null)
            {
                return HttpNotFound();
            }

            return PartialView(superStructure);
        }

        //GET
        public ActionResult Abutment([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abutment abutment = db.Abutments.Find(bridgeid);
            if (abutment == null)
            {
                return HttpNotFound();
            }

            return PartialView(abutment);
        }

        //GET
        public ActionResult Pier([Optional] string bridgeid /* drop down value */)
        {
            List<Pier> pierList = (from s in db.Piers
                                   where s.BridgePierId == bridgeid
                                   select s).ToList();

            return PartialView(pierList);
        }

        //GET
        public ActionResult Component([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Component component = db.Components.Find(bridgeid);
            if (component == null)
            {
                return HttpNotFound();
            }

            return PartialView(component);
        }


       
    }
}