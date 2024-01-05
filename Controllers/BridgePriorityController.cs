using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Data.Entity;
using System.Net;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class BridgePriorityController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: BridgePriority
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BridgePriorityByInterventionRequirement()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;

            List<SelectListItem> inspYearList = (from DmgInspMjr in db.DamageInspMajors
                                                 orderby DmgInspMjr.InspectionYear descending
                                                 select new
                                                 {
                                                     DmgInspMjr.InspectionYear
                                                 }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
                                                 { Text = c.InspectionYear.ToString(), Value = c.InspectionYear.ToString() }).ToList();
            ViewBag.InspectionYear = inspYearList;

            List<SelectListItem> priorityTypeList = new List<SelectListItem>();

            //Add items
            priorityTypeList.Insert(0, new SelectListItem { Text = "Bridge Priority by Road Segment", Value = "1" });
            priorityTypeList.Insert(1, new SelectListItem { Text = "Bridge Priority by Section", Value = "2" });
            priorityTypeList.Insert(2, new SelectListItem { Text = "Bridge Priority by District", Value = "3" });
            priorityTypeList.Insert(3, new SelectListItem { Text = "Bridge Priority (All)", Value = "4" });
            ViewBag.PriorityTypes = priorityTypeList;

            return View();
        }

        public ActionResult _GetBridgePriorityByInterventionRequirementBySegment(string segmentid, int inspectionyear)
        {
            // Get allbridge priority results in the current inspection year
            List<BridgePriorityByInterventionRequirement> bridgePriority = db.BridgePriorityByInterventionRequirements.Where(pr => pr.SegmentId == segmentid && pr.InspectionYear == inspectionyear).ToList();
            
            ViewBag.InspectionYear = inspectionyear;
            var segment = db.Segments.Find(segmentid);
            ViewBag.Segment = segment.SegmentName;
            ViewBag.Section = segment.Section.SectionName;
            ViewBag.District = segment.Section.District.DistrictName;

            return PartialView(bridgePriority);
        }

        public ActionResult _GetBridgePriorityByInterventionRequirementBySection(string sectionid, int inspectionyear)
        {
            // Get allbridge priority results in the current inspection year
            List<BridgePriorityByInterventionRequirement> bridgePriority = db.BridgePriorityByInterventionRequirements.Where(pr => pr.SectionId == sectionid && pr.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;
            ViewBag.Section = db.Sections.Find(sectionid).SectionName;
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName;

            return PartialView(bridgePriority);
        }

        public ActionResult _GetBridgePriorityByInterventionRequirementByDistrict(string districtid, int inspectionyear)
        {
            // Get allbridge priority results in the current inspection year
            List<BridgePriorityByInterventionRequirement> bridgePriority = db.BridgePriorityByInterventionRequirements.Where(pr => pr.DistrictId == districtid &&  pr.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;
            ViewBag.District = db.Districts.Find(districtid).DistrictName;

            return PartialView(bridgePriority);
        }

        public ActionResult _GetBridgePriorityByInterventionRequirementAll(int inspectionyear)
        {
            // Get allbridge priority results in the current inspection year
            //List<BridgePriorityByInterventionRequirement> bridgePriority = db.BridgePriorityByInterventionRequirements.Where(pr => pr.InspectionYear == inspectionyear).ToList();
            List<BridgePriorityByInterventionRequirement> bridgePriority = db.BridgePriorityByInterventionRequirements.ToList();

            ViewBag.InspectionYear = inspectionyear;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult BridgePriorityByHighestDamageRank()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;

            List<SelectListItem> inspYearList = (from DmgInspMjr in db.DamageInspMajors
                                                 orderby DmgInspMjr.InspectionYear descending
                                                 select new
                                                 {
                                                     DmgInspMjr.InspectionYear
                                                 }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
                                                  { Text = c.InspectionYear.ToString(), Value = c.InspectionYear.ToString() } ).ToList();
            ViewBag.InspectionYear = inspYearList;

            return View();
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByHighestDamageRankBySegment(string segmentid, int inspectionyear)
        {
            List<BridgePriorityByHighestDamageRank> bridgePriority = db.BridgePriorityByHighestDamageRanks.Where(pr => pr.SegmentId == segmentid && pr.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;
            var segment = db.Segments.Find(segmentid);
            ViewBag.Segment = segment.SegmentName;
            ViewBag.Section = segment.Section.SectionName;
            ViewBag.District = segment.Section.District.DistrictName;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByHighestDamageRankBySection(string sectionid, int inspectionyear)
        {
            List<BridgePriorityByHighestDamageRank> bridgePriority = db.BridgePriorityByHighestDamageRanks.Where(pr => pr.SectionId == sectionid && pr.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;
            ViewBag.Section = db.Sections.Find(sectionid).SectionName;
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByHighestDamageRankByDistrict(string districtid, int inspectionyear)
        {
            List<BridgePriorityByHighestDamageRank> bridgePriority = db.BridgePriorityByHighestDamageRanks.Where(pr => pr.DistrictId == districtid && pr.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;
            ViewBag.District = db.Districts.Find(districtid).DistrictName;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByHighestDamageRankAll(int inspectionyear)
        {
            List<BridgePriorityByHighestDamageRank> bridgePriority = db.BridgePriorityByHighestDamageRanks.Where(pr => pr.InspectionYear == inspectionyear).ToList();

            ViewBag.InspectionYear = inspectionyear;

            return PartialView(bridgePriority);
        }

        // e.g. 0-10 = "Good", 10-15 = "Fair", 15-100 = "Bad", 
        public DamageConditionRange CalculateServiceCondition(double? dmgPercent)
        {
            List<DamageConditionRange> dmgRange = db.DamageConditionRanges.ToList();

            foreach (var item in dmgRange)
            {
                if (item.ValueFrom <= dmgPercent && dmgPercent < item.ValueTo)
                    return item;
            }
            return (new DamageConditionRange());
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult BridgePriorityByAverage()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;

            List<SelectListItem> inspYearList = (from DmgInspMjr in db.DamageInspMajors
                                                 orderby DmgInspMjr.InspectionYear descending
                                                 select new
                                                 {
                                                     DmgInspMjr.InspectionYear
                                                 }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
                                                 { Text = c.InspectionYear.ToString(), Value = c.InspectionYear.ToString() }).ToList();
            ViewBag.InspectionYear = inspYearList;

            return View();
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByAverageBySegment(string segmentid, int inspectionyear)
        {
            List<BridgePriorityByInterventionRequirement> priorityByIntervention = db.BridgePriorityByInterventionRequirements.Where(pr => pr.SegmentId == segmentid && pr.InspectionYear == inspectionyear).ToList();

            var maxRepairCost = priorityByIntervention.Max(s => s.RepairCost);

            var bridgePriority = (from pr in priorityByIntervention
                                  select new BridgePriorityByAverageViewModel
                                  {
                                      BridgeId = pr.BridgeId,
                                      RevisedBridgeNo = pr.RevisedBridgeNo,
                                      BridgeName = pr.BridgeName,
                                      InspectionYear = (int) pr.InspectionYear,
                                      UrgencyId = pr.UrgencyId,
                                      InspRecommendation = pr.UrgencyName,
                                      RepairCost = pr.RepairCost,
                                      RepairCostPerc = (pr.RepairCost / maxRepairCost) * 100, // %age ofcrepair cost to max repair cost in the list
                                      DmgPerc = pr.DmgPerc,
                                      PriorityVal = pr.PriorityVal,
                                      AverageDamage = (double)(((pr.RepairCost / maxRepairCost) * 100) + pr.DmgPerc + pr.PriorityVal) / 3, // average of the three %ages
                                      ServiceConditionId = CalculateServiceCondition(pr.DmgPerc).DamageConditionId,
                                      ServiceCondition = CalculateServiceCondition(pr.DmgPerc).DamageConditionName, // "Good", "Fair", "Bad"
                                      RequiredActionId = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionId,
                                      RequiredAction = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionName
                                  }).ToList();
            ViewBag.InspectionYear = inspectionyear;
            var segment = db.Segments.Find(segmentid);
            ViewBag.Segment = segment.SegmentName;
            ViewBag.Section = segment.Section.SectionName;
            ViewBag.District = segment.Section.District.DistrictName;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByAverageBySection(string sectionid, int inspectionyear)
        {
            List<BridgePriorityByInterventionRequirement> priorityByIntervention = db.BridgePriorityByInterventionRequirements.Where(pr => pr.SectionId == sectionid && pr.InspectionYear == inspectionyear).ToList();

            var maxRepairCost = priorityByIntervention.Max(s => s.RepairCost);

            var bridgePriority = (from pr in priorityByIntervention 
                                  select new BridgePriorityByAverageViewModel
                                  {
                                      BridgeId = pr.BridgeId,
                                      RevisedBridgeNo = pr.RevisedBridgeNo,
                                      BridgeName = pr.BridgeName,
                                      InspectionYear = (int)pr.InspectionYear,
                                      UrgencyId = pr.UrgencyId,
                                      InspRecommendation = pr.UrgencyName,
                                      RepairCost = pr.RepairCost,
                                      RepairCostPerc = (pr.RepairCost / maxRepairCost) * 100, // %age ofcrepair cost to max repair cost in the list
                                      DmgPerc = pr.DmgPerc,
                                      PriorityVal = pr.PriorityVal,
                                      AverageDamage = (double)(((pr.RepairCost / maxRepairCost) * 100) + pr.DmgPerc + pr.PriorityVal) / 3, // average of the three %ages
                                      ServiceConditionId = CalculateServiceCondition(pr.DmgPerc).DamageConditionId,
                                      ServiceCondition = CalculateServiceCondition(pr.DmgPerc).DamageConditionName, // "Good", "Fair", "Bad"
                                      RequiredActionId = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionId,
                                      RequiredAction = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionName
                                  }).ToList();

            ViewBag.InspectionYear = inspectionyear;
            ViewBag.Section = db.Sections.Find(sectionid).SectionName;
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByAverageByDistrict(string districtid, int inspectionyear)
        {
            List<BridgePriorityByInterventionRequirement> priorityByIntervention = db.BridgePriorityByInterventionRequirements.Where(pr => pr.DistrictId == districtid && pr.InspectionYear == inspectionyear).ToList();

            var maxRepairCost = priorityByIntervention.Max(s => s.RepairCost);

            var bridgePriority = (from pr in priorityByIntervention
                                  select new BridgePriorityByAverageViewModel
                                  {
                                      BridgeId = pr.BridgeId,
                                      RevisedBridgeNo = pr.RevisedBridgeNo,
                                      BridgeName = pr.BridgeName,
                                      InspectionYear = (int)pr.InspectionYear,
                                      UrgencyId = pr.UrgencyId,
                                      InspRecommendation = pr.UrgencyName,
                                      RepairCost = pr.RepairCost,
                                      RepairCostPerc = (pr.RepairCost / maxRepairCost) * 100, // percentage of repair cost to max repair cost in the list
                                      DmgPerc = pr.DmgPerc,
                                      PriorityVal = pr.PriorityVal,
                                      AverageDamage = (double)(((pr.RepairCost / maxRepairCost) * 100) + pr.DmgPerc + pr.PriorityVal) / 3, // average of the three %ages
                                      ServiceConditionId = CalculateServiceCondition(pr.DmgPerc).DamageConditionId,
                                      ServiceCondition = CalculateServiceCondition(pr.DmgPerc).DamageConditionName, // "Good", "Fair", "Bad"
                                      RequiredActionId = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionId,
                                      RequiredAction = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionName
                                  }).ToList(); 
            
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.District = db.Districts.Find(districtid).DistrictName;

            return PartialView(bridgePriority);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult _GetBridgePriorityByAverageAll(int inspectionyear)
        {
            List<BridgePriorityByInterventionRequirement> priorityByIntervention = db.BridgePriorityByInterventionRequirements.Where(pr => pr.InspectionYear == inspectionyear).ToList();

            var maxRepairCost = priorityByIntervention.Max(s => s.RepairCost);

            var bridgePriority = (from pr in priorityByIntervention
                                  select new BridgePriorityByAverageViewModel
                                  {
                                      BridgeId = pr.BridgeId,
                                      RevisedBridgeNo = pr.RevisedBridgeNo,
                                      BridgeName = pr.BridgeName,
                                      InspectionYear = (int)pr.InspectionYear,
                                      UrgencyId = pr.UrgencyId,
                                      InspRecommendation = pr.UrgencyName,
                                      RepairCost = pr.RepairCost,
                                      RepairCostPerc = (pr.RepairCost / maxRepairCost) * 100, // %age ofcrepair cost to max repair cost in the list
                                      DmgPerc = pr.DmgPerc,
                                      PriorityVal = pr.PriorityVal,
                                      AverageDamage = (double)(((pr.RepairCost / maxRepairCost) * 100) + pr.DmgPerc + pr.PriorityVal) / 3, // average of the three %ages
                                      ServiceConditionId = CalculateServiceCondition(pr.DmgPerc).DamageConditionId,
                                      ServiceCondition = CalculateServiceCondition(pr.DmgPerc).DamageConditionName, // "Good", "Fair", "Bad"
                                      RequiredActionId = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionId,
                                      RequiredAction = db.RequiredActions.Find(pr.RequiredActionId).RequiredActionName
                                  }).ToList();

            ViewBag.InspectionYear = inspectionyear;

            return PartialView(bridgePriority);
        }

        //public double GetDamagePercentWeight(double dmgPerc)
        //{
        //    double dmgPercWt = 0.0;

        //    var dmgPercPriority = (from c in db.DamagePriorities.ToList()
        //                           where c.PrioritizationCriteriaId == 1
        //                           select new
        //                           {
        //                               c.DmgValFrom,
        //                               c.DmgValTo,
        //                               c.DmgWtVal
        //                           }).ToList();

        //    if (dmgPerc > dmgPercPriority[0].DmgValFrom && dmgPerc <= dmgPercPriority[0].DmgValTo)
        //        dmgPercWt = dmgPercPriority[0].DmgWtVal;
        //    else if (dmgPerc > dmgPercPriority[1].DmgValFrom && dmgPerc <= dmgPercPriority[1].DmgValTo)
        //        dmgPercWt = dmgPercPriority[1].DmgWtVal;
        //    else if (dmgPerc > dmgPercPriority[2].DmgValFrom && dmgPerc <= dmgPercPriority[2].DmgValTo)
        //        dmgPercWt = dmgPercPriority[2].DmgWtVal;

        //    return dmgPercWt;
        //}

        //public double GetUrgencyIndexWeight(double urgencyIndex)
        //{
        //    double urgencyIndexWt = 0.0;

        //    var urgencyIndexPriority = (from c in db.DamagePriorities.ToList()
        //                                where c.PrioritizationCriteriaId == 2
        //                                select new
        //                                {
        //                                    c.DmgValTo,
        //                                    c.DmgWtVal
        //                                }).ToList();

        //    if (urgencyIndex == urgencyIndexPriority[0].DmgValTo)
        //        urgencyIndexWt = urgencyIndexPriority[0].DmgWtVal;
        //    else if (urgencyIndex == urgencyIndexPriority[1].DmgValTo)
        //        urgencyIndexWt = urgencyIndexPriority[1].DmgWtVal;
        //    else if (urgencyIndex == urgencyIndexPriority[2].DmgValTo)
        //        urgencyIndexWt = urgencyIndexPriority[2].DmgWtVal;

        //    return urgencyIndexWt;
        //}

        //public double GetWaterWayAdequacyWeight(int waterWay)
        //{
        //    double waterWayWt = 0.0;

        //    var waterWayPriority = (from c in db.DamagePriorities.ToList()
        //                            where c.PrioritizationCriteriaId == 3
        //                            select new
        //                            {
        //                                c.DmgValTo,
        //                                c.DmgWtVal
        //                            }).ToList();

        //    // If waterWay = 1 (Yes) => waterWayWt = 7
        //    // If waterWay = 0 (No) => waterWayWt = 0
        //    if (waterWay == waterWayPriority[0].DmgValTo - 1)
        //        waterWayWt = waterWayPriority[1].DmgWtVal;
        //    else if (waterWay == waterWayPriority[1].DmgValTo - 1)
        //        waterWayWt = waterWayPriority[0].DmgWtVal;

        //    return waterWayWt;
        //}

        //public double GetRoadClassWeight(double roadClass)
        //{
        //    double roadClassWt = 0.0;

        //    var roadClassPriority = (from c in db.DamagePriorities.ToList()
        //                             where c.PrioritizationCriteriaId == 4
        //                             select new
        //                             {
        //                                 c.DmgValTo,
        //                                 c.DmgWtVal
        //                             }).ToList();

        //    if (roadClass == roadClassPriority[0].DmgValTo)
        //        roadClassWt = roadClassPriority[0].DmgWtVal;
        //    else if (roadClass == roadClassPriority[1].DmgValTo)
        //        roadClassWt = roadClassPriority[1].DmgWtVal;
        //    else if (roadClass == roadClassPriority[2].DmgValTo)
        //        roadClassWt = roadClassPriority[2].DmgWtVal;
        //    else if (roadClass == roadClassPriority[3].DmgValTo)
        //        roadClassWt = roadClassPriority[3].DmgWtVal;
        //    else if (roadClass == roadClassPriority[4].DmgValTo)
        //        roadClassWt = roadClassPriority[4].DmgWtVal;

        //    return roadClassWt;
        //}

        //public double GetDBridgeLengthWeight(double brgLength)
        //{
        //    double brgLengthWt = 0.0;

        //    var brgLengthPriority = (from c in db.DamagePriorities.ToList()
        //                             where c.PrioritizationCriteriaId == 5
        //                             select new
        //                             {
        //                                 c.DmgValFrom,
        //                                 c.DmgValTo,
        //                                 c.DmgWtVal
        //                             }).ToList();

        //    if (brgLength > brgLengthPriority[0].DmgValFrom && brgLength <= brgLengthPriority[0].DmgValTo)
        //        brgLengthWt = brgLengthPriority[0].DmgWtVal;
        //    else if (brgLength > brgLengthPriority[1].DmgValFrom && brgLength <= brgLengthPriority[1].DmgValTo)
        //        brgLengthWt = brgLengthPriority[1].DmgWtVal;
        //    else if (brgLength > brgLengthPriority[2].DmgValFrom && brgLength <= brgLengthPriority[2].DmgValTo)
        //        brgLengthWt = brgLengthPriority[2].DmgWtVal;
        //    else if (brgLength >= brgLengthPriority[3].DmgValFrom && brgLength <= brgLengthPriority[3].DmgValTo)
        //        brgLengthWt = brgLengthPriority[3].DmgWtVal;

        //    return brgLengthWt;
        //}

        //public double GetConstructionYearWeight(double constrYear)
        //{
        //    double constrYearWt = 0.0;

        //    var contrYearPriority = (from c in db.DamagePriorities.ToList()
        //                             where c.PrioritizationCriteriaId == 6
        //                             select new
        //                             {
        //                                 c.DmgValFrom,
        //                                 c.DmgValTo,
        //                                 c.DmgWtVal
        //                             }).ToList();

        //    if (constrYear > contrYearPriority[0].DmgValFrom && constrYear <= contrYearPriority[0].DmgValTo)
        //        constrYearWt = contrYearPriority[0].DmgWtVal;
        //    else if (constrYear > contrYearPriority[1].DmgValFrom && constrYear <= contrYearPriority[1].DmgValTo)
        //        constrYearWt = contrYearPriority[1].DmgWtVal;
        //    else if (constrYear > contrYearPriority[2].DmgValFrom && constrYear <= contrYearPriority[2].DmgValTo)
        //        constrYearWt = contrYearPriority[2].DmgWtVal;
        //    else if (constrYear > contrYearPriority[3].DmgValFrom && constrYear <= contrYearPriority[3].DmgValTo)
        //        constrYearWt = contrYearPriority[3].DmgWtVal;

        //    return constrYearWt;
        //}

        //public double GetDetourPossibilityWeight(int detour)
        //{
        //    double detourWt = 0.0;

        //    var detourPriority = (from c in db.DamagePriorities.ToList()
        //                          where c.PrioritizationCriteriaId == 7
        //                          select new
        //                          {
        //                              c.DmgValTo,
        //                              c.DmgWtVal
        //                          }).ToList();

        //    // If detour = 1 (Yes) => detourWt = 3
        //    // If detour = 0 (No) => detourWt = 0
        //    if (detour == detourPriority[0].DmgValTo - 1)
        //        detourWt = detourPriority[1].DmgWtVal;
        //    else if (detour == detourPriority[1].DmgValTo - 1)
        //        detourWt = detourPriority[0].DmgWtVal;

        //    return detourWt;
        //}

        //public double GetADTWeight(double ADT)
        //{
        //    double ADTWt = 0.0;

        //    var ADTPriority = (from c in db.DamagePriorities.ToList()
        //                       where c.PrioritizationCriteriaId == 8
        //                       select new
        //                       {
        //                           c.DmgValFrom,
        //                           c.DmgValTo,
        //                           c.DmgWtVal
        //                       }).ToList();

        //    if (ADT >= ADTPriority[0].DmgValFrom && ADT <= ADTPriority[0].DmgValTo)
        //        ADTWt = ADTPriority[0].DmgWtVal;
        //    else if (ADT > ADTPriority[1].DmgValFrom && ADT <= ADTPriority[1].DmgValTo)
        //        ADTWt = ADTPriority[1].DmgWtVal;
        //    else if (ADT > ADTPriority[2].DmgValFrom && ADT <= ADTPriority[2].DmgValTo)
        //        ADTWt = ADTPriority[2].DmgWtVal;

        //    return ADTWt;
        //}

        //public double CalculatePriorityPercent(string bridgeid, int inspectionyear)
        //{
        //    double dmgPerc = (double)db.ResultInspMajors.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).FirstOrDefault().DmgPerc;

        //    var comment = db.BridgeObservations.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).FirstOrDefault();
        //    int urgencyIndex = (int)comment.UrgencyId;
        //    int waterWay = ((bool)comment.WaterWayAdequacy == true) ? 1 : 0;

        //    var genInfo = db.BridgeGeneralInfoes.Find(bridgeid);

        //    // If bridge lenth data is available take it. Otherwise, simply take 6.0 meter which is the minimum
        //    double brgLength = (genInfo.BridgeLength == null) ? 6.0 : (double)genInfo.BridgeLength;

        //    // If Replaced year data is available take it. Otherwise, simply take construction year
        //    int constrYear = (genInfo.ReplacedYear == null) ? (int)genInfo.ConstructionYear : (int)genInfo.ReplacedYear;
        //    int detour = ((bool)genInfo.DetourPossible == true) ? 1 : 0;

        //    // Take average daily traffic data from the road segment
        //    var segmentADT = db.Bridges.Find(bridgeid).Segment.AverageDailyTraffic;

        //    // If average daily traffic is null, then take ADT = 0
        //    int ADT = (subRouteADT == null) ? 0 : (int)subRouteADT;

        //    // Take road class data from the subroute
        //    int roadClass = (int)db.Bridges.Find(bridgeid).SubRoute.MainRoute.RoadClassId;

        //    return GetDamagePercentWeight(dmgPerc) + GetUrgencyIndexWeight(urgencyIndex) + GetWaterWayAdequacyWeight(waterWay)
        //                + GetRoadClassWeight(roadClass) + GetDBridgeLengthWeight(brgLength) + GetConstructionYearWeight(constrYear)
        //                + GetDetourPossibilityWeight(detour) + GetADTWeight(ADT);
        //}
    }
}