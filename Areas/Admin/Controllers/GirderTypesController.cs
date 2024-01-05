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
    public class GirderTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/GirderTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.GirderTypes.ToList().Count();
            
            return View(db.GirderTypes.ToList());
        }

        // GET: Admin/GirderTypes/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GirderType girderType = db.GirderTypes.Find(id);
            if (girderType == null)
            {
                return HttpNotFound();
            }
            return View(girderType);
        }

        // GET: Admin/GirderTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/GirderTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GirderTypeId,GirderTypeName,Remark")] GirderType girderType)
        {
            if (ModelState.IsValid)
            {
                db.GirderTypes.Add(girderType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = girderType.GirderTypeId });
            }

            return View(girderType);
        }

        // GET: Admin/GirderTypes/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GirderType girderType = db.GirderTypes.Find(id);
            if (girderType == null)
            {
                return HttpNotFound();
            }
            return View(girderType);
        }

        // POST: Admin/GirderTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GirderTypeId,GirderTypeName,Remark")] GirderType girderType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(girderType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = girderType.GirderTypeId });
            }
            return View(girderType);
        }

        // GET: Admin/GirderTypes/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GirderType girderType = db.GirderTypes.Find(id);
            if (girderType == null)
            {
                return HttpNotFound();
            }
            return View(girderType);
        }

        // POST: Admin/GirderTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            GirderType girderType = db.GirderTypes.Find(id);
            db.GirderTypes.Remove(girderType);
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
