using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SectionsController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/Sections
        public ActionResult Index(string id) // id is District Id
        {
            var SectionList = (from s in db.Sections
                              where s.DistrictId == id
                              select s).ToList();

            ViewBag.Districts = LocationModel.GetDistrictList();
            
            if (id != null && SectionList.Count != 0)
            {
                //For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = id;

                ViewBag.DistrictName = db.Districts.Find(id).DistrictName;
            }

            return View(SectionList);
        }

        public ActionResult _GetSectionsList(string districtid /* drop down value */)
        {
            List<Section> SectionList = (from s in db.Sections
                                         where s.DistrictId == districtid
                                         select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(SectionList);
        }

        // GET: Admin/Sections/Details/5
        public ActionResult Details(string id)
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

            ViewBag.DistrictId = section.DistrictId;

            return View(section);
        }

        //public string LastSectionId()
        //{
        //    string lastSectionId = db.Sections.Select(s => s.SectionId).Max().ToString();

        //    // SectionId is a three digit unique incrementing string in the format "123" or "430"
        //    // When a new Section is to be registered, it takes the next value to last Section id from the "Section" table
        //    // So convert it to int, increment it by 1 and convert it back to string

        //    string assignedSectionId = (Int32.Parse(lastSectionId) + 1).ToString();

        //    // The returned result is a json string like [{"SectionId": "12345"}]
        //    return "[{\"SectionId\": " + "\"" + assignedSectionId + "\"}]";
        //}

        public string LastSectionId()
        {
            //// SectionId is a GUID value
            //// When a new section is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string
            ///
            string assignedSectionId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"SectionId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"SectionId\": " + "\"" + assignedSectionId + "\"}]";
        }

        // GET: Sections/New
        [Authorize(Roles = "Admin")]
        public ActionResult New(string id) // id is District Id
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == null)
            {
                
            }
            else
            {
                District district = db.Districts.Find(id);
                
                //For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = district.DistrictId;

                ViewBag.DistrictName = district.DistrictName;
            }

            return View();
        }

        // POST: Admin/Sections/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult New([Bind(Include = "SectionId,SectionNo,SectionName,DistrictId,Remark")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Sections.Add(section);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 2 }); // id = 2 for duplicate section error
                }

                return RedirectToAction("Details", new { id = section.SectionId });
            }

            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = section.DistrictId;

            return View(section);
        }

        // GET: Admin/Sections/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
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
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = section.DistrictId;

            return View(section);
        }

        // POST: Admin/Sections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SectionId,SectionNo,SectionName,DistrictId,Remark")] Section section)
        {
            if (ModelState.IsValid)
            {
                db.Entry(section).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 2 }); // id = 2 for duplicate section error
                }

                return RedirectToAction("Details", new { id = section.SectionId });
            }
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = section.DistrictId; 

            return View(section);
        }

        // GET: Admin/Sections/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
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
            ViewBag.DistrictId = section.DistrictId;

            return View(section);
        }

        // POST: Admin/Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            Section section = db.Sections.Find(id);
            string districtId = section.DistrictId;
            
            db.Sections.Remove(section);
            try
            {
                db.SaveChanges();
            }
            catch (Exception) // catches all exceptions
            {
                return RedirectToAction("DeleteError", "ErrorHandler", new { id = 2 }); // id = 2 for section delete error
            }
            return RedirectToAction("Index", "Sections", new { id = districtId });
        }

        public string UpdateSectionNo(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return value;

            Section sec = db.Sections.Find(id);

            sec.SectionNo = value.ToString().Trim();
            db.SaveChanges();

            return value;
        }

        public string UpdateSectionName(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return value;

            Section sec = db.Sections.Find(id);

            sec.SectionName = value.ToString().Trim();
            db.SaveChanges();

            return value;
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
