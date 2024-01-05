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
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class AbutmentTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/AbutmentTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.AbutmentTypes.ToList().Count();

            return View(db.AbutmentTypes.ToList());
        }

        // GET: Admin/AbutmentTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbutmentType abutmentType = db.AbutmentTypes.Find(id);
            if (abutmentType == null)
            {
                return HttpNotFound();
            }
            return View(abutmentType);
        }

        // GET: Admin/AbutmentTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/AbutmentTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "AbutmentTypeId,AbutmentTypeName,Remark")] AbutmentType abutmentType)
        {
            if (ModelState.IsValid)
            {
                db.AbutmentTypes.Add(abutmentType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = abutmentType.AbutmentTypeId });
            }

            return View(abutmentType);
        }

        // GET: Admin/AbutmentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbutmentType abutmentType = db.AbutmentTypes.Find(id);
            if (abutmentType == null)
            {
                return HttpNotFound();
            }
            return View(abutmentType);
        }

        // POST: Admin/AbutmentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AbutmentTypeId,AbutmentTypeName,Remark")] AbutmentType abutmentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(abutmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = abutmentType.AbutmentTypeId });
            }
            return View(abutmentType);
        }

        // GET: Admin/AbutmentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbutmentType abutmentType = db.AbutmentTypes.Find(id);
            if (abutmentType == null)
            {
                return HttpNotFound();
            }
            return View(abutmentType);
        }

        // POST: Admin/AbutmentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AbutmentType abutmentType = db.AbutmentTypes.Find(id);
            db.AbutmentTypes.Remove(abutmentType);
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
