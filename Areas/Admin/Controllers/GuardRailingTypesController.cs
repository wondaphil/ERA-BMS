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
    public class GuardRailingTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/GuardRailingTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.GuardRailingTypes.ToList().Count();

            return View(db.GuardRailingTypes.ToList());
        }

        // GET: Admin/GuardRailingTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuardRailingType guardRailingType = db.GuardRailingTypes.Find(id);
            if (guardRailingType == null)
            {
                return HttpNotFound();
            }
            return View(guardRailingType);
        }

        // GET: Admin/GuardRailingTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/GuardRailingTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "GuardRailingTypeId,GuardRailingTypeName,Remark")] GuardRailingType guardRailingType)
        {
            if (ModelState.IsValid)
            {
                db.GuardRailingTypes.Add(guardRailingType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = guardRailingType.GuardRailingTypeId });
            }

            return View(guardRailingType);
        }

        // GET: Admin/GuardRailingTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuardRailingType guardRailingType = db.GuardRailingTypes.Find(id);
            if (guardRailingType == null)
            {
                return HttpNotFound();
            }
            return View(guardRailingType);
        }

        // POST: Admin/GuardRailingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GuardRailingTypeId,GuardRailingTypeName,Remark")] GuardRailingType guardRailingType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guardRailingType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = guardRailingType.GuardRailingTypeId });
            }
            return View(guardRailingType);
        }

        // GET: Admin/GuardRailingTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuardRailingType guardRailingType = db.GuardRailingTypes.Find(id);
            if (guardRailingType == null)
            {
                return HttpNotFound();
            }
            return View(guardRailingType);
        }

        // POST: Admin/GuardRailingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuardRailingType guardRailingType = db.GuardRailingTypes.Find(id);
            db.GuardRailingTypes.Remove(guardRailingType);
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
