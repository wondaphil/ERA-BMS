using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BCMS.Models;

namespace ERA_BCMS.Controllers
{
    public class BridgesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Bridges
        public ActionResult Index()
        {
            var BridgeList = db.Bridges.Include(b => b.GirderType).Include(b => b.Route).Include(b => b.Segment);
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
            
            return View(BridgeList);
        }

        // GET: Bridges
        public ActionResult Index2()
        {
            var BridgeList = db.Bridges.Include(b => b.GirderType).Include(b => b.Route).Include(b => b.Segment);
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

            return View(BridgeList);
        }

        public ActionResult AllBridges()
        {
            var BridgeList = db.Bridges.Include(b => b.GirderType).Include(b => b.Route).Include(b => b.Segment);
            
            return View(BridgeList);
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
            //List<Segment> SegmentList = (from s in db.Segments
            //                             where s.SectionId == sectionid
            //                             select s).ToList();

            ////return a partial view so as to return clean html (avoid headers, footers, menu etc)
            //return PartialView(SegmentList);

            List<Segment> SegmentList = db.Segments.Where(s => s.SectionId == sectionid).ToList();

            //return SelectList item in Json format
            //e.g. [{"Disabled":false,"Group":null,"Selected":false,"Text":"Alaba-Sodo","Value":"1001"},
            //      {"Disabled":false,"Group":null,"Selected":false,"Text":"Areka-Sodo","Value":"1002"}]
            return this.Json(new SelectList(SegmentList.ToArray(), "SegmentId", "SegmentName"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _GetBridges(int segmentid /* drop down value */)
        {
            List<Bridge> BridgeList = (from s in db.Bridges
                                         where s.SegmentId == segmentid
                                         select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(BridgeList);
            //return this.Json(new SelectList(BridgeList.ToArray(), "BridgeId", "BridgeName"), JsonRequestBehavior.AllowGet);
        }

        // GET: Bridges/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Find(id);
            if (bridge == null)
            {
                return HttpNotFound();
            }

            Segment segment = db.Segments.Find(bridge.SegmentId);
            
            //Get district name that encompasses the current bridge
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;

            ViewBag.SectionName = segment.Section.SectionName;
            
            return View(bridge);
        }

        // GET: Bridges/New
        public ActionResult New()
        {
            ViewBag.GirderTypeId = new SelectList(db.GirderTypes, "GirderTypeId", "GirderTypeName");
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName");
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName");
            ViewBag.Districts = new SelectList(db.Districts, "DistrictId", "DistrictName");

            return View();
        }

        // POST: Bridges/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "BridgeId,BridgeName,BridgeSerNo,SegmentId,RouteId,KMFromAddis,XCoord,YCoord,GirderTypeId,BudgetYear,BudgetYearComment")] Bridge bridge)
        {
            if (ModelState.IsValid)
            {
                db.Bridges.Add(bridge);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = bridge.BridgeId });
            }

            ViewBag.GirderTypeId = new SelectList(db.GirderTypes, "GirderTypeId", "GirderTypeName", bridge.GirderTypeId);
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName", bridge.RouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", bridge.SegmentId);
            return View(bridge);
        }

        // GET: Bridges/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Find(id);
            if (bridge == null)
            {
                return HttpNotFound();
            }
            ViewBag.GirderTypeId = new SelectList(db.GirderTypes, "GirderTypeId", "GirderTypeName", bridge.GirderTypeId);
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName", bridge.RouteId);
            
            Segment segment = db.Segments.Find(bridge.SegmentId);

            // Id of the district encompassing the current segment 
            int districtId = (int)db.Sections.Find(segment.SectionId).DistrictId;

            //Get list sections in that district which encompasses the current segment
            List<Section> sectionList = (from s in db.Sections
                                         where s.DistrictId == districtId
                                         select s).ToList();
            int sectionId = (int)segment.SectionId;

            List<Segment> segmentList = (from s in db.Segments
                                         where s.SectionId == sectionId
                                         select s).ToList();

            ViewBag.Segments = new SelectList(segmentList, "SegmentId", "SegmentName", bridge.SegmentId);


            // The SectionId of the section encompassing the current segment 
            ViewBag.SectionId = sectionId;

            //For a dropdownlist that has a list of sections in that district and the current section selected
            ViewBag.Sections = new SelectList(sectionList, "SectionId", "SectionName");

            //For a dropdownlist that has a list of all districts
            ViewBag.Districts = new SelectList(db.Districts, "DistrictId", "DistrictName");

            //The DistricId of the district encompassing the current section
            ViewBag.DistrictId = districtId;
            
            return View(bridge);
        }

        // POST: Bridges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BridgeId,BridgeName,BridgeSerNo,SegmentId,RouteId,KMFromAddis,XCoord,YCoord,GirderTypeId,BudgetYear,BudgetYearComment")] Bridge bridge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bridge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = bridge.BridgeId });
            }
            ViewBag.GirderTypeId = new SelectList(db.GirderTypes, "GirderTypeId", "GirderTypeName", bridge.GirderTypeId);
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName", bridge.RouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", bridge.SegmentId);
            return View(bridge);
        }

        // GET: Bridges/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Find(id);
            if (bridge == null)
            {
                return HttpNotFound();
            }
            return View(bridge);
        }

        // POST: Bridges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Bridge bridge = db.Bridges.Find(id);
            db.Bridges.Remove(bridge);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
