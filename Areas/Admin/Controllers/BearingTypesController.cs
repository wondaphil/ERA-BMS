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
    public class BearingTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/BearingTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.BearingTypes.ToList().Count();

            return View(db.BearingTypes.ToList());
        }

        // GET: Admin/BearingTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BearingType bearingType = db.BearingTypes.Find(id);
            if (bearingType == null)
            {
                return HttpNotFound();
            }
            return View(bearingType);
        }

        // GET: Admin/BearingTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/BearingTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BearingTypeId,BearingTypeName")] BearingType bearingType)
        {
            if (ModelState.IsValid)
            {
                db.BearingTypes.Add(bearingType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = bearingType.BearingTypeId });
            }

            return View(bearingType);
        }

        // GET: Admin/BearingTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BearingType bearingType = db.BearingTypes.Find(id);
            if (bearingType == null)
            {
                return HttpNotFound();
            }
            return View(bearingType);
        }

        // POST: Admin/BearingTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BearingTypeId,BearingTypeName")] BearingType bearingType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bearingType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = bearingType.BearingTypeId });
            }
            return View(bearingType);
        }

        // GET: Admin/BearingTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BearingType bearingType = db.BearingTypes.Find(id);
            if (bearingType == null)
            {
                return HttpNotFound();
            }
            return View(bearingType);
        }

        // POST: Admin/BearingTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BearingType bearingType = db.BearingTypes.Find(id);
            db.BearingTypes.Remove(bearingType);
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
