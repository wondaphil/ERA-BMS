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
    public class DeckSlabTypesController : Controller
    {
        private BCMSEntities db = new BCMSEntities();

        // GET: Admin/DeckSlabTypes
        public ActionResult Index()
        {
            ViewBag.TotalRecords = db.DeckSlabTypes.ToList().Count();

            return View(db.DeckSlabTypes.ToList());
        }

        // GET: Admin/DeckSlabTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeckSlabType deckSlabType = db.DeckSlabTypes.Find(id);
            if (deckSlabType == null)
            {
                return HttpNotFound();
            }
            return View(deckSlabType);
        }

        // GET: Admin/DeckSlabTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DeckSlabTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeckSlabTypeId,DeckSlabTypeName,Remark")] DeckSlabType deckSlabType)
        {
            if (ModelState.IsValid)
            {
                db.DeckSlabTypes.Add(deckSlabType);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = deckSlabType.DeckSlabTypeId });
            }

            return View(deckSlabType);
        }

        // GET: Admin/DeckSlabTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeckSlabType deckSlabType = db.DeckSlabTypes.Find(id);
            if (deckSlabType == null)
            {
                return HttpNotFound();
            }
            return View(deckSlabType);
        }

        // POST: Admin/DeckSlabTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeckSlabTypeId,DeckSlabTypeName,Remark")] DeckSlabType deckSlabType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deckSlabType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = deckSlabType.DeckSlabTypeId });
            }
            return View(deckSlabType);
        }

        // GET: Admin/DeckSlabTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeckSlabType deckSlabType = db.DeckSlabTypes.Find(id);
            if (deckSlabType == null)
            {
                return HttpNotFound();
            }
            return View(deckSlabType);
        }

        // POST: Admin/DeckSlabTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeckSlabType deckSlabType = db.DeckSlabTypes.Find(id);
            db.DeckSlabTypes.Remove(deckSlabType);
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
