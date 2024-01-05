﻿using System;
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
    public class EndWallTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/culEndWallTypes
        public ActionResult Index()
        {
            return View(db.culEndWallTypes.ToList());
        }

        // GET: Admin/culEndWallTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            culEndWallType culEndWallType = db.culEndWallTypes.Find(id);
            if (culEndWallType == null)
            {
                return HttpNotFound();
            }
            return View(culEndWallType);
        }

        // GET: Admin/culEndWallTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/culEndWallTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "EndWallTypeId,EndWallTypeName,Remark")] culEndWallType culEndWallType)
        {
            if (ModelState.IsValid)
            {
                db.culEndWallTypes.Add(culEndWallType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

                return RedirectToAction("Details", new { id = culEndWallType.EndWallTypeId });
            }

            return View(culEndWallType);
        }

        // GET: Admin/culEndWallTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            culEndWallType culEndWallType = db.culEndWallTypes.Find(id);
            if (culEndWallType == null)
            {
                return HttpNotFound();
            }
            return View(culEndWallType);
        }

        // POST: Admin/culEndWallTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EndWallTypeId,EndWallTypeName,Remark")] culEndWallType culEndWallType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(culEndWallType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = culEndWallType.EndWallTypeId });
            }
            return View(culEndWallType);
        }

        // GET: Admin/culEndWallTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            culEndWallType culEndWallType = db.culEndWallTypes.Find(id);
            if (culEndWallType == null)
            {
                return HttpNotFound();
            }
            return View(culEndWallType);
        }

        // POST: Admin/culEndWallTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            culEndWallType culEndWallType = db.culEndWallTypes.Find(id);
            db.culEndWallTypes.Remove(culEndWallType);
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