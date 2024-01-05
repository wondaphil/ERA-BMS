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
    public class CulvertTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/CulvertTypes
        public ActionResult Index()
        {
            return View(db.CulvertTypes.ToList());
        }

        // GET: Admin/CulvertTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulvertType culvertType = db.CulvertTypes.Find(id);
            if (culvertType == null)
            {
                return HttpNotFound();
            }
            return View(culvertType);
        }

        // GET: Admin/CulvertTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/CulvertTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "CulvertTypeId,CulvertTypeName,Remark")] CulvertType culvertType)
        {
            if (ModelState.IsValid)
            {
                db.CulvertTypes.Add(culvertType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = culvertType.CulvertTypeId });
            }

            return View(culvertType);
        }

        // GET: Admin/CulvertTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulvertType culvertType = db.CulvertTypes.Find(id);
            if (culvertType == null)
            {
                return HttpNotFound();
            }
            return View(culvertType);
        }

        // POST: Admin/CulvertTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CulvertTypeId,CulvertTypeName,Remark")] CulvertType culvertType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(culvertType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = culvertType.CulvertTypeId });
            }
            return View(culvertType);
        }

        // GET: Admin/CulvertTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulvertType culvertType = db.CulvertTypes.Find(id);
            if (culvertType == null)
            {
                return HttpNotFound();
            }
            return View(culvertType);
        }

        // POST: Admin/CulvertTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CulvertType culvertType = db.CulvertTypes.Find(id);
            db.CulvertTypes.Remove(culvertType);
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
