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
    public class SurfaceTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/SurfaceTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.SurfaceTypes.ToList().Count();

            return View(db.SurfaceTypes.ToList());
        }

        // GET: Admin/SurfaceTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurfaceType surfaceType = db.SurfaceTypes.Find(id);
            if (surfaceType == null)
            {
                return HttpNotFound();
            }
            return View(surfaceType);
        }

        // GET: Admin/SurfaceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SurfaceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SurfaceTypeId,SurfaceTypeName,Remark")] SurfaceType surfaceType)
        {
            if (ModelState.IsValid)
            {
                db.SurfaceTypes.Add(surfaceType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = surfaceType.SurfaceTypeId });
            }

            return View(surfaceType);
        }

        // GET: Admin/SurfaceTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurfaceType surfaceType = db.SurfaceTypes.Find(id);
            if (surfaceType == null)
            {
                return HttpNotFound();
            }
            return View(surfaceType);
        }

        // POST: Admin/SurfaceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SurfaceTypeId,SurfaceTypeName,Remark")] SurfaceType surfaceType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(surfaceType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = surfaceType.SurfaceTypeId });
            }
            return View(surfaceType);
        }

        // GET: Admin/SurfaceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SurfaceType surfaceType = db.SurfaceTypes.Find(id);
            if (surfaceType == null)
            {
                return HttpNotFound();
            }
            return View(surfaceType);
        }

        // POST: Admin/SurfaceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SurfaceType surfaceType = db.SurfaceTypes.Find(id);
            db.SurfaceTypes.Remove(surfaceType);
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
