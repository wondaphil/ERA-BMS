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

namespace ERA_BMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DistrictsController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/Districts
        public ActionResult Index()
        {
            return View(db.Districts.ToList());
        }

        // GET: Admin/Districts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        //public string LastDistrictId()
        //{
        //    string lastDistrictId = db.Districts.Select(s => s.DistrictId).Max().ToString();

        //    // DistrictId is a three digit unique incrementing string in the format "123" or "430"
        //    // When a new district is to be registered, it takes the next value to last district id from the "District" table
        //    // So convert it to int, increment it by 1 and convert it back to string

        //    string assignedDistrictId = (Int32.Parse(lastDistrictId) + 1).ToString();

        //    // The returned result is a json string like [{"DistrictId": "12345"}]
        //    return "[{\"DistrictId\": " + "\"" + assignedDistrictId + "\"}]";
        //}

        public string LastDistrictId()
        {
            //// DistrictId is a GUID value
            //// When a new district is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string
            ///
            string assignedDistrictId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"DistrictId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"DistrictId\": " + "\"" + assignedDistrictId + "\"}]";
        }

        // GET: Districts/NEW
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/Districts/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult New([Bind(Include = "DistrictId,DistrictNo,DistrictName,Remark")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Districts.Add(district);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 1 }); // id = 1 for duplicate district error
                }
                return RedirectToAction("Details", new { id = district.DistrictId });
            }

            return View(district);
        }

        // GET: Admin/Districts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Admin/Districts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "DistrictId,DistrictNo,DistrictName,Remark")] District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 1 }); // id = 1 for duplicate district error
                }

                return RedirectToAction("Details", new { id = district.DistrictId });
            }
            return View(district);
        }

        // GET: Admin/Districts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = db.Districts.Find(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Admin/Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            District district = db.Districts.Find(id);
            db.Districts.Remove(district);
            try
            {
                db.SaveChanges();
            }
            catch (Exception) // catches all exceptions
            {
                return RedirectToAction("DeleteError", "ErrorHandler", new { id = 1 }); // id = 1 for district delete error
            }
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
