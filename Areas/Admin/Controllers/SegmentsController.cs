using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BCMS.Models;

using ERA_BCMS.Areas.Admin.ViewModel;
 
namespace ERA_BCMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class SegmentsController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/Segments
        public ActionResult Index()
        {
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

        // GET: Admin/Segments
        public ActionResult _Index()
        {
            var SegmentList = db.Segments.Include(s => s.Section).ToList();
            return View(SegmentList);
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
            List<Segment> SegmentList = (from s in db.Segments
                                         where s.SectionId == sectionid
                                         select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(SegmentList);
        }

        // GET: Admin/Segments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Segment segment = db.Segments.Find(id);
            if (segment == null)
            {
                return HttpNotFound();
            }

            //Get district name that encompasses the current segment
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;

            return View(segment);
        }

        // GET: Admin/Segments/New
        public ActionResult New()
        {
            ViewBag.SectionId = new SelectList(db.Sections, "SectionId", "SectionName");
            //For a dropdownlist that has a list of all districts
            ViewBag.Districts = new SelectList(db.Districts, "DistrictId", "DistrictName");
            
            return View();
        }

        // POST: Admin/Segments/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "SegmentId,SegmentName,SectionId,Remark")] Segment segment)
        {
            if (ModelState.IsValid)
            {
                db.Segments.Add(segment);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = segment.SegmentId });
            }

            ViewBag.SectionId = new SelectList(db.Sections, "SectionId", "SectionName", segment.SectionId);
            return View(segment);
        }

        // GET: Admin/Segments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //Get the current segement with the given id
            Segment segment = db.Segments.Find(id);

            if (segment == null)
            {
                return HttpNotFound();
            }

            // Id of the district encompassing the current segment 
            int districtId = (int) db.Sections.Find(segment.SectionId).DistrictId;

            //Get list sections in that district which encompassed the current segment
            List<Section> sectionList = (from s in db.Sections
                                         where s.DistrictId == districtId
                                         select s).ToList();

            //For a dropdownlist that has a list of sections in that district and the current section selected
            ViewBag.Sections = new SelectList(sectionList, "SectionId", "SectionName", segment.SectionId);

            //For a dropdownlist that has a list of all districts
            ViewBag.Districts = new SelectList(db.Districts, "DistrictId", "DistrictName");

            //The DistricId of the district encompassing the current section
            ViewBag.DistrictId = districtId;
            
            return View(segment);
        }

        // POST: Admin/Segments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SegmentId,SegmentName,SectionId,Remark")] Segment segment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(segment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = segment.SegmentId });
            }
            ViewBag.SectionId = new SelectList(db.Sections, "SectionId", "SectionName", segment.SectionId);
            return View(segment);
        }

        //// GET: Admin/Segments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Segment segment = db.Segments.Find(id);
            if (segment == null)
            {
                return HttpNotFound();
            }
            
            //Get district name that encompasses the current segment
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;
            
            return View(segment);
        }

        // POST: Admin/Segments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Segment segment = db.Segments.Find(id);
            db.Segments.Remove(segment);
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
