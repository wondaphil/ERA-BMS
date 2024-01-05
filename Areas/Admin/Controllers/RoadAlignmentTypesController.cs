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
    public class RoadAlignmentTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/RoadAlignmentTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.RoadAlignmentTypes.ToList().Count();

            return View(db.RoadAlignmentTypes.ToList());
        }

        // GET: Admin/RoadAlignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoadAlignmentType roadAlignment = db.RoadAlignmentTypes.Find(id);
            if (roadAlignment == null)
            {
                return HttpNotFound();
            }
            return View(roadAlignment);
        }

        // GET: Admin/RoadAlignments/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/RoadAlignmentTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "RoadAlignmentTypeId,RoadAlignmentTypeName")] RoadAlignmentType roadAlignment)
        {
            if (ModelState.IsValid)
            {
                db.RoadAlignmentTypes.Add(roadAlignment);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = roadAlignment.RoadAlignmentTypeId });
            }

            return View(roadAlignment);
        }

        // GET: Admin/RoadAlignmentTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoadAlignmentType roadAlignment = db.RoadAlignmentTypes.Find(id);
            if (roadAlignment == null)
            {
                return HttpNotFound();
            }
            return View(roadAlignment);
        }

        // POST: Admin/RoadAlignmentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoadAlignmentTypeId,RoadAlignmentTypeName")] RoadAlignmentType roadAlignmentType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roadAlignmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = roadAlignmentType.RoadAlignmentTypeId });
            }
            return View(roadAlignmentType);
        }

        // GET: Admin/RoadAlignmentTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoadAlignmentType roadAlignment = db.RoadAlignmentTypes.Find(id);
            if (roadAlignment == null)
            {
                return HttpNotFound();
            }
            return View(roadAlignment);
        }

        // POST: Admin/RoadAlignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoadAlignmentType roadAlignment = db.RoadAlignmentTypes.Find(id);
            db.RoadAlignmentTypes.Remove(roadAlignment);
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
