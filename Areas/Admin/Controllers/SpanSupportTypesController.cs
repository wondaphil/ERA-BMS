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
    public class SpanSupportTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/SpanSupportTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.SpanSupportTypes.ToList().Count();

            return View(db.SpanSupportTypes.ToList());
        }

        // GET: Admin/SpanSupportTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpanSupportType spanSupportType = db.SpanSupportTypes.Find(id);
            if (spanSupportType == null)
            {
                return HttpNotFound();
            }
            return View(spanSupportType);
        }

        // GET: Admin/SpanSupportTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/SpanSupportTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "SpanSupportTypeId,SpanSupportTypeName,Remark")] SpanSupportType spanSupportType)
        {
            if (ModelState.IsValid)
            {
                db.SpanSupportTypes.Add(spanSupportType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = spanSupportType.SpanSupportTypeId });
            }

            return View(spanSupportType);
        }

        // GET: Admin/SpanSupportTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpanSupportType spanSupportType = db.SpanSupportTypes.Find(id);
            if (spanSupportType == null)
            {
                return HttpNotFound();
            }
            return View(spanSupportType);
        }

        // POST: Admin/SpanSupportTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SpanSupportTypeId,SpanSupportTypeName,Remark")] SpanSupportType spanSupportType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spanSupportType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = spanSupportType.SpanSupportTypeId });
            }
            return View(spanSupportType);
        }

        // GET: Admin/SpanSupportTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SpanSupportType spanSupportType = db.SpanSupportTypes.Find(id);
            if (spanSupportType == null)
            {
                return HttpNotFound();
            }
            return View(spanSupportType);
        }

        // POST: Admin/SpanSupportTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SpanSupportType spanSupportType = db.SpanSupportTypes.Find(id);
            db.SpanSupportTypes.Remove(spanSupportType);
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
