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
    public class DeckSlabTypesController : Controller
    {
        private BMSEntities db = new BMSEntities();

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

        // GET: Admin/DeckSlabTypes/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admin/DeckSlabTypes/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "DeckSlabTypeId,DeckSlabTypeName,Remark")] DeckSlabType deckSlabType)
        {
            if (ModelState.IsValid)
            {
                db.DeckSlabTypes.Add(deckSlabType);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }

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
