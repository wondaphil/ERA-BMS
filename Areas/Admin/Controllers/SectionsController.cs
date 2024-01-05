using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BCMS.Models;

namespace ERA_BCMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class SectionsController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/Sections
        public ActionResult Index()
        {
            List<SelectListItem> districtList = db.Districts.Select(c => new SelectListItem
                                                                    {
                                                                        Text = c.DistrictName + " => " + c.DistrictId.ToString(),
                                                                        //Text = String.Format("{0}\t{1}", c.DistrictName, c.DistrictId.ToString()),
                                                                        Value = c.DistrictId.ToString()
                                                                    }
                                                            ).ToList();

            //Add the first unselected item
            districtList.Insert(0, new SelectListItem { Text = "--Select District--", Value = "0" });
            ViewBag.Districts = districtList;
            ViewBag.TotalRecords = districtList.Count();

            return View();
        }

        // GET: Admin/Sections
        public ActionResult _Index()
        {
            var sections = db.Sections.Include(s => s.District);
            return View(sections.ToList());
        }

        public ActionResult Index2()
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

        //public PartialViewResult GetDistrict(int districtid /* drop down value */)
        //{
        //    var model = db.Districts.Find(districtid); // This is for example put your code to fetch record.   

        //    return PartialView("MyPartialView", model);
        //}

        public ActionResult _GetSections(int districtid /* drop down value */)
        {
            List<Section> SectionList = (from s in db.Sections
                                         where s.DistrictId == districtid
                                         select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(SectionList);
        }

        // GET: Admin/Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Admin/Sections/New
        public ActionResult New()
        {
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName");
            return View();
        }

        // POST: Admin/Sections/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "SectionId,SectionName,DistrictId,Remark")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex) // catches all exceptions
                {
                    return View(ex.Message);
                }
                    
                return RedirectToAction("Details", new { id = section.SectionId });
            }

            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName", section.DistrictId);
            return View(section);
        }

        // GET: Admin/Sections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName", section.DistrictId);
            return View(section);
        }

        // POST: Admin/Sections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SectionId,SectionName,DistrictId,Remark")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = section.SectionId });
            }
            ViewBag.DistrictId = new SelectList(db.Districts, "DistrictId", "DistrictName", section.DistrictId);
            return View(section);
        }

        // GET: Admin/Sections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = db.Sections.Find(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Admin/Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Section section = db.Sections.Find(id);
            db.Sections.Remove(section);
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
