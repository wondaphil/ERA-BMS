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
    public class ExpansionJointTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/ExpansionJointTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.ExpansionJointTypes.ToList().Count();

            return View(db.ExpansionJointTypes.ToList());
        }

        // GET: Admin/ExpansionJointTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpansionJointType expansionJointType = db.ExpansionJointTypes.Find(id);
            if (expansionJointType == null)
            {
                return HttpNotFound();
            }
            return View(expansionJointType);
        }

        // GET: Admin/ExpansionJointTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExpansionJointTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpansionJointTypeId,ExpansionJointTypeName,Remark")] ExpansionJointType expansionJointType)
        {
            if (ModelState.IsValid)
            {
                db.ExpansionJointTypes.Add(expansionJointType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = expansionJointType.ExpansionJointTypeId });
            }

            return View(expansionJointType);
        }

        // GET: Admin/ExpansionJointTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpansionJointType expansionJointType = db.ExpansionJointTypes.Find(id);
            if (expansionJointType == null)
            {
                return HttpNotFound();
            }
            return View(expansionJointType);
        }

        // POST: Admin/ExpansionJointTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpansionJointTypeId,ExpansionJointTypeName,Remark")] ExpansionJointType expansionJointType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expansionJointType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = expansionJointType.ExpansionJointTypeId });
            }
            return View(expansionJointType);
        }

        // GET: Admin/ExpansionJointTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpansionJointType expansionJointType = db.ExpansionJointTypes.Find(id);
            if (expansionJointType == null)
            {
                return HttpNotFound();
            }
            return View(expansionJointType);
        }

        // POST: Admin/ExpansionJointTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpansionJointType expansionJointType = db.ExpansionJointTypes.Find(id);
            db.ExpansionJointTypes.Remove(expansionJointType);
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
