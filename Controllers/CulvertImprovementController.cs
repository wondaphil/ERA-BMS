using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;

using System.Data.Entity;
using System.Net;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class CulvertImprovementController : Controller
    {
        private BMSEntities db = new BMSEntities();
        private static DateTime globalInspectionDate = DateTime.Now;

        // GET: CulvertImprovement
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(string id = "")
        {
            Culvert culvert = db.Culverts.Find(id);
            var culImprov = db.CulvertImprovements.Where(d => d.CulvertId == id);

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == "" || culvert == null)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Culverts = new List<SelectListItem>(); // For an empty culverts dropdownlist

                return View();
            }
            else
            {
                // For a dropdownlist that has a list of culverts in the current segment and the current culvert selected
                ViewBag.Culverts = LocationModel.GetCulvertNameList(culvert.SegmentId);
                TempData["CulvertId"] = id;
                TempData["Culvert"] = db.Culverts.Find(id);

                // For a dropdownlist that has a list of segments in the current section and the current segment selected
                ViewBag.Segments = LocationModel.GetSegmentList(culvert.Segment.SectionId);
                ViewBag.SegmentId = culvert.Segment.SegmentId;

                // For a dropdownlist that has a list of sections in the current district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(culvert.Segment.Section.DistrictId);
                ViewBag.SectionId = culvert.Segment.SectionId;

                // For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = culvert.Segment.Section.DistrictId;

                // For displaying district, section, and segment names and culvert no. on reports
                ViewBag.DistrictName = culvert.Segment.Section.District.DistrictName;
                ViewBag.SectionName = culvert.Segment.Section.SectionName;
                ViewBag.SegmentName = culvert.Segment.SegmentName;

                TempData["ImrpovementTypeId"] = new SelectList(db.ImprovementTypes, "ImprovementTypeId", "ImprovementTypeName").ToList();
                TempData["ImprovementTypeList"] = (from c in db.ImprovementTypes
                                                   select new
                                                   {
                                                       Value = c.ImprovementTypeId,
                                                       Text = c.ImprovementTypeName
                                                   }).ToList();
            }
        
            return View(culImprov);
        }

        // GET
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult _GetCulvertImprovement([Optional] string culvertid /* drop down value */)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<CulvertImprovement> culImprovementList = db.CulvertImprovements.Where(s => s.CulvertId == culvertid).ToList();

            List<SelectListItem> imprTypeSelectList = db.ImprovementTypes.Select(c => new SelectListItem
                                                        { Text = c.ImprovementTypeName, Value = c.ImprovementTypeId.ToString() }).ToList();


            TempData["ImrpovementTypeId"] = imprTypeSelectList; // for improvement activity type dropdown list
            TempData["CulvertId"] = culvertid;

            TempData["ImprovementTypeList"] = (from c in db.ImprovementTypes
                                           select new
                                           {
                                               Value = c.ImprovementTypeId,
                                               Text = c.ImprovementTypeName
                                           }).ToList();

            TempData["Culvert"] = db.Culverts.Find(culvertid);

            return PartialView(culImprovementList);
        }

        [HttpPost]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public JsonResult InsertCulvertImprovement(CulvertImprovement culImprovement)
        {
            culImprovement.Id = Guid.NewGuid().ToString();
            db.CulvertImprovements.Add(culImprovement);
            db.SaveChanges();

            return Json(culImprovement);
        }

        [HttpPost]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult UpdateCulvertImprovement(CulvertImprovement culImprovement)
        {
            CulvertImprovement updatedculImprovement = (from c in db.CulvertImprovements
                                                       where c.Id == culImprovement.Id
                                                       select c).FirstOrDefault();

            updatedculImprovement.ImprovementDate = (culImprovement.ImprovementDate != null) ? culImprovement.ImprovementDate : globalInspectionDate; ;
            updatedculImprovement.ImprovementYear = culImprovement.ImprovementYear;
            updatedculImprovement.ImprovementTypeId = culImprovement.ImprovementTypeId;
            updatedculImprovement.ImprovementCost = culImprovement.ImprovementCost;
            updatedculImprovement.ImprovementAction = culImprovement.ImprovementAction;

            db.SaveChanges();

            return new EmptyResult();
        }

        //GET
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult GetLastCulvertImprovement(string culvertid)
        {
            var lastculImprovement = (from c in db.CulvertImprovements
                                      where c.CulvertId == culvertid
                                      select new
                                      {
                                          Id = c.Id,
                                          CulvertId = c.CulvertId,
                                      }).ToList().LastOrDefault();

            return Json(lastculImprovement, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult DeleteCulvertImprovement(string id)
        {
            CulvertImprovement culImprovement = db.CulvertImprovements.Find(id);
            if (culImprovement != null)
            {
                db.CulvertImprovements.Remove(culImprovement);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        public ActionResult _GetCulvertImprovementActionRecords(int improvementyear)
        {
            List<CulvertImprovement> culImproveList = db.CulvertImprovements.Where(s => s.ImprovementYear == improvementyear).ToList();

            ViewBag.ImprovementYear = improvementyear;
            return PartialView(culImproveList);
        }

        public ActionResult CulvertImprovementActionRecords()
        {
            var improvementyear = (from culImrov in db.CulvertImprovements
                                   select new
                                   {
                                       culImrov.ImprovementYear
                                   }).Distinct();

            List<SelectListItem> improvementYearList = improvementyear.OrderByDescending(s => s.ImprovementYear).Select(c => new SelectListItem
            {
                Text = c.ImprovementYear.ToString(),
                Value = c.ImprovementYear.ToString()
            }
                                                            ).ToList();

            //Add the first unselected item
            improvementYearList.Insert(0, new SelectListItem { Text = "--Select Improvement Year--", Value = "0" });

            ViewBag.ImprovementYearList = improvementYearList;
            return View();
        }

        public ActionResult PrintImprovement([Optional] string culvertid /* drop down value */)
        {
            ViewBag.Culvert = db.Culverts.Find(culvertid);

            return PartialView(db.CulvertImprovements.Where(s => s.CulvertId == culvertid));
        }

        public ActionResult _GetCulvertsForUpgradingAll(int inspectionyear)
        {
            List<CulvertsForUpgrading> culvertsforupgrading = new List<CulvertsForUpgrading>();

            // If inspection year "ALL YEARS" is selected
            if (inspectionyear == 0)
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2).ToList();
            else
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.InspectionYear == inspectionyear).ToList();
            
            ViewBag.InspectionYear = (inspectionyear == 0) ? "All Inspection Year" : inspectionyear.ToString();

            return PartialView(culvertsforupgrading);
        }

        public ActionResult _GetCulvertsForUpgradingByDistrict(string districtid, int inspectionyear /* drop down values */)
        {
            List<CulvertsForUpgrading> culvertsforupgrading = new List<CulvertsForUpgrading>();

            // If inspection year "ALL YEARS" is selected
            if (inspectionyear == 0)
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.DistrictId == districtid).ToList();
            else
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.DistrictId == districtid
                                                                && s.InspectionYear == inspectionyear).ToList();
            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();
            ViewBag.InspectionYear = (inspectionyear == 0) ? "All Inspection Year" : inspectionyear.ToString();

            return PartialView(culvertsforupgrading);
        }

        public ActionResult _GetCulvertsForUpgradingBySection(string sectionid, int inspectionyear /* drop down values */)
        {
            List<CulvertsForUpgrading> culvertsforupgrading = new List<CulvertsForUpgrading>();
            
            // If inspection year "ALL YEARS" is selected
            if (inspectionyear == 0)
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.SectionId == sectionid).ToList();
            else
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.SectionId == sectionid
                                                                && s.InspectionYear == inspectionyear).ToList();

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.InspectionYear = (inspectionyear == 0) ? "All Inspection Year" : inspectionyear.ToString();

            return PartialView(culvertsforupgrading);
        }

        public ActionResult _GetCulvertsForUpgradingBySegment(string segmentid, int inspectionyear /* drop down values */)
        {
            List<CulvertsForUpgrading> culvertsforupgrading = new List<CulvertsForUpgrading>();

            // If inspection year "ALL YEARS" is selected
            if (inspectionyear == 0)
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.SegmentId == segmentid).ToList();
            else
                culvertsforupgrading = db.CulvertsForUpgradings.Where(s => s.UrgencyIndex == 2 && s.SegmentId == segmentid
                                                                && s.InspectionYear == inspectionyear).ToList();
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = (inspectionyear == 0) ? "All Inspection Year" : inspectionyear.ToString();

            return PartialView(culvertsforupgrading);
        }

        // GET: ObservationAndRecommendations
        public ActionResult CulvertsForUpgrading()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts
            ViewBag.Districts = districtList;

            List<SelectListItem> inspectionYearList = (from culDmgInsp in db.ResultInspCulverts
                                                       orderby culDmgInsp.InspectionDate
                                                       select new
                                                       {
                                                           InspYear = culDmgInsp.InspectionDate.ToString().Substring(0, 4) // take the year part from the date
                                                       }).Distinct().OrderByDescending(s => s.InspYear).Select(c => new SelectListItem
                                                       {
                                                           Text = c.InspYear.ToString(),
                                                           Value = c.InspYear.ToString()
                                                       }
                                                            ).ToList();

            //List<SelectListItem> inspectionYearList = new List<SelectListItem>();

            //List<MajorInspectionYear> inspYears = db.MajorInspectionYears.ToList();

            //// Iterate through major inspection year
            //foreach (var year in inspYears)
            //{
            //    string txt = year.InspectionYear.ToString(); // e.g. "2016"
            //    string val = year.StartYear + " - " + year.EndYear; // e.g. "2014 - 2016"
            //    inspectionYearList.Insert(0, new SelectListItem { Text = txt, Value = val });
            //}


            //var lastInspectionYear = Int32.Parse(inspectionYearList.Select(s => s.Value).Max()); // last inpection year is the max in the year list
            inspectionYearList.Insert(0, new SelectListItem { Text = "ALL", Value = "0" });
            ViewBag.InspectionYearList = inspectionYearList;

            return View();
        }
    }
}