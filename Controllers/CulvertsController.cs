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
    public class CulvertsController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Culverts
        public ActionResult Index()
        {
            var CulvertList = db.Culverts.Include(b => b.Segment);
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

            return View(CulvertList);
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

        public ActionResult _GetCulverts(int segmentid /* drop down value */)
        {
            List<Culvert> CulvertList = (from s in db.Culverts
                                         where s.SegmentId == segmentid
                                       select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(CulvertList);
            //return this.Json(new SelectList(CulvertList.ToArray(), "CulvertId", "CulvertName"), JsonRequestBehavior.AllowGet);
        }

        // GET: Culverts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culvert culvert = db.Culverts.Find(id);
            if (culvert == null)
            {
                return HttpNotFound();
            }
            
            Segment segment = db.Segments.Find(culvert.SegmentId);

            //Get district name that encompasses the current bridge
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;

            ViewBag.SectionName = segment.Section.SectionName;

            return View(culvert);
        }

        // GET: Culverts/New
        public ActionResult New()
        {
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName");
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName");
            ViewBag.Districts = new SelectList(db.Districts, "DistrictId", "DistrictName");
            
            return View();
        }

        // POST: Culverts/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "CulvertId,CulvertName,SegmentId,RouteId,KMFromAddis,XCoord,YCoord")] Culvert culvert)
        {
            if (ModelState.IsValid)
            {
                db.Culverts.Add(culvert);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = culvert.CulvertId });
            }

            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName", culvert.RouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", culvert.SegmentId);
            return View(culvert);
        }

        // GET: Culverts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culvert culvert = db.Culverts.Find(id);
            if (culvert == null)
            {
                return HttpNotFound();
            }
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName", culvert.RouteId);

            Segment segment = db.Segments.Find(culvert.SegmentId);

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

            ViewBag.Segments = new SelectList(segmentList, "SegmentId", "SegmentName", culvert.SegmentId);


            // The SectionId of the section encompassing the current segment 
            ViewBag.SectionId = sectionId;

            //For a dropdownlist that has a list of sections in that district and the current section selected
            ViewBag.Sections = new SelectList(sectionList, "SectionId", "SectionName");

            //For a dropdownlist that has a list of all districts
            ViewBag.Districts = new SelectList(db.Districts, "DistrictId", "DistrictName");

            //The DistricId of the district encompassing the current section
            ViewBag.DistrictId = districtId;

            return View(culvert);
        }

        // POST: Culverts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CulvertId,CulvertName,SegmentId,RouteId,KMFromAddis,XCoord,YCoord")] Culvert culvert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(culvert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = culvert.CulvertId });
            }
            ViewBag.RouteId = new SelectList(db.Routes, "RouteId", "RouteName", culvert.RouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", culvert.SegmentId);
            return View(culvert);
        }

        // GET: Culverts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culvert culvert = db.Culverts.Find(id);
            if (culvert == null)
            {
                return HttpNotFound();
            }
            return View(culvert);
        }

        // POST: Culverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Culvert culvert = db.Culverts.Find(id);
            db.Culverts.Remove(culvert);
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
