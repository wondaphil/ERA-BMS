using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class CulvertInspectionReportsController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: CulvertInspectionReports
        public ActionResult Index()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts
            ViewBag.Districts = districtList;

            List<SelectListItem> reportTypeList = new List<SelectListItem>();

            //Add items
            reportTypeList.Insert(0, new SelectListItem { Text = "Culvert Service Condition", Value = "1" });
            reportTypeList.Insert(1, new SelectListItem { Text = "Culvert Condition by Inspection Year", Value = "2" });
            reportTypeList.Insert(2, new SelectListItem { Text = "Inspector's Recommendation & Comments", Value = "3" });
            reportTypeList.Insert(3, new SelectListItem { Text = "Summary of Service Condition & Repair Cost", Value = "4" });
            reportTypeList.Insert(4, new SelectListItem { Text = "Culvert Condition & Bill of Quantity (BOQ)", Value = "5" });

            ViewBag.ReportTypes = reportTypeList;

            //List<SelectListItem> inspectionYearList = (from culDmgInsp in db.ResultInspCulverts
            //                                           orderby culDmgInsp.InspectionDate
            //                                           select new
            //                                           {
            //                                               InspYear = culDmgInsp.InspectionDate.ToString().Substring(0, 4) // take the year part from the date
            //                                           }).Distinct().OrderByDescending(s => s.InspYear).Select(c => new SelectListItem
            //                                           {
            //                                               Text = c.InspYear.ToString(),
            //                                               Value = c.InspYear.ToString()
            //                                           }
            //                                                ).ToList();

            List<SelectListItem> inspectionYearSelectList = new List<SelectListItem>();
            List<MajorInspectionYear> inspYears = db.MajorInspectionYears.ToList();

            // Iterate through major inspection year
            foreach (var year in inspYears)
            {
                string txt = year.InspectionYear.ToString();  // e.g. "2016"
                string val = year.InspectionYear.ToString();  // e.g. "2016"
                inspectionYearSelectList.Insert(0, new SelectListItem { Text = txt, Value = val });
            }

            ViewBag.InspectionYearList = inspectionYearSelectList;

            return View();
        }

        public ActionResult _GetCulvertConditionAndBOQ(string culvertid /* drop down values */)
        {
            var segmentid = db.Culverts.Find(culvertid).SegmentId;

            ViewBag.CulvertNo = db.Culverts.Find(culvertid).CulvertNo.ToString();
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.CulvertStructure = CulStrInspectionDetails(culvertid);
            ViewBag.DamageRateCulvert = CulvertStructureDamageRateAndCost();

            ViewBag.HydraulicAndChannelDamage = db.culDamageInspHydraulics.Find(culvertid);

            ObservationAndRecommendation obs = db.ObservationAndRecommendations.Find(culvertid);
            ViewBag.Recommendation = obs.InspectorRecommendation;
            ViewBag.InterventionType = obs.MaintenanceUrgency.UrgencyName;

            return PartialView();
        }

        // 0-10 = Good, 10-15 = Fair, 15-100 = Bad, 
        public string CalculateDamageCondition(int? urgencyid)
        {
            string condition;
            switch (urgencyid)
            {
                case 0:
                    condition = "Good";
                    break;

                case 1:
                    condition = "Fair";
                    break;

                case 2:
                    condition = "Bad";
                    break;

                default:
                    condition = "";
                    break;
            }
            

            return condition;
        }

        public List<Culvert> GetCulvertListByDistrict(string districtid)
        {
            List<Culvert> culvertList = (from br in db.Culverts
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       where sec.DistrictId == districtid
                                       select br).ToList();

            return culvertList;
        }

        public List<Culvert> GetCulvertListBySection(string sectionid)
        {
            List<Culvert> culvertList = (from br in db.Culverts
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       where seg.SectionId == sectionid
                                       select br).ToList();

            return culvertList;
        }

        public List<Culvert> GetCulvertListBySegment(string segmentid)
        {
            List<Culvert> culvertList = (from br in db.Culverts
                                       where br.SegmentId == segmentid
                                       select br).ToList();

            return culvertList;
        }

        public ActionResult _GetCulvertDamageConditionByDistrict(string districtid /* drop down values */)
        {
            // Get all culverts in the current district
            List<Culvert> culvertList = GetCulvertListByDistrict(districtid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();
            List<ObservationAndRecommendation> recommList = db.ObservationAndRecommendations.ToList();
            
            var result = from dmg in culvertDmgResultList
                         join cul in culvertList on dmg.CulvertId equals cul.CulvertId into table1
                         from cul in table1.ToList()
                         join rec in recommList on cul.CulvertId equals rec.CulvertId into table2
                         from rec in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = dmg,
                             condition = CalculateDamageCondition(rec.UrgencyIndex),
                             recommendation = rec
                         };

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();
            
            return PartialView(result);
        }

        public ActionResult _GetCulvertDamageConditionBySection(string sectionid /* drop down values */)
        {
            // Get all culverts in the current section
            List<Culvert> culvertList = GetCulvertListBySection(sectionid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();
            List<ObservationAndRecommendation> recommList = db.ObservationAndRecommendations.ToList();
            
            var result = from dmg in culvertDmgResultList
                         join cul in culvertList on dmg.CulvertId equals cul.CulvertId into table1
                         from cul in table1.ToList()
                         join rec in recommList on cul.CulvertId equals rec.CulvertId into table2
                         from rec in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = dmg,
                             condition = CalculateDamageCondition(rec.UrgencyIndex),
                             recommendation = rec
                         };

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetCulvertDamageConditionBySegment(string segmentid /* drop down values */)
        {
            // Get all culverts in the current segment
            List<Culvert> culvertList = GetCulvertListBySegment(segmentid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();
            List<ObservationAndRecommendation> recommList = db.ObservationAndRecommendations.ToList();
            
            var result = from dmg in culvertDmgResultList
                         join cul in culvertList on dmg.CulvertId equals cul.CulvertId into table1
                         from cul in table1.ToList()
                         join rec in recommList on cul.CulvertId equals rec.CulvertId into table2
                         from rec in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = dmg,
                             condition = CalculateDamageCondition(rec.UrgencyIndex),
                             recommendation = rec
                         };

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            
            return PartialView(result);
        }

        public int[] GetInspectionYearRange(int inspectionyear)
        {
            var majInspYears = db.MajorInspectionYears.ToList();

            int[] range = new int[2];
            foreach (var maj in majInspYears)
            {
                if (inspectionyear == maj.InspectionYear)
                {
                    range[0] = (int)maj.StartYear;
                    range[1] = (int)maj.EndYear;
                    return range;
                }
            }

            return range;
        }

        public ActionResult _GetCulvertConditionByInspectionYearByDistrict(string districtid, int inspectionyear /* drop down values */)
        {
            // Gives range of years covered by major inspecion year
            // e.g. inspectionyear = 2016 => startyear = 2014, endyear = 2017
            int[] yearRange = GetInspectionYearRange(inspectionyear);
            int startyear = yearRange[0];
            int endyear = yearRange[1];

            // Get all culverts in the current district
            List<Culvert> culvertList = GetCulvertListByDistrict(districtid);

            // Get all damage inspection results in the current inspection year
            //List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.Where(s => DateTime.Parse(s.InspectionDate.ToString()).Year == inspectionyear).ToList();
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.Where(s => s.InspectionDate.ToString().Substring(0, 4).CompareTo(startyear.ToString()) >= 0
                                                               && s.InspectionDate.ToString().Substring(0, 4).CompareTo(endyear.ToString()) <= 0 ).ToList();

            List<ObservationAndRecommendation> recommList = db.ObservationAndRecommendations.ToList();
            
            var result = from dmg in culvertDmgResultList
                         join cul in culvertList on dmg.CulvertId equals cul.CulvertId into table1
                         from cul in table1.ToList()
                         join rec in recommList on cul.CulvertId equals rec.CulvertId into table2
                         from rec in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = dmg,
                             condition = CalculateDamageCondition(rec.UrgencyIndex),
                             recommendation = rec
                         };

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName;
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.YearRange = yearRange;

            return PartialView(result);
        }

        public ActionResult _GetCulvertConditionByInspectionYearBySection(string sectionid, int inspectionyear /* drop down values */)
        {
            // Gives range of years covered by major inspecion year
            // e.g. inspectionyear = 2016 => startyear = 2014, endyear = 2017
            int[] yearRange = GetInspectionYearRange(inspectionyear);
            int startyear = yearRange[0];
            int endyear = yearRange[1];

            // Get all culverts in the current district
            List<Culvert> culvertList = GetCulvertListBySection(sectionid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.Where(s => s.InspectionDate.ToString().Substring(0, 4).CompareTo(startyear.ToString()) >= 0
                                                               && s.InspectionDate.ToString().Substring(0, 4).CompareTo(endyear.ToString()) <= 0).ToList();

            List<ObservationAndRecommendation> recommList = db.ObservationAndRecommendations.ToList();
            
            var result = from dmg in culvertDmgResultList
                         join cul in culvertList on dmg.CulvertId equals cul.CulvertId into table1
                         from cul in table1.ToList()
                         join rec in recommList on cul.CulvertId equals rec.CulvertId into table2
                         from rec in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = dmg,
                             condition = CalculateDamageCondition(rec.UrgencyIndex),
                             recommendation = rec
                         };

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString(); 
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.YearRange = yearRange;

            return PartialView(result);
        }

        public ActionResult _GetCulvertConditionByInspectionYearBySegment(string segmentid, int inspectionyear /* drop down values */)
        {
            // Gives range of years covered by major inspecion year
            // e.g. inspectionyear = 2016 => startyear = 2014, endyear = 2017
            int[] yearRange = GetInspectionYearRange(inspectionyear);
            int startyear = yearRange[0];
            int endyear = yearRange[1];

            // Get all culverts in the current district
            List<Culvert> culvertList = GetCulvertListBySegment(segmentid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.Where(s => s.InspectionDate.ToString().Substring(0, 4).CompareTo(startyear.ToString()) >= 0
                                                               && s.InspectionDate.ToString().Substring(0, 4).CompareTo(endyear.ToString()) <= 0).ToList();

            List<ObservationAndRecommendation> recommList = db.ObservationAndRecommendations.ToList();
            
            var result = from dmg in culvertDmgResultList
                         join cul in culvertList on dmg.CulvertId equals cul.CulvertId into table1
                         from cul in table1.ToList()
                         join rec in recommList on cul.CulvertId equals rec.CulvertId into table2
                         from rec in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = dmg,
                             condition = CalculateDamageCondition(rec.UrgencyIndex),
                             recommendation = rec
                         };

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.YearRange = yearRange;

            return PartialView(result);
        }

        public ActionResult _GetObservationAndRecommendationByDistrict(string districtid /* drop down values */)
        {
            // Get all culverts in the current segment
            List<Culvert> culvertList = GetCulvertListByDistrict(districtid);
            List<ObservationAndRecommendation> observationList = db.ObservationAndRecommendations.ToList();
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();

            var result = from cul in culvertList
                         join rec in observationList on cul.CulvertId equals rec.CulvertId into table1
                         from rec in table1.ToList()
                         join res in culvertDmgResultList on cul.CulvertId equals res.CulvertId into table2
                         from res in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = res,
                             recommendation = rec
                         };

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName;

            return PartialView(result);
        }

        public ActionResult _GetObservationAndRecommendationBySection(string sectionid /* drop down values */)
        {
            // Get all culverts in the current segment
            List<Culvert> culvertList = GetCulvertListBySection(sectionid);
            List<ObservationAndRecommendation> observationList = db.ObservationAndRecommendations.ToList();
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();

            var result = from cul in culvertList
                         join rec in observationList on cul.CulvertId equals rec.CulvertId into table1
                         from rec in table1.ToList()
                         join res in culvertDmgResultList on cul.CulvertId equals res.CulvertId into table2
                         from res in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = res,
                             recommendation = rec
                         };

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetObservationAndRecommendationBySegment(string segmentid /* drop down values */)
        {
            // Get all culverts in the current segment
            List<Culvert> culvertList = GetCulvertListBySegment(segmentid);
            List<ObservationAndRecommendation> observationList = db.ObservationAndRecommendations.ToList();
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();

            var result = from cul in culvertList
                         join rec in observationList on cul.CulvertId equals rec.CulvertId into table1
                         from rec in table1.ToList()
                         join res in culvertDmgResultList on cul.CulvertId equals res.CulvertId into table2
                         from res in table2.ToList()
                         select new CulvertDamageInspectionViewModel
                         {
                             culverts = cul,
                             resultInspCulvert = res,
                             recommendation = rec
                         };

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetConditionAndRepairCostSummary(int inspectionyear)
        {
            List<CulvertConditionAndRepairCostByDistrict> culCondRepairCostByDistrict = db.CulvertConditionAndRepairCostByDistricts.Where(s => s.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;
            ViewBag.YearRange = GetInspectionYearRange(inspectionyear);
            
            return PartialView(culCondRepairCostByDistrict);
        }

        public ActionResult _GetConditionAndRepairCostSummaryByDistrict(string districtid, int inspectionyear)
        {
            List<CulvertConditionAndRepairCostBySection> culCondRepairCostByDistrict = db.CulvertConditionAndRepairCostBySections.Where(s => s.InspectionYear == inspectionyear && s.District == districtid).ToList();
            
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.YearRange = GetInspectionYearRange(inspectionyear);

            return PartialView(culCondRepairCostByDistrict);
        }

        public ActionResult _GetConditionAndRepairCostSummaryBySection(string sectionid, int inspectionyear)
        {
            List<CulvertConditionAndRepairCostBySegment> culCondRepairCostBySegment = db.CulvertConditionAndRepairCostBySegments.Where(s => s.InspectionYear == inspectionyear && s.Section == sectionid).ToList();

            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.YearRange = GetInspectionYearRange(inspectionyear);

            return PartialView(culCondRepairCostBySegment);
        }

        // GET: CulvertDamageCondition
        public ActionResult CulvertDamageCondition()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public List<List<culDamageInspStructure>> CulStrInspectionDetails(string culvertid)
        {
            // Get all damage inspection results in the current inspection year
            List<culDamageInspStructure> culDamageInspStructureList = db.culDamageInspStructures.Where(s => s.CulvertId == culvertid).ToList();

            // Get a refined list of damage inspection results for each structure item in culvert
            List<culDamageInspStructure> culvertList = culDamageInspStructureList.Where(s => s.StructureItemId == 1).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> abutmentList = culDamageInspStructureList.Where(s => s.StructureItemId == 2).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> guardParapetList = culDamageInspStructureList.Where(s => s.StructureItemId == 3).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> wingWallList = culDamageInspStructureList.Where(s => s.StructureItemId == 4).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> headWallList = culDamageInspStructureList.Where(s => s.StructureItemId == 5).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> pavedWaterWayList = culDamageInspStructureList.Where(s => s.StructureItemId == 6).OrderBy(s => s.DamageTypeId).ToList();

            // Put the lists into culvert structure list
            List<List<culDamageInspStructure>> culStructureList = new List<List<culDamageInspStructure>>
            {
                culvertList,
                abutmentList,
                guardParapetList,
                wingWallList,
                headWallList,
                pavedWaterWayList
            };

            // Empty the culvert structure list if it is composed of empty structure item lists
            bool isEmpty = true;
            for (int i = 0; i < culStructureList.Count; i++)
            {
                if (culStructureList[i].Count != 0)
                    isEmpty = false;
            }

            if (isEmpty)
                culStructureList.Clear();

            return culStructureList;
        }

        public List<List<culDamageRateAndCost>> CulvertStructureDamageRateAndCost()
        {
            var dmgRate = db.culDamageRateAndCosts.ToList();

            // Get damage rate and cost for each structure item in culver structures
            List<culDamageRateAndCost> dmgRateCulvert = dmgRate.Where(d => d.StructureItemId == 1).ToList();
            List<culDamageRateAndCost> dmgRateAbutment = dmgRate.Where(d => d.StructureItemId == 2).ToList();
            List<culDamageRateAndCost> dmgRateGuardParapet = dmgRate.Where(d => d.StructureItemId == 3).ToList();
            List<culDamageRateAndCost> dmgRateWingWall = dmgRate.Where(d => d.StructureItemId == 4).ToList();
            List<culDamageRateAndCost> dmgRateHearWall = dmgRate.Where(d => d.StructureItemId == 5).ToList();
            List<culDamageRateAndCost> dmgRatePavedWaterWay = dmgRate.Where(d => d.StructureItemId == 6).ToList();

            // Put the unit repair cost lists into culvert structure list
            List<List<culDamageRateAndCost>> culDmgRateAndCost = new List<List<culDamageRateAndCost>>
            {
                dmgRateCulvert,
                dmgRateAbutment,
                dmgRateGuardParapet,
                dmgRateWingWall,
                dmgRateHearWall,
                dmgRatePavedWaterWay
             };

            List<List<culDamageInspStructure>> refinedDmgInspList = ViewBag.CulvertStructure;

            for (int i=0; i< refinedDmgInspList.Count; i++)
            {
                if (ViewBag.CulvertStructure[i].Count == 0)
                {
                    culDmgRateAndCost.RemoveAt(i);
                    refinedDmgInspList.RemoveAt(i);
                    i--;
                }
            }

            ViewBag.CulvertStructure = refinedDmgInspList;

            return culDmgRateAndCost;
        }

        public List<List<culDamageInspStructure>> CulvertStructureInspectionDetails(string culvertid)
        {
            // Get all damage inspection results
            List<culDamageInspStructure> culDmgInspStrList = db.culDamageInspStructures.Where(s => s.CulvertId == culvertid).ToList();

            // Get a refined list of damage inspection results for each structure item in Substructure
            List<culDamageInspStructure> culvertList = culDmgInspStrList.Where(s => s.StructureItemId == 1).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> abutmentList = culDmgInspStrList.Where(s => s.StructureItemId == 2).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> guardParapetList = culDmgInspStrList.Where(s => s.StructureItemId == 3).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> wingWallList = culDmgInspStrList.Where(s => s.StructureItemId == 4).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> headWallList = culDmgInspStrList.Where(s => s.StructureItemId == 3).OrderBy(s => s.DamageTypeId).ToList();
            List<culDamageInspStructure> PavedWaterWayList = culDmgInspStrList.Where(s => s.StructureItemId == 4).OrderBy(s => s.DamageTypeId).ToList();

            // Put the lists into culvert structure culvert part
            List<List<culDamageInspStructure>> culStructureList = new List<List<culDamageInspStructure>>
            {
                culvertList,
                abutmentList,
                guardParapetList,
                wingWallList,
                headWallList,
                PavedWaterWayList
            };

            // Empty the substructure list if it is composed of empty structure item lists
            bool isEmpty = true;
            for (int i = 0; i < culStructureList.Count; i++)
            {
                if (culStructureList[i].Count != 0)
                    isEmpty = false;
            }

            if (isEmpty)
                culStructureList.Clear();

            return culStructureList;
        }

        public ActionResult _GetCulvertInspectionDetails(string culvertid /* drop down values */)
        {
            var segmentid = db.Culverts.Find(culvertid).SegmentId;

            ViewBag.CulvertNo = db.Culverts.Find(culvertid).CulvertNo.ToString();
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.CulvertStructure = CulStrInspectionDetails(culvertid);
            ViewBag.DamageRateCulvert = CulvertStructureDamageRateAndCost();

            ViewBag.DamageRateCulvert = CulvertStructureDamageRateAndCost();

            ViewBag.HydraulicAndChannelDamage = db.culDamageInspHydraulics.Find(culvertid);

            ObservationAndRecommendation obs = db.ObservationAndRecommendations.Find(culvertid);
            ViewBag.Recommendation = obs.InspectorRecommendation;
            ViewBag.InterventionType = obs.MaintenanceUrgency.UrgencyName;

            return PartialView();
        }

        public ActionResult _GetBillOfQuantity(string culvertid /* drop down values */)
        {
            var segmentid = db.Culverts.Find(culvertid).SegmentId;

            ViewBag.CulvertNo = db.Culverts.Find(culvertid).CulvertNo.ToString();
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.CulvertStructure = CulStrInspectionDetails(culvertid);
            ViewBag.DamageRateCulvert = CulvertStructureDamageRateAndCost();
            
            return PartialView();
        }

        public ActionResult BillOfQuantity(string id /* drop down values */)
        {
            var segmentid = db.Culverts.Find(id).SegmentId;

            ViewBag.CulvertNo = db.Culverts.Find(id).CulvertNo.ToString();
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.CulvertStructure = CulStrInspectionDetails(id);
            ViewBag.DamageRateCulvert = CulvertStructureDamageRateAndCost();

            return View();
        }
    }
}