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
    public class SoilTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/SoilTypes
        public ActionResult Index()
        {
            return View(db.SoilTypes.ToList());
        }

        // GET: Admin/SoilTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoilType SoilType = db.SoilTypes.Find(id);
            if (SoilType == null)
            {
                return HttpNotFound();
            }
            return View(SoilType);
        }

        // GET: Admin/SoilTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/SoilTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "SoilTypeId,SoilTypeName,Remark")] SoilType SoilType)
        {
            if (ModelState.IsValid)
            {
                db.SoilTypes.Add(SoilType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Index");
            }

            return View(SoilType);
        }

        // GET: Admin/SoilTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoilType SoilType = db.SoilTypes.Find(id);
            if (SoilType == null)
            {
                return HttpNotFound();
            }
            return View(SoilType);
        }

        // POST: Admin/SoilTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoilTypeId,SoilTypeName,Remark")] SoilType SoilType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(SoilType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(SoilType);
        }

        // GET: Admin/SoilTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SoilType SoilType = db.SoilTypes.Find(id);
            if (SoilType == null)
            {
                return HttpNotFound();
            }
            return View(SoilType);
        }

        // POST: Admin/SoilTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SoilType SoilType = db.SoilTypes.Find(id);
            db.SoilTypes.Remove(SoilType);
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
