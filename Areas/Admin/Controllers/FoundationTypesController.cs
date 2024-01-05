using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity; using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;

namespace ERA_BMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class FoundationTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/FoundationTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.FoundationTypes.ToList().Count();

            return View(db.FoundationTypes.ToList());
        }

        // GET: Admin/FoundationTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoundationType foundationType = db.FoundationTypes.Find(id);
            if (foundationType == null)
            {
                return HttpNotFound();
            }
            return View(foundationType);
        }

        // GET: Admin/FoundationTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/FoundationTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "FoundationTypeId,FoundationTypeName,Remark")] FoundationType foundationType)
        {
            if (ModelState.IsValid)
            {
                db.FoundationTypes.Add(foundationType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = foundationType.FoundationTypeId });
            }

            return View(foundationType);
        }

        // GET: Admin/FoundationTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoundationType foundationType = db.FoundationTypes.Find(id);
            if (foundationType == null)
            {
                return HttpNotFound();
            }
            return View(foundationType);
        }

        // POST: Admin/FoundationTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoundationTypeId,FoundationTypeName,Remark")] FoundationType foundationType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foundationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = foundationType.FoundationTypeId });
            }
            return View(foundationType);
        }

        // GET: Admin/FoundationTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoundationType foundationType = db.FoundationTypes.Find(id);
            if (foundationType == null)
            {
                return HttpNotFound();
            }
            return View(foundationType);
        }

        // POST: Admin/FoundationTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoundationType foundationType = db.FoundationTypes.Find(id);
            db.FoundationTypes.Remove(foundationType);
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
