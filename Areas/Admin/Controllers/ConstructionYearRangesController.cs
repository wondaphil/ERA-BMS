using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity; using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class ConstructionYearRangesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/ConstructionYearAndConstructionYearRanges
        public ActionResult Index(StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<ConstructionYear> constrYear = db.ConstructionYears.ToList();

            return View(constrYear);
        }

        [HttpPost]
        public ActionResult UpdateConstructionYearRange(ConstructionYear constructionYearRange)
        {
            ConstructionYear updatedConstructionYearRange = (from c in db.ConstructionYears
                                                             where c.CategoryId == constructionYearRange.CategoryId
                                                             select c).FirstOrDefault();

            updatedConstructionYearRange.ConstructionYears = constructionYearRange.ConstructionYears;
            updatedConstructionYearRange.FromYear = constructionYearRange.FromYear;
            updatedConstructionYearRange.ToYear = constructionYearRange.ToYear;
            updatedConstructionYearRange.Remark = constructionYearRange.Remark;
            
            db.SaveChanges();

            return new EmptyResult();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConstructionYearRange(int id)
        {
            ConstructionYear constrYear = db.ConstructionYears.Find(id);
            if (constrYear != null)
            {
                db.ConstructionYears.Remove(constrYear);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NewConstructionYearRangePartial()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult NewConstructionYearRangePartial([Bind(Include = "CategoryId,ConstructionYears,FromYear,ToYear,Remark")] ConstructionYear constrYear)
        {

            if (ModelState.IsValid)
            {
                db.ConstructionYears.Add(constrYear);
                
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index", "ConstructionYearRanges", new { messageId = StatusMessageId.AddDataSuccess });
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }
            }

            return View(constrYear);
        }

        public string LastConstructionYearRangeId()
        {
            int lastId = db.ConstructionYears.Select(s => s.CategoryId).Max();
            int newId = lastId + 1;

            // The returned result is a json string like [{"ConstructionYearId": "1"}]
            return "[{\"CategoryId\": " + newId + "}]";
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