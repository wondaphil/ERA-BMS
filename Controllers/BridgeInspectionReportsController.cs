using ERA_BMS.Models;
using ERA_BMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERA_BMS.Controllers
{
    public class BridgeInspectionReportsController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Index
        public ActionResult Index()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts
            ViewBag.Districts = districtList;

            List<SelectListItem> inspectionYearList = (from DmgInspMjr in db.DamageInspMajors
                                                       orderby DmgInspMjr.InspectionYear descending
                                                       select new
                                                       {
                                                           DmgInspMjr.InspectionYear
                                                       }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
                                                               {
                                                                   Text = c.InspectionYear.ToString(),
                                                                   Value = c.InspectionYear.ToString()
                                                               }
                                                            ).ToList();

            ViewBag.InspectionYear = inspectionYearList;

            List<SelectListItem> reportTypeList = new List<SelectListItem>();

            //Add items
            reportTypeList.Insert(0, new SelectListItem { Text = "Bridge Main Parts Damage Condition", Value = "1" });
            reportTypeList.Insert(1, new SelectListItem { Text = "Bridge Service Condition", Value = "2" });
            reportTypeList.Insert(2, new SelectListItem { Text = "Inspector's Recommendation & Comments", Value = "3" });
            reportTypeList.Insert(3, new SelectListItem { Text = "Bridge Improvement Priority", Value = "4" });
            reportTypeList.Insert(4, new SelectListItem { Text = "Bridge Improvement Indicator", Value = "5" });
            reportTypeList.Insert(5, new SelectListItem { Text = "Prevalent Bridge Damage Type by Inspection Year", Value = "6" });
            reportTypeList.Insert(6, new SelectListItem { Text = "Summary of Bridge Condition & Repair Cost", Value = "7" });
            reportTypeList.Insert(7, new SelectListItem { Text = "Bridge Regular/Visual Inspection Details", Value = "8" });
            reportTypeList.Insert(8, new SelectListItem { Text = "Bridge Major Inspection Details", Value = "9" });
            reportTypeList.Insert(9, new SelectListItem { Text = "Bridge Bill of Quantity (BOQ)", Value = "10" });

            ViewBag.ReportTypes = reportTypeList;

            return View();
        }

        public ActionResult BridgeConditionAndBOQ()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            List<SelectListItem> inspectionYearList = (from DmgInspMjr in db.DamageInspMajors
                                                       orderby DmgInspMjr.InspectionYear descending
                                                       select new
                                                       {
                                                           DmgInspMjr.InspectionYear
                                                       }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
                                                                   {
                                                                       Text = c.InspectionYear.ToString(),
                                                                       Value = c.InspectionYear.ToString()
                                                                   }
                                                            ).ToList();
            ViewBag.InspectionYear = inspectionYearList;

            List<SelectListItem> reportTypeList = new List<SelectListItem>();

            //Add items
            reportTypeList.Insert(0, new SelectListItem { Text = "Regular/Visual Inspection Details", Value = "1" });
            reportTypeList.Insert(1, new SelectListItem { Text = "Major Inspection Details", Value = "2" });
            reportTypeList.Insert(2, new SelectListItem { Text = "Bill of Quantity (BOQ)", Value = "3" });
            
            ViewBag.ReportTypes = reportTypeList;

            return View();
        }

        public ActionResult _GetVisualInspectionDetails(string bridgeid)
        {
            var dmgInspVisualList = db.DamageInspVisuals.Where(d => d.BridgeId == bridgeid).ToList();
            //List<DamageType> damageTypeList = new List<DamageType>();
            //List<string> strItems = db.StructureItems.Select(s => s.StructureItemName).ToList();

            ViewBag.StrItemNamesList = db.StructureItems.Select(s => s.StructureItemName).ToList();
            ViewBag.BridgeName = db.Bridges.Find(bridgeid).BridgeName;
            ViewBag.Segment = db.Bridges.Find(bridgeid).Segment.SegmentName.ToString();
            ViewBag.Section = db.Bridges.Find(bridgeid).Segment.Section.SectionName.ToString();
            ViewBag.District = db.Bridges.Find(bridgeid).Segment.Section.District.DistrictName.ToString();

            return PartialView(dmgInspVisualList);
        }

        public ActionResult VisualInspectionDetails()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        // e.g. 0-10 = Good, 10-15 = Fair, 15-100 = Bad, 
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
        
        public List<Bridge> GetBridgeListByDistrict(string districtid)
        {
            List<Bridge> bridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       where sec.DistrictId == districtid
                                       select br).ToList();

            return bridgeList;
        }

        public List<Bridge> GetBridgeListBySection(string sectionid)
        {
            List<Bridge> bridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       where seg.SectionId == sectionid
                                       select br).ToList();

            return bridgeList;
        }

        public List<Bridge> GetBridgeListBySegment(string segmentid)
        {
            List<Bridge> bridgeList = (from br in db.Bridges
                                       where br.SegmentId == segmentid
                                       select br).ToList();

            return bridgeList;
        }

        public ActionResult _GetBridgeMainPartsDamageConditionBySegment(string segmentid, int inspectionyear /* drop down values */)
        {
            // Get all bridges in the current segment
            List<Bridge> bridgeList = GetBridgeListBySegment(segmentid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            //          SELECT *
            //          FROM Bridge JOIN ResultInspMajor
            //          ON bridge.BridgeId = ResultInspMajor.BridgeId
            //          WHERE SegmentId = 8011 and ResultInspMajor.InspectionYear = 2006

            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeMainPartsDamageConditionBySection(string sectionid, int inspectionyear /* drop down values */)
        {
            // Get all bridges in the current section
            List<Bridge> bridgeList = GetBridgeListBySection(sectionid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            //          SELECT *
            //          FROM Bridge JOIN ResultInspMajor
            //          ON bridge.BridgeId = ResultInspMajor.BridgeId
            //          WHERE SegmentId = 8011 and ResultInspMajor.InspectionYear = 2006

            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeMainPartsDamageConditionByDistrict(string districtid, int inspectionyear /* drop down values */)
        {
            // Get all bridges in the current district
            List<Bridge> bridgeList = GetBridgeListByDistrict(districtid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            //          SELECT *
            //          FROM Bridge JOIN ResultInspMajor
            //          ON bridge.BridgeId = ResultInspMajor.BridgeId
            //          WHERE SegmentId = 8011 and ResultInspMajor.InspectionYear = 2006

            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeDamageCondition(int inspectionyear /* drop down values */)
        {
            // Get all bridges in all districts
            List<Bridge> bridgeList = db.Bridges.ToList();

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeDamageConditionByDistrict(string districtid, int inspectionyear /* drop down values */)
        {
            // Get all bridges in the current district
            List<Bridge> bridgeList = GetBridgeListByDistrict(districtid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeDamageConditionBySection(string sectionid, int inspectionyear /* drop down values */)
        {
            // Get all bridges in the current section
            List<Bridge> bridgeList = GetBridgeListBySection(sectionid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeDamageConditionBySegment(string segmentid, int inspectionyear /* drop down values */)
        {
            // Get all bridges in the current segment
            List<Bridge> bridgeList = GetBridgeListBySegment(segmentid);

            // Get all damage inspection results in the current inspection year
            List<ResultInspMajor> resultInspMajorList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Join the two tables based on BridgeId
            var result = from dmg in resultInspMajorList
                         join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                         from br in table1.ToList()
                         select new BridgeServiceConditionViewModel
                         {
                             bridges = br,
                             resultInspMajor = dmg,
                             condition = CalculateServiceCondition(dmg.DmgPerc)
                         };

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result);
        }

        public ActionResult _GetRecommendationsAndCommentsByDistrict(string districtid, int inspectionyear)
        {
            // Get all bridges in the current district
            List<Bridge> bridgeList = GetBridgeListByDistrict(districtid);

            // Get all comments in the given inspection year
            List<BridgeComment> comment = db.BridgeComments.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all comments in the given segment and inspection year
            var commentList = (from com in comment
                                 join br in bridgeList on com.BridgeId equals br.BridgeId into table1
                                 from br in table1.ToList()
                                 select new BridgeComment
                                 {
                                     BridgeId = com.BridgeId,
                                     InspectionYear = (int)com.InspectionYear,
                                     InspectionDate = com.InspectionDate,
                                     Comment = com.Comment
                                 }).ToList();

            // Get all observations in the given inspection year
            List<BridgeObservation> observation = db.BridgeObservations.Where(s => s.InspectionYear == inspectionyear).ToList();
            
            // Get all observations in the given district and inspection year
            var observationList = (from obs in observation
                                     join br in bridgeList on obs.BridgeId equals br.BridgeId into table1
                                     from br in table1.ToList()
                                     select new BridgeObservation
                                     {
                                         BridgeId = obs.BridgeId,
                                         Bridge = db.Bridges.Find(obs.BridgeId),
                                         InspectionYear = (int)obs.InspectionYear,
                                         InspectionDate = (DateTime)obs.InspectionDate,
                                         UrgencyId = obs.UrgencyId,
                                         MaintenanceUrgency = db.MaintenanceUrgencies.Find(obs.UrgencyId),
                                         WaterWayAdequacy = obs.WaterWayAdequacy
                                     }).ToList();

            // Create a list of "BrdgeObservation" and "List<BridgeComment>"
            List<CommentAndObservationViewModel> commObservList = new List<CommentAndObservationViewModel>();

            for (int i = 0; i < observationList.Count; i++)
            {
                CommentAndObservationViewModel comObs = new CommentAndObservationViewModel();
                comObs.Observation = observationList[i];
                
                // Get a list comments for the current bridge 
                List<string> comm = commentList.Where(s => s.BridgeId == observationList[i].BridgeId).Select(s => s.Comment).ToList();
                comObs.Comment = comm;

                commObservList.Add(comObs);
            }

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(commObservList);
            //return PartialView();
        }

        public ActionResult _GetRecommendationsAndCommentsBySection(string sectionid, int inspectionyear)
        {
            // Get all bridges in the current section
            List<Bridge> bridgeList = GetBridgeListBySection(sectionid);

            // Get all comments in the given inspection year
            List<BridgeComment> comment = db.BridgeComments.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all comments in the given segment and inspection year
            var commentList = (from com in comment
                                 join br in bridgeList on com.BridgeId equals br.BridgeId into table1
                                 from br in table1.ToList()
                                 select new BridgeComment
                                 {
                                     BridgeId = com.BridgeId,
                                     InspectionYear = (int)com.InspectionYear,
                                     InspectionDate = com.InspectionDate,
                                     Comment = com.Comment
                                 }).ToList();

            // Get all observations in the given inspection year
            List<BridgeObservation> observation = db.BridgeObservations.Where(s => s.InspectionYear == inspectionyear).ToList();
            
            // Get all observations in the given section and inspection year
            var observationList = (from obs in observation
                                     join br in bridgeList on obs.BridgeId equals br.BridgeId into table1
                                     from br in table1.ToList()
                                     select new BridgeObservation
                                     {
                                         BridgeId = obs.BridgeId,
                                         Bridge = db.Bridges.Find(obs.BridgeId),
                                         InspectionYear = (int)obs.InspectionYear,
                                         InspectionDate = (DateTime)obs.InspectionDate,
                                         UrgencyId = obs.UrgencyId,
                                         MaintenanceUrgency = db.MaintenanceUrgencies.Find(obs.UrgencyId),
                                         WaterWayAdequacy = obs.WaterWayAdequacy
                                     }).ToList();

            // Create a list of "BrdgeObservation" and "List<BridgeComment>"
            List<CommentAndObservationViewModel> commObservList = new List<CommentAndObservationViewModel>();

            for (int i = 0; i < observationList.Count; i++)
            {
                CommentAndObservationViewModel comObs = new CommentAndObservationViewModel();
                comObs.Observation = observationList[i];
                
                // Get a list comments for the current bridge 
                List<string> comm = commentList.Where(s => s.BridgeId == observationList[i].BridgeId).Select(s => s.Comment).ToList();
                comObs.Comment = comm;

                commObservList.Add(comObs);
            }

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(commObservList);
            //return PartialView();
        }

        public ActionResult _GetRecommendationsAndCommentsBySegment(string segmentid, int inspectionyear)
        {
            // Get all bridges in the given segment
            List<Bridge> bridgeList = GetBridgeListBySegment(segmentid);

            // Get all comments in the given inspection year
            List<BridgeComment> comment = db.BridgeComments.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all comments in the given segment and inspection year
            var commentList = (from com in comment
                                 join br in bridgeList on com.BridgeId equals br.BridgeId into table1
                                 from br in table1.ToList()
                                 select new BridgeComment
                                 {
                                     BridgeId = com.BridgeId,
                                     InspectionYear = (int)com.InspectionYear,
                                     InspectionDate = com.InspectionDate,
                                     Comment = com.Comment
                                 }).ToList();

            // Get all observations in the given inspection year
            List<BridgeObservation> observation = db.BridgeObservations.Where(s => s.InspectionYear == inspectionyear).ToList();
            
            // Get all observations in the given segment and inspection year
            var observationList = (from obs in observation
                                     join br in bridgeList on obs.BridgeId equals br.BridgeId into table1
                                     from br in table1.ToList()
                                     select new BridgeObservation
                                     {
                                         BridgeId = obs.BridgeId,
                                         Bridge = db.Bridges.Find(obs.BridgeId),
                                         InspectionYear = (int)obs.InspectionYear,
                                         InspectionDate = (DateTime)obs.InspectionDate,
                                         UrgencyId = obs.UrgencyId,
                                         MaintenanceUrgency = db.MaintenanceUrgencies.Find(obs.UrgencyId),
                                         WaterWayAdequacy = obs.WaterWayAdequacy
                                     }).ToList();

            // Create a list of "BrdgeObservation" and "List<BridgeComment>"
            List<CommentAndObservationViewModel> commObservList = new List<CommentAndObservationViewModel>();

            for (int i = 0; i < observationList.Count; i++)
            {
                CommentAndObservationViewModel comObs = new CommentAndObservationViewModel();
                comObs.Observation = observationList[i];
                
                // Get a list comments for the current bridge 
                List<string> comm = commentList.Where(s => s.BridgeId == observationList[i].BridgeId).Select(s => s.Comment).ToList();
                comObs.Comment = comm;

                commObservList.Add(comObs);
            }

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(commObservList);
            //return PartialView();
        }

        public ActionResult _GetBridgeImprovementPriority(int inspectionyear)
        {
            // Get all bridges in the given segment
            List<Bridge> bridgeList = db.Bridges.ToList();

            // Get all damage inspection results in the given inspection year
            List<ResultInspMajor> bridgeDmgResultList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all bridges in the given segment and inspection year by ordering them based on repair cost from top to bottom
            var result = (from dmg in bridgeDmgResultList
                          join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select new BridgeServiceConditionViewModel
                          {
                              bridges = br,
                              resultInspMajor = dmg,
                              damagePercent = (double) dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }
        
        public ActionResult _GetBridgeImprovementPriorityByDistrict(string districtid, int inspectionyear)
        {
            // Get all bridges in the current district
            List<Bridge> bridgeList = GetBridgeListByDistrict(districtid);

            // Get all damage inspection results in the given inspection year
            List<ResultInspMajor> bridgeDmgResultList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all bridges in the given segment and inspection year by ordering them based on repair cost from top to bottom
            var result = (from dmg in bridgeDmgResultList
                          join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select new BridgeServiceConditionViewModel
                          {
                              bridges = br,
                              resultInspMajor = dmg,
                              damagePercent = (double) dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }
        
        public ActionResult _GetBridgeImprovementPriorityBySection(string sectionid, int inspectionyear)
        {
            // Get all bridges in the current section
            List<Bridge> bridgeList = GetBridgeListBySection(sectionid);

            // Get all damage inspection results in the given inspection year
            List<ResultInspMajor> bridgeDmgResultList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all bridges in the given segment and inspection year by ordering them based on repair cost from top to bottom
            var result = (from dmg in bridgeDmgResultList
                          join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select new BridgeServiceConditionViewModel
                          {
                              bridges = br,
                              resultInspMajor = dmg,
                              damagePercent = (double) dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }
        
        public ActionResult _GetBridgeImprovementPriorityBySegment(string segmentid, int inspectionyear)
        {
            // Get all bridges in the given segment
            List<Bridge> bridgeList = (from s in db.Bridges
                                       where s.SegmentId == segmentid
                                       select s).ToList();

            // Get all damage inspection results in the given inspection year
            List<ResultInspMajor> bridgeDmgResultList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all bridges in the given segment and inspection year by ordering them based on repair cost from top to bottom
            var result = (from dmg in bridgeDmgResultList
                          join br in bridgeList on dmg.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select new BridgeServiceConditionViewModel
                          {
                              bridges = br,
                              resultInspMajor = dmg,
                              damagePercent = (double) dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }
        
        public List<DamageByStructureItemByDamgeTypeViewModel> GetStructureItemDamageList(List<DamageInspMajor> dmgInspMaj, int stritemid)
        {
            //SELECT DamageInspMajor.StructureItemId, DamageInspMajor.DamageTypeId, count(bridgeid) as BridgeCount
            //FROM DamageInspMajor INNER JOIN StructureItem ON DamageInspMajor.StructureItemId = StructureItem.StructureItemId
            //                     INNER JOIN DamageType ON DamageInspMajor.DamageTypeId = DamageType.DamageTypeId
            //GROUP BY DamageInspMajor.StructureItemId, DamageInspMajor.DamageTypeId
            //ORDER BY StructureItemId, DamageTypeId

            var inspList = (from insp in dmgInspMaj
                            group insp by new { insp.StructureItemId, insp.DamageTypeId } into res
                            where res.Key.StructureItemId == stritemid
                            select new DamageByStructureItemByDamgeTypeViewModel
                            {
                                StructureItemId = (int)res.Key.StructureItemId,
                                DamageTypeId = (int)res.Key.DamageTypeId,
                                DamageTypeName = db.DamageTypes.Where(d => d.StructureItemId == res.Key.StructureItemId && d.DamageTypeId == res.Key.DamageTypeId).FirstOrDefault().DamageTypeName,
                                BridgeCount = res.Count()
                            }).OrderBy(s => s.StructureItemId).ThenBy(s => s.DamageTypeId).ToList();

            return inspList;
        }

        public ActionResult _GetPrevalentDamageType(int inspectionyear)
        {
            // Get all damage major inspections in the given inspection year
            List<DamageInspMajor> dmgByInspYear = db.DamageInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();
            
            // Get damage inspections list for each structure item
            ViewBag.PierAndFoundation = GetStructureItemDamageList(dmgByInspYear, 1);
            ViewBag.AbutmentAndWingwall = GetStructureItemDamageList(dmgByInspYear, 2);
            ViewBag.Embankment = GetStructureItemDamageList(dmgByInspYear, 3);
            ViewBag.RipRap = GetStructureItemDamageList(dmgByInspYear, 4);
            ViewBag.DeckSlab = GetStructureItemDamageList(dmgByInspYear, 5);
            ViewBag.ConcreteGirder = GetStructureItemDamageList(dmgByInspYear, 6);
            ViewBag.SteelTruss = GetStructureItemDamageList(dmgByInspYear, 7);
            ViewBag.Pavement = GetStructureItemDamageList(dmgByInspYear, 8);
            ViewBag.CurbAndRailing = GetStructureItemDamageList(dmgByInspYear, 9);
            ViewBag.Drainage = GetStructureItemDamageList(dmgByInspYear, 10);
            ViewBag.Bearing = GetStructureItemDamageList(dmgByInspYear, 11);
            ViewBag.ExpansionJoint = GetStructureItemDamageList(dmgByInspYear, 12);

            ViewBag.InspectionYear = inspectionyear;
            return PartialView();
        }

        public ActionResult _GetBridgeImprovementIndicator()
        {
            // Get all bridges in all districts
            var result = (from cond in db.BridgeConditionByInspectionYears
                          join br in db.Bridges on cond.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          join seg in db.Segments on br.SegmentId equals seg.SegmentId into table2
                          from seg in table2.ToList()
                          join sec in db.Sections on seg.SectionId equals sec.SectionId into table3
                          from sec in table3.ToList()
                          join dis in db.Districts on sec.DistrictId equals dis.DistrictId into table4
                          from dis in table4.ToList()
                          select new BridgeImprovementIndicatorViewModel
                          {
                              bridges = br,
                              segments = seg,
                              sections = sec,
                              districts = dis,
                              bridgeConditionByInspYear = cond
                          }).GroupBy(x => x.bridges.BridgeId).Select(x => x.FirstOrDefault()).ToList(); //to select distinct item

            ViewBag.NoOfBridges = db.MajorInspectionYears.OrderBy(s => s.InspectionYear).Select(s => s.NoOfRegisteredBridges).ToList();
            
            return PartialView(result);
        }
        
        public ActionResult _GetBridgeImprovementIndicatorByDistrict(string districtid)
        {
            // Get all bridges in the current district and join with bridge condition by inspection year results
            var result = (from br in db.Bridges
                          join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                          from seg in table1.ToList()
                          join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                          from sec in table2.ToList()
                          join cond in db.BridgeConditionByInspectionYears on br.BridgeId equals cond.BridgeId into table3
                          from cond in table3.ToList()
                          where sec.DistrictId == districtid
                          select new BridgeImprovementIndicatorViewModel
                          {
                              bridges = br,
                              segments = seg,
                              sections = sec,
                              bridgeConditionByInspYear = cond
                          }).GroupBy(x => x.bridges.BridgeId).Select(x => x.FirstOrDefault()).ToList(); //to select distinct item

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetBridgeImprovementIndicatorBySection(string sectionid)
        {
            // Get all bridges in the current section and join with bridge condition by inspection year results
            var result = (from br in db.Bridges
                         join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                         from seg in table1.ToList()
                         join cond in db.BridgeConditionByInspectionYears on br.BridgeId equals cond.BridgeId into table2
                         from cond in table2.ToList()
                         where seg.SectionId == sectionid
                         select  new BridgeImprovementIndicatorViewModel
                         {
                             bridges = br,
                             segments = seg,
                             bridgeConditionByInspYear = cond
                         }).GroupBy(x => x.bridges.BridgeId).Select(x => x.FirstOrDefault()).ToList(); //to select distinct item

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetBridgeImprovementIndicatorBySegment(string segmentid)
        {
            // Get all bridges in the current segment
            List<Bridge> bridgeList = (from br in db.Bridges
                                       where br.SegmentId == segmentid
                                       select br).ToList();

            // Get all bridge condition by inspection year results
            List<BridgeConditionByInspectionYear> brgCondByInspYear = db.BridgeConditionByInspectionYears.ToList();

            // Get all bridges in the given district
            var result = (from br in bridgeList
                          join cond in brgCondByInspYear on br.BridgeId equals cond.BridgeId into table1
                          from cond in table1.ToList()
                          select new BridgeImprovementIndicatorViewModel
                          {
                              bridges = br,
                              bridgeConditionByInspYear = cond
                          }).GroupBy(x => x.bridges.BridgeId).Select(x => x.FirstOrDefault()).ToList(); //to select distinct item

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetConditionAndRepairCostSummary(int inspectionyear)
        {
            List<BridgeConditionAndRepairCostByDistrict> brgCondRepairCostByDistrict = db.BridgeConditionAndRepairCostByDistricts.Where(s => s.InspectionYear == inspectionyear).ToList();
            ViewBag.InspectionYear = inspectionyear;

            return PartialView(brgCondRepairCostByDistrict);
        }

        public ActionResult _GetConditionAndRepairCostSummaryByDistrict(string districtid, int inspectionyear)
        {
            List<BridgeConditionAndRepairCostBySection> brgCondRepairCostBySection = db.BridgeConditionAndRepairCostBySections.Where(s => s.InspectionYear == inspectionyear
                                                                                            && s.District == districtid).ToList();
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();
            
            return PartialView(brgCondRepairCostBySection);
        }

        public ActionResult _GetConditionAndRepairCostSummaryBySection(string sectionid, int inspectionyear)
        {
            List<BridgeConditionAndRepairCostBySegment> brgCondRepairCostBySegment = db.BridgeConditionAndRepairCostBySegments.Where(s => s.InspectionYear == inspectionyear
                                                                                            && s.Section == sectionid).ToList();
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();
            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();

            return PartialView(brgCondRepairCostBySegment);
        }

        //Return a list of DamageInspMajor for the given bridge id, inspection year and structure item id
        public List<DamageInspMajor> damageInspMajorList(string bridgeid, int inspectionyear, int structItemId)
        {
            return (from s in db.DamageInspMajors
                    where s.BridgeId == bridgeid && s.InspectionYear == inspectionyear && s.StructureItemId == structItemId
                    select s).ToList();
        }

        //Return a "Damage Type" list for the given structure item id
        public List<string[]> GetDamageTypeList(int structItemId)
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == structItemId).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeId.ToString(), d.DamageTypeName });
            //var list = damageTypes.Select(d => d.DamageTypeName).ToList();

            return list.ToList();
        }

        //Return a list of Damage Ranks (A/A+, B, C, D)
        public List<string> GetDamageRankList()
        {
            var damageRankList = db.DamageRanks.Select(r => r.DamageRankName).ToList();
            //var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });
            var updatedList = damageRankList.ToList();

            // Change the list ("A+", "A", "B", "C", "D") to ("A/A+", "A", "B", "C")
            //updatedList.Remove("A");
            updatedList.Remove("D");
            //updatedList[updatedList.FindIndex(i => i.Equals("A+"))] = "A/A+";

            return updatedList;
        }

        public List<List<DamageInspMajor>> SubstructureInspectionDetails(string bridgeid, int inspectionyear)
        {
            // Get all damage inspection results in the current inspection year
            List<DamageInspMajor> damageInspMajorList = db.DamageInspMajors.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();

            // Get a refined list of damage inspection results for each structure item in Substructure
            List<DamageInspMajor> pierAndFoundationList = damageInspMajorList.Where(s => s.StructureItemId == 1).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> abutmentAndWingWallList = damageInspMajorList.Where(s => s.StructureItemId == 2).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> embankmentList = damageInspMajorList.Where(s => s.StructureItemId == 3).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> ripRapList = damageInspMajorList.Where(s => s.StructureItemId == 4).OrderBy(s => s.DamageTypeId).ToList();

            // Put the lists into Substructure bridge part
            List<List<DamageInspMajor>> substructureList = new List<List<DamageInspMajor>>
            {
                pierAndFoundationList,
                abutmentAndWingWallList,
                embankmentList,
                ripRapList
            };

            // Empty the substructure list if it is composed of empty structure item lists
            bool isEmpty = true;
            for (int i = 0; i < substructureList.Count; i++)
            {
                if (substructureList[i].Count != 0)
                    isEmpty = false;
            }

            if (isEmpty)
                substructureList.Clear();

            return substructureList;
        }

        public List<List<DamageInspMajor>> SuperstructureInspectionDetails(string bridgeid, int inspectionyear)
        {
            // Get all damage inspection results in the current inspection year
            List<DamageInspMajor> damageInspMajorList = db.DamageInspMajors.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();

            // Get a refined list of damage inspection results for each structure item in Superstructures
            List<DamageInspMajor> deckSlabList = damageInspMajorList.Where(s => s.StructureItemId == 5).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> concreteGirderList = damageInspMajorList.Where(s => s.StructureItemId == 6).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> steelTrussGirderList = damageInspMajorList.Where(s => s.StructureItemId == 7).OrderBy(s => s.DamageTypeId).ToList();

            // Put the lists into Superstructure bridge part list
            List<List<DamageInspMajor>> superstructureList = new List<List<DamageInspMajor>>
            {
                deckSlabList,
                concreteGirderList,
                steelTrussGirderList
            };

            // Empty the superstructure list if it is composed of empty structure item lists
            bool isEmpty = true;
            for (int i = 0; i < superstructureList.Count; i++)
            {
                if (superstructureList[i].Count != 0)
                    isEmpty = false;
            }

            if (isEmpty)
                superstructureList.Clear();

            return superstructureList;
        }

        public List<List<DamageInspMajor>> AncillariesInspectionDetails(string bridgeid, int inspectionyear)
        {
            // Get all damage inspection results in the current inspection year
            List<DamageInspMajor> damageInspMajorList = db.DamageInspMajors.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();

            // Get a refined list of damage inspection results for each structure item in Ancillaries
            List<DamageInspMajor> pavementList = damageInspMajorList.Where(s => s.StructureItemId == 8).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> curbAndRailingList = damageInspMajorList.Where(s => s.StructureItemId == 9).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> drainageList = damageInspMajorList.Where(s => s.StructureItemId == 10).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> bearingList = damageInspMajorList.Where(s => s.StructureItemId == 11).OrderBy(s => s.DamageTypeId).ToList();
            List<DamageInspMajor> expansionJointList = damageInspMajorList.Where(s => s.StructureItemId == 12).OrderBy(s => s.DamageTypeId).ToList();

            // Put the lists into Ancillaries bridge part list
            List<List<DamageInspMajor>> ancillariesList = new List<List<DamageInspMajor>>
            {
                pavementList,
                curbAndRailingList,
                drainageList,
                bearingList,
                expansionJointList
            };

            // Empty the anciliaries list if it is composed of empty structure item lists
            bool isEmpty = true;
            for (int i = 0; i < ancillariesList.Count; i++)
            {
                if (ancillariesList[i].Count != 0)
                    isEmpty = false;
            }

            if (isEmpty)
                ancillariesList.Clear();

            return ancillariesList;
        }

        public List<List<DamageRateAndCost>> SubstructureDamageRateAndCost()
        {
            // Since unit repair cost is the same for ranks, just take one rank only (say A+)
            //var dmgRate = db.DamageRateAndCosts.Where(d => d.DamageRankId == 1).ToList();
            var dmgRate = db.DamageRateAndCosts.ToList();

            // Get damage rate and cost for each structure item in substructures
            List<DamageRateAndCost> dmgRatePierAndFoundation = dmgRate.Where(d => d.StructureItemId == 1).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateAbutmentAndWingwall = dmgRate.Where(d => d.StructureItemId == 2).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateEmbankment = dmgRate.Where(d => d.StructureItemId == 3).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateRipRap = dmgRate.Where(d => d.StructureItemId == 4).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();

            // Put the unit repair cost lists into Substructure bridge part list
            List<List<DamageRateAndCost>> damageRateSubstructure = new List<List<DamageRateAndCost>>
            {
                dmgRatePierAndFoundation,
                dmgRateAbutmentAndWingwall,
                dmgRateEmbankment,
                dmgRateRipRap
             };

            return damageRateSubstructure;
        }

        public List<List<DamageRateAndCost>> SuperstructureDamageRateAndCost()
        {
            // Since unit repair cost is the same for ranks, just take one rank only (say A+)
            //var dmgRate = db.DamageRateAndCosts.Where(d => d.DamageRankId == 1).ToList();
            var dmgRate = db.DamageRateAndCosts.ToList();

            // Get damage rate and cost for each structure item in Superstructures
            List<DamageRateAndCost> dmgRateDeckSlab = dmgRate.Where(d => d.StructureItemId == 5).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateConcreteGirder = dmgRate.Where(d => d.StructureItemId == 6).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateSteelTrussGirder = dmgRate.Where(d => d.StructureItemId == 7).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();

            // Put the unit repair cost lists into Superstructure bridge part list
            List<List<DamageRateAndCost>> damageRateSuperstructure = new List<List<DamageRateAndCost>>
            {
                dmgRateDeckSlab,
                dmgRateConcreteGirder,
                dmgRateSteelTrussGirder
             };

            return damageRateSuperstructure;
        }

        public List<List<DamageRateAndCost>> AncillariesDamageRateAndCost()
        {
            // Since unit repair cost is the same for ranks, just take one rank only (say A+)
            //var dmgRate = db.DamageRateAndCosts.Where(d => d.DamageRankId == 1).ToList();
            var dmgRate = db.DamageRateAndCosts.ToList();

            // Get damage rate and cost for each structure item in Ancillaries
            List<DamageRateAndCost> dmgRatePavement = dmgRate.Where(d => d.StructureItemId == 8).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateCurbRailings = dmgRate.Where(d => d.StructureItemId == 9).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateDrainage = dmgRate.Where(d => d.StructureItemId == 10).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateBearing = dmgRate.Where(d => d.StructureItemId == 11).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();
            List<DamageRateAndCost> dmgRateExpansionJoint = dmgRate.Where(d => d.StructureItemId == 12).OrderBy(d => d.DamageTypeId).ThenBy(d => d.DamageRankId).ToList();

            // Put the unit repair cost lists into Ancillaries bridge part list
            List<List<DamageRateAndCost>> damageRateAncillaries = new List<List<DamageRateAndCost>>
            {
                dmgRatePavement,
                dmgRateCurbRailings,
                dmgRateDrainage,
                dmgRateBearing,
                dmgRateExpansionJoint
            };

            return damageRateAncillaries;
        }

        public ActionResult _GetMajorInspectionDetails(string bridgeid, int inspectionyear /* drop down values */)
        {
            var segmentid = db.Bridges.Find(bridgeid).SegmentId;

            ViewBag.BridgeName = db.Bridges.Find(bridgeid).BridgeName;
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;
            //ViewBag.InspectionDate = pierAndFoundationList[0].InspectionDate;
            //ViewBag.InspMajorBridgeParts = inspMajorBridgePartsViewModel;
            ViewBag.SubstructureInspection = SubstructureInspectionDetails(bridgeid, inspectionyear);
            ViewBag.SuperstructureInspection = SuperstructureInspectionDetails(bridgeid, inspectionyear);
            ViewBag.AncillariesInspection = AncillariesInspectionDetails(bridgeid, inspectionyear);
            ViewBag.DamageRankList = GetDamageRankList();
            ViewBag.DamageRateSubstructure = SubstructureDamageRateAndCost();
            ViewBag.DamageRateSuperstructure = SuperstructureDamageRateAndCost();
            ViewBag.DamageRateAncillaries = AncillariesDamageRateAndCost();

            // GirderType == 1 for Concrete and GirderType == 2 for Steel
            int girderTypeId = db.SuperStructures.Find(bridgeid).GirderType.GirderTypeId;
            var strItemDmgWt = (girderTypeId == 1) ? db.StructureItemDmgWts.Select(s => s.ConcreteDmgWt).ToList() : db.StructureItemDmgWts.Select(s => s.SteelDmgWt).ToList();
            ViewBag.GirderTypeId = girderTypeId;

            // Get structure item damage weights for each bridge part
            ViewBag.SubstructureStrItemDmgWt = strItemDmgWt.GetRange(0, 4); // Structure items 1,2,3,4 [Substructures]
            ViewBag.SuperstructureStrItemDmgWt = strItemDmgWt.GetRange(4, 3); // Structure items 5,6,7 [Superstructures]
            ViewBag.AncillariesStrItemDmgWt = strItemDmgWt.GetRange(7, 5); // Structure items 8,9,10,11,12 [Ancillaries]

            ViewBag.BrgPartDmgWt = db.BridgePartDmgWts.Select(r => r.DmgWt).ToList();

            ViewBag.Comments = db.BridgeComments.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).Select(s => s.Comment).ToList();

            BridgeObservation obs = db.BridgeObservations.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).FirstOrDefault();

            if (obs != null)
            {
                ViewBag.InspectorRecommendation = obs.MaintenanceUrgency.UrgencyName;
                ViewBag.WaterWayAdequacy = obs.WaterWayAdequacy.Value;
            }

            return PartialView();
        }

        //public ActionResult _GetMajorInspectionDetails(string bridgeid, int inspectionyear /* drop down values */)
        //{
        //    var segmentid = db.Bridges.Find(bridgeid).SegmentId;

        //    ViewBag.BridgeName = db.Bridges.Find(bridgeid).BridgeName;
        //    ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
        //    ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
        //    ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
        //    ViewBag.InspectionYear = inspectionyear;
        //    //ViewBag.InspectionDate = pierAndFoundationList[0].InspectionDate;
        //    //ViewBag.InspMajorBridgeParts = inspMajorBridgePartsViewModel;
        //    ViewBag.SubstructureInspection = SubstructureInspectionDetails(bridgeid, inspectionyear);
        //    ViewBag.SuperstructureInspection = SuperstructureInspectionDetails(bridgeid, inspectionyear);
        //    ViewBag.AncillariesInspection = AncillariesInspectionDetails(bridgeid, inspectionyear);
        //    ViewBag.DamageRankList = GetDamageRankList();
        //    ViewBag.DamageRateSubstructure = SubstructureDamageRateAndCost();
        //    ViewBag.DamageRateSuperstructure = SuperstructureDamageRateAndCost();
        //    ViewBag.DamageRateAncillaries = AncillariesDamageRateAndCost();

        //    // GirderType == 1 for Steel and GirderType == 2 for Concrete
        //    int girderType = db.SuperStructures.Find(bridgeid).GirderType.GirderTypeId;
        //    var strItemDmgWt = (girderType == 1) ? db.StructureItemDmgWts.Select(s => s.ConcreteDmgWt).ToList() : db.StructureItemDmgWts.Select(s => s.SteelDmgWt).ToList();

        //    // Get structure item damage weights for each bridge part
        //    ViewBag.SubstructureStrItemDmgWt = strItemDmgWt.GetRange(0, 4); // Structure items 1,2,3,4 [Substructures]
        //    ViewBag.SuperstructureStrItemDmgWt = strItemDmgWt.GetRange(4, 3); // Structure items 5,6,7 [Superstructures]
        //    ViewBag.AncillariesStrItemDmgWt = strItemDmgWt.GetRange(7, 5); // Structure items 8,9,10,11,12 [Ancillaries]

        //    ViewBag.BrgPartDmgWt = db.BridgePartDmgWts.Select(r => r.DmgWt).ToList();

        //    ViewBag.Comments = db.BridgeComments.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).Select(s => s.Comment).ToList();

        //    BridgeObservation obs = db.BridgeObservations.Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).FirstOrDefault();

        //    if (obs != null)
        //    {
        //        ViewBag.InspectorRecommendation = obs.MaintenanceUrgency.UrgencyName;
        //        ViewBag.WaterWayAdequacy = obs.WaterWayAdequacy.Value;
        //    }

        //    return PartialView();
        //}

        public ActionResult _GetBillOfQuantity(string bridgeid, int inspectionyear /* drop down values */)
        {
            var segmentid = db.Bridges.Find(bridgeid).SegmentId;

            ViewBag.BridgeName = db.Bridges.Find(bridgeid).BridgeName;
            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            ViewBag.InspectionYear = inspectionyear;
            //ViewBag.InspectionDate = pierAndFoundationList[0].InspectionDate;
            //ViewBag.InspMajorBridgeParts = inspMajorBridgePartsViewModel;
            ViewBag.Substructure = SubstructureInspectionDetails(bridgeid, inspectionyear);
            ViewBag.Superstructure = SuperstructureInspectionDetails(bridgeid, inspectionyear);
            ViewBag.Ancillaries = AncillariesInspectionDetails(bridgeid, inspectionyear);
            ViewBag.DamageRankList = GetDamageRankList();
            ViewBag.DamageRateSubstructure = SubstructureDamageRateAndCost();
            ViewBag.DamageRateSuperstructure = SuperstructureDamageRateAndCost();
            ViewBag.DamageRateAncillaries = AncillariesDamageRateAndCost();

            // GirderType == 1 for Steel and GirderType == 2 for Concrete
            int girderType = db.SuperStructures.Find(bridgeid).GirderType.GirderTypeId;
            var strItemDmgWt = (girderType == 1) ? db.StructureItemDmgWts.Select(s => s.ConcreteDmgWt).ToList() : db.StructureItemDmgWts.Select(s => s.SteelDmgWt).ToList();

            // Get structure item damage weights for each bridge part
            ViewBag.StrItemSubStructureDmgWt = strItemDmgWt.GetRange(0, 4); // Structure items 1,2,3,4 [Substructures]
            ViewBag.StrItemSuperStructureDmgWt = strItemDmgWt.GetRange(4, 3); // Structure items 5,6,7 [Superstructures]
            ViewBag.StrItemAncillariesDmgWt = strItemDmgWt.GetRange(7, 5); // Structure items 8,9,10,11,12 [Ancillaries]

            ViewBag.BrgPartDmgWt = db.BridgePartDmgWts.Select(r => r.DmgWt).ToList();

            return PartialView();
        }

        public ActionResult BillOfQuantity()
        {
            return View();
        }
    }
}