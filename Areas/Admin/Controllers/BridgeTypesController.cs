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
    public class BridgeTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/BridgeTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.BridgeTypes.ToList().Count();

            return View(db.BridgeTypes.ToList());
        }

        public ActionResult Temp()
        {
            return View();
        }

        // GET: Admin/BridgeTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BridgeType bridgeType = db.BridgeTypes.Find(id);
            if (bridgeType == null)
            {
                return HttpNotFound();
            }
            return View(bridgeType);
        }

        // GET: Admin/BridgeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/BridgeTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BridgeTypeId,BridgeTypeName,BridgeTypeShortCode,Remark")] BridgeType bridgeType)
        {
            if (ModelState.IsValid)
            {
                db.BridgeTypes.Add(bridgeType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = bridgeType.BridgeTypeId });
            }

            return View(bridgeType);
        }

        // GET: Admin/BridgeTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BridgeType bridgeType = db.BridgeTypes.Find(id);
            if (bridgeType == null)
            {
                return HttpNotFound();
            }
            return View(bridgeType);
        }

        // POST: Admin/BridgeTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BridgeTypeId,BridgeTypeName,BridgeTypeShortCode,Remark")] BridgeType bridgeType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bridgeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = bridgeType.BridgeTypeId });
            }
            return View(bridgeType);
        }

        // GET: Admin/BridgeTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BridgeType bridgeType = db.BridgeTypes.Find(id);
            if (bridgeType == null)
            {
                return HttpNotFound();
            }
            return View(bridgeType);
        }

        // POST: Admin/BridgeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BridgeType bridgeType = db.BridgeTypes.Find(id);
            db.BridgeTypes.Remove(bridgeType);
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
