using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Areas.Admin.ViewModels;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;
using Newtonsoft.Json;

namespace ERA_BMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class MajorInspectionYearController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: MajorInspectionYear
        public ActionResult Index(StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);
            
            List<MajorInspectionYear> majInspYears = db.MajorInspectionYears.ToList();
                              
            return View(majInspYears);
        }

        public ActionResult UpdateMajorInspectionYear(MajorInspectionYear inspYears)
        {
            MajorInspectionYear majorInspYear = db.MajorInspectionYears.Find(inspYears.Id);

            majorInspYear.InspectionYear = inspYears.InspectionYear;
            majorInspYear.StartYear = inspYears.StartYear;
            majorInspYear.EndYear = inspYears.EndYear;
            majorInspYear.NoOfRegisteredBridges = inspYears.NoOfRegisteredBridges;
            majorInspYear.NoOfInspectedBridges = inspYears.NoOfInspectedBridges;
            majorInspYear.NoOfRegisteredCulverts = inspYears.NoOfRegisteredCulverts;
            majorInspYear.NofInspectedCulverts = inspYears.NofInspectedCulverts;
            majorInspYear.Remark = inspYears.Remark;
            
            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult DeleteMajorInspectionYear(int id)
        {
            MajorInspectionYear majorInspYear = db.MajorInspectionYears.Find(id);
            if (majorInspYear != null)
            {
                db.MajorInspectionYears.Remove(majorInspYear);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        public ActionResult NewMajorInspectionYearPartial()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewMajorInspectionYearPartial([Bind(Include = "Id,InspectionYear,StartYear,EndYear,NoOfRegisteredBridges,NoOfRegisteredCulverts,NoOfInspectedBridges,NofInspectedCulverts,Remark")] MajorInspectionYear inspYears)
        {
            if (ModelState.IsValid)
            {
                db.MajorInspectionYears.Add(inspYears);
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index", "MajorInspectionYear", new { messageId = StatusMessageId.AddDataSuccess });
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { area = "" });
                }
            }

            return View(inspYears);
        }
    }
}