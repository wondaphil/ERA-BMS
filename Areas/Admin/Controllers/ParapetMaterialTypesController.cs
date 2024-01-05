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
    public class ParapetMaterialTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/ParapetMaterialTypes
        public ActionResult Index()
        {
            return View(db.ParapetMaterialTypes.ToList());
        }

        // GET: Admin/ParapetMaterialTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParapetMaterialType parapetMaterialType = db.ParapetMaterialTypes.Find(id);
            if (parapetMaterialType == null)
            {
                return HttpNotFound();
            }
            return View(parapetMaterialType);
        }

        // GET: Admin/ParapetMaterialTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/ParapetMaterialTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "MaterialTypeId,MaterialTypeName,Remark")] ParapetMaterialType parapetMaterialType)
        {
            if (ModelState.IsValid)
            {
                db.ParapetMaterialTypes.Add(parapetMaterialType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = parapetMaterialType.MaterialTypeId });
            }

            return View(parapetMaterialType);
        }

        // GET: Admin/ParapetMaterialTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParapetMaterialType parapetMaterialType = db.ParapetMaterialTypes.Find(id);
            if (parapetMaterialType == null)
            {
                return HttpNotFound();
            }
            return View(parapetMaterialType);
        }

        // POST: Admin/ParapetMaterialTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaterialTypeId,MaterialTypeName,Remark")] ParapetMaterialType parapetMaterialType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parapetMaterialType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = parapetMaterialType.MaterialTypeId });
            }
            return View(parapetMaterialType);
        }

        // GET: Admin/ParapetMaterialTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParapetMaterialType parapetMaterialType = db.ParapetMaterialTypes.Find(id);
            if (parapetMaterialType == null)
            {
                return HttpNotFound();
            }
            return View(parapetMaterialType);
        }

        // POST: Admin/ParapetMaterialTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ParapetMaterialType parapetMaterialType = db.ParapetMaterialTypes.Find(id);
            db.ParapetMaterialTypes.Remove(parapetMaterialType);
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
