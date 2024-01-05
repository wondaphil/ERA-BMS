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
    public class PierTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/PierTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.PierTypes.ToList().Count();

            return View(db.PierTypes.ToList());
        }

        // GET: Admin/PierTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PierType pierType = db.PierTypes.Find(id);
            if (pierType == null)
            {
                return HttpNotFound();
            }
            return View(pierType);
        }

        // GET: Admin/PierTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PierTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PierTypeId,PierTypeName,Remark")] PierType pierType)
        {
            if (ModelState.IsValid)
            {
                db.PierTypes.Add(pierType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = pierType.PierTypeId });
            }

            return View(pierType);
        }

        // GET: Admin/PierTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PierType pierType = db.PierTypes.Find(id);
            if (pierType == null)
            {
                return HttpNotFound();
            }
            return View(pierType);
        }

        // POST: Admin/PierTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PierTypeId,PierTypeName,Remark")] PierType pierType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pierType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = pierType.PierTypeId });
            }
            return View(pierType);
        }

        // GET: Admin/PierTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PierType pierType = db.PierTypes.Find(id);
            if (pierType == null)
            {
                return HttpNotFound();
            }
            return View(pierType);
        }

        // POST: Admin/PierTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PierType pierType = db.PierTypes.Find(id);
            db.PierTypes.Remove(pierType);
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
