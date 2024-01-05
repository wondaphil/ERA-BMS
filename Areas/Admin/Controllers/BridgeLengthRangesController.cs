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
    public class BridgeLengthRangesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/BridgeLengthRanges
        public ActionResult Index(StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<BridgeLength> brgLength = db.BridgeLengths.ToList();

            return View(brgLength);
        }

        [HttpPost]
        public ActionResult UpdateBridgeLengthRange(BridgeLength bridgeLengthRange)
        {
            BridgeLength updatedBridgeLengthRange = (from c in db.BridgeLengths
                                                     where c.BridgeLengthId == bridgeLengthRange.BridgeLengthId
                                                     select c).FirstOrDefault();

            updatedBridgeLengthRange.BridgeLengthName = bridgeLengthRange.BridgeLengthName;
            updatedBridgeLengthRange.FromLength = bridgeLengthRange.FromLength;
            updatedBridgeLengthRange.ToLength = bridgeLengthRange.ToLength;
            updatedBridgeLengthRange.Remark = bridgeLengthRange.Remark;

            db.SaveChanges();

            return new EmptyResult();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBridgeLengthRange(int id)
        {
            BridgeLength brgLength = db.BridgeLengths.Find(id);
            if (brgLength != null)
            {
                db.BridgeLengths.Remove(brgLength);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult NewBridgeLengthRangePartial()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult NewBridgeLengthRangePartial([Bind(Include = "BridgeLengthId,BridgeLengthName,FromLength,ToLength,Remark")] BridgeLength brgLength)
        {

            if (ModelState.IsValid)
            {
                db.BridgeLengths.Add(brgLength);

                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index", "BridgeLengthRanges", new { messageId = StatusMessageId.AddDataSuccess });
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }
            }

            return View(brgLength);
        }

        public string LastBridgeLengthRangeId()
        {
            int lastId = db.BridgeLengths.Select(s => s.BridgeLengthId).Max();
            int newId = lastId + 1;

            // The returned result is a json string like [{"BridgeLengthId": "1"}]
            return "[{\"BridgeLengthId\": " + newId + "}]";
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