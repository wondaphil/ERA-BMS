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
    public class BridgeImprovementController : Controller
    {
        private BMSEntities db = new BMSEntities();
        private static DateTime globalInspectionDate = DateTime.Now;

        // GET: BridgeImprovement
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(string id = "")
        {
            Bridge bridge = db.Bridges.Find(id);
            var brgImprov = db.BridgeImprovements.Where(d => d.BridgeId == id);

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == "" || bridge == null)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Bridges = new List<SelectListItem>(); // For an empty bridges dropdownlist

                return View();
            }
            else
            {
                // For a dropdownlist that has a list of bridges in the current segment and the current bridge selected
                ViewBag.Bridges = LocationModel.GetBridgeNameList(bridge.SegmentId);
                TempData["BridgeId"] = id;

                // For a dropdownlist that has a list of segments in the current section and the current segment selected
                ViewBag.Segments = LocationModel.GetSegmentList(bridge.Segment.SectionId);
                ViewBag.SegmentId = bridge.Segment.SegmentId;

                // For a dropdownlist that has a list of sections in the current district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(bridge.Segment.Section.DistrictId);
                ViewBag.SectionId = bridge.Segment.SectionId;

                // For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = bridge.Segment.Section.DistrictId;

                // For displaying district, section, and segment names and bridge no. on reports
                ViewBag.DistrictName = bridge.Segment.Section.District.DistrictName;
                ViewBag.SectionName = bridge.Segment.Section.SectionName;
                ViewBag.SegmentName = bridge.Segment.SegmentName;
                ViewBag.BridgeName = bridge.BridgeName;

                TempData["ImrpovementTypeId"] = new SelectList(db.ImprovementTypes, "ImprovementTypeId", "ImprovementTypeName").ToList();
                TempData["ImprovementTypeList"] = (from c in db.ImprovementTypes
                                                   select new
                                                   {
                                                       Value = c.ImprovementTypeId,
                                                       Text = c.ImprovementTypeName
                                                   }).ToList();

                return View(brgImprov);
            }
        }

        // GET
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult _GetBridgeImprovement([Optional] string bridgeid)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<BridgeImprovement> brgImprovementList = db.BridgeImprovements.Where(s => s.BridgeId == bridgeid).ToList();

            List<SelectListItem> imprTypeSelectList = db.ImprovementTypes.Select(c => new SelectListItem
                                                    { Text = c.ImprovementTypeName, Value = c.ImprovementTypeId.ToString() }).ToList();


            //ViewBag.ImprovementTypeSelectList = imprTypeSelectList; // for improvement activity type dropdown list
            TempData["ImrpovementTypeId"] = imprTypeSelectList; // for improvement activity type dropdown list
            TempData["BridgeId"] = bridgeid;

            TempData["ImprovementTypeList"] = (from c in db.ImprovementTypes
                                    select new
                                    {
                                        Value = c.ImprovementTypeId,
                                        Text = c.ImprovementTypeName
                                    }).ToList();

            
            return PartialView(brgImprovementList);
        }

        [HttpPost]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public JsonResult InsertBridgeImprovement(BridgeImprovement brgImprovement)
        {
            brgImprovement.Id = Guid.NewGuid().ToString();
            db.BridgeImprovements.Add(brgImprovement);
            db.SaveChanges();

            return Json(brgImprovement);
        }

        [HttpPost]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult UpdateBridgeImprovement(BridgeImprovement brgImprovement)
        {
            BridgeImprovement updatedBrgImprovement = (from c in db.BridgeImprovements
                                                       where c.Id == brgImprovement.Id
                                                       select c).FirstOrDefault();

            updatedBrgImprovement.ImprovementDate = (brgImprovement.ImprovementDate != null) ? brgImprovement.ImprovementDate : globalInspectionDate;
            updatedBrgImprovement.ImprovementYear = brgImprovement.ImprovementYear;
            updatedBrgImprovement.ImprovementTypeId = brgImprovement.ImprovementTypeId;
            updatedBrgImprovement.Contractor = brgImprovement.Contractor;
            updatedBrgImprovement.Supervisor = brgImprovement.Supervisor;
            updatedBrgImprovement.ImprovementCost = brgImprovement.ImprovementCost;
            updatedBrgImprovement.ImprovementAction = brgImprovement.ImprovementAction;
            
            db.SaveChanges();

            return new EmptyResult();
        }

        //GET
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult GetLastBridgeImprovement(string bridgeid)
        {
            var lastBrgImprovement = (from c in db.BridgeImprovements
                            where c.BridgeId == bridgeid
                            select new
                            {
                                Id = c.Id,
                                BridgeId = c.BridgeId,
                            }).ToList().LastOrDefault();

            return Json(lastBrgImprovement, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult DeleteBridgeImprovement(string id)
        {
            BridgeImprovement brgImprovement = db.BridgeImprovements.Find(id);
            if (brgImprovement != null)
            {
                db.BridgeImprovements.Remove(brgImprovement);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        public ActionResult _GetBridgeImprovementActionRecords(int startyear, int endyear, int improvementtypeid)
        {
            List<BridgeImprovement> brgImproveList;
            
            if (improvementtypeid == 0)
            {
                brgImproveList = db.BridgeImprovements.Where(s => s.ImprovementYear >= startyear && s.ImprovementYear <= endyear).ToList();
            }
            else
            {
                brgImproveList = db.BridgeImprovements.Where(s => s.ImprovementYear >= startyear && s.ImprovementYear <= endyear && s.ImprovementTypeId == improvementtypeid).ToList();
            }

            ViewBag.ImprovementType = (improvementtypeid != 0) ? db.ImprovementTypes.Find(improvementtypeid).ImprovementTypeName.ToString() : "Rehabilitation/Replacement";
            ViewBag.StartImprovementYear = startyear;
            ViewBag.EndImprovementYear = endyear;

            return PartialView(brgImproveList);
        }

        public ActionResult BridgeImprovementActionRecords()
        {
            var improvementyear = (from BrgImrov in db.BridgeImprovements
                                   select new
                                      {
                                          BrgImrov.ImprovementYear
                                      }).Distinct();

            List <SelectListItem> improvementYearSelectList = new List<SelectListItem>();
            List<MajorInspectionYear> inspYears = db.MajorInspectionYears.ToList();

            // Iterate through major inspection year
            foreach (var year in inspYears)
            {
                string txt = year.InspectionYear + " => (" + year.StartYear + " - " + year.EndYear + ")";  // e.g. "2016 => (2014 - 2016)"
                string val = year.StartYear + " - " + year.EndYear;  // e.g. "2014 - 2016"
                improvementYearSelectList.Insert(0, new SelectListItem { Text = txt, Value = val });
            }

            //Add the first unselected item
            improvementYearSelectList.Insert(0, new SelectListItem { Text = "--Select Year Range--", Value = "0" });

            List<SelectListItem> improvementTypeSelectList = db.ImprovementTypes.Select(c => new SelectListItem
                                                                 { Text = c.ImprovementTypeName, Value = c.ImprovementTypeId.ToString()} ).ToList();

            //Add the first unselected item
            improvementTypeSelectList.Insert(0, new SelectListItem { Text = "Rehab. / Replacement", Value = "0" });

            ViewBag.ImprovementYearSelectList = improvementYearSelectList; // for improvement year dropdown list
            ViewBag.ImprovementTypeSelectList = improvementTypeSelectList; // for improvement type dropdown list

            return View();
        }

        public ActionResult PrintImprovement([Optional] string bridgeid /* drop down value */)
        {
            ViewBag.Bridge = db.Bridges.Find(bridgeid);
            return PartialView(db.BridgeImprovements.Where(s => s.BridgeId == bridgeid));
        }

        public ActionResult _GetBridgeImprovementHistoryAll(int improvementtypeid)
        {
            // Get all bridges in all districts
            List<Bridge> bridgeList = (from br in db.Bridges
                                       select br).ToList();

            // Get all improvement histories for the given improvement year and improvement type
            List<BridgeImprovement> brgImprovementList;

            if (improvementtypeid == 0)
            {
                brgImprovementList = db.BridgeImprovements.ToList();
            }
            else
            {
                brgImprovementList = db.BridgeImprovements.Where(s => s.ImprovementTypeId == improvementtypeid).ToList();
            }

            // Get all bridges in the given segment, improvement year and improvement type
            var result = (from imp in brgImprovementList
                          join br in bridgeList on imp.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select imp);

            ViewBag.ImprovementType = (improvementtypeid != 0) ? db.ImprovementTypes.Find(improvementtypeid).ImprovementTypeName.ToString() : "Rehabilitation/Replacement";
            //ViewBag.StartImprovementYear = startyear;
            //ViewBag.EndImprovementYear = endyear;

            return PartialView(result);
        }

        public ActionResult _GetBridgeImprovementHistoryByDistrict(string districtid, int improvementtypeid)
        {
            // Get all bridges in the given district
            List<Bridge> bridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       join dist in db.Districts on sec.DistrictId equals dist.DistrictId into table3
                                       from dist in table3.ToList()
                                       where dist.DistrictId == districtid
                                       select br).ToList();

            // Get all improvement histories for the given improvement year and improvement type
            List<BridgeImprovement> brgImprovementList;
            if (improvementtypeid == 0)
            {
                brgImprovementList = db.BridgeImprovements.ToList();
            }
            else
            {
                brgImprovementList = db.BridgeImprovements.Where(s => s.ImprovementTypeId == improvementtypeid).ToList();
            }

            // Get all bridges in the given segment, improvement year and improvement type
            var result = (from imp in brgImprovementList
                          join br in bridgeList on imp.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select imp);

            ViewBag.ImprovementType = (improvementtypeid != 0) ? db.ImprovementTypes.Find(improvementtypeid).ImprovementTypeName.ToString() : "Rehabilitation/Replacement";
            //ViewBag.StartImprovementYear = startyear;
            //ViewBag.EndImprovementYear = endyear;
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetBridgeImprovementHistoryBySection(string sectionid, int improvementtypeid)
        {
            // Get all bridges in the given section
            List<Bridge> bridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       where sec.SectionId == sectionid
                                       select br).ToList();

            // Get all improvement histories for the given improvement year and improvement type
            List<BridgeImprovement> brgImprovementList;
            if (improvementtypeid == 0)
            {
                brgImprovementList = db.BridgeImprovements.ToList();
            }
            else
            {
                brgImprovementList = db.BridgeImprovements.Where(s => s.ImprovementTypeId == improvementtypeid).ToList();
            }

            // Get all bridges in the given segment, improvement year and improvement type
            var result = (from imp in brgImprovementList
                          join br in bridgeList on imp.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select imp);

            ViewBag.ImprovementType = (improvementtypeid != 0) ? db.ImprovementTypes.Find(improvementtypeid).ImprovementTypeName.ToString() : "Rehabilitation/Replacement";
            //ViewBag.StartImprovementYear = startyear;
            //ViewBag.EndImprovementYear = endyear;
            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetBridgeImprovementHistoryBySegment(string segmentid, int improvementtypeid)
        {
            // Get all bridges in the given segment
            List<Bridge> bridgeList = (from s in db.Bridges
                                       where s.SegmentId == segmentid
                                       select s).ToList();

            // Get all improvement histories for the given improvement year and improvement type
            List<BridgeImprovement> brgImprovementList;
            if (improvementtypeid == 0)
            {
                brgImprovementList = db.BridgeImprovements.ToList();
            }
            else
            {
                brgImprovementList = db.BridgeImprovements.Where(s => s.ImprovementTypeId == improvementtypeid).ToList();
            }

            // Get all bridges in the given segment, improvement year and improvement type
            var result = (from imp in brgImprovementList
                          join br in bridgeList on imp.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select imp);

            ViewBag.ImprovementType = (improvementtypeid != 0) ? db.ImprovementTypes.Find(improvementtypeid).ImprovementTypeName.ToString() : "Rehabilitation/Replacement";
            //ViewBag.StartImprovementYear = startyear;
            //ViewBag.EndImprovementYear = endyear;
            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult BridgeImprovementHistory()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            //var improvementyear = (from BrgImrov in db.BridgeImprovements
            //                       select new
            //                       {
            //                           BrgImrov.ImprovementYear
            //                       }).Distinct();

            //List<SelectListItem> improvementYearSelectList = new List<SelectListItem>();
            //List<MajorInspectionYear> inspYears = db.MajorInspectionYears.ToList();

            //// Iterate through major inspection year
            //foreach (var year in inspYears)
            //{
            //    string txt = year.InspectionYear.ToString(); // e.g. "2016"
            //    string val = year.StartYear + " - " + year.EndYear; // e.g. "2014 - 2016"
            //    improvementYearSelectList.Insert(0, new SelectListItem { Text = txt, Value = val });
            //}

            List<SelectListItem> improvementTypeSelectList = db.ImprovementTypes.Select(c => new SelectListItem
                                { Text = c.ImprovementTypeName, Value = c.ImprovementTypeId.ToString() }).ToList();

            //Add the first unselected item
            improvementTypeSelectList.Insert(0, new SelectListItem { Text = "Rehabilitation/Replacement", Value = "0" });

            //ViewBag.ImprovementYearSelectList = improvementYearSelectList; // for improvement year dropdown list
            ViewBag.ImprovementTypeSelectList = improvementTypeSelectList; // for improvement type dropdown list
            
            return View();
        }

        public ActionResult BridgeImprovementCost()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;

            return View();
        }

        public ActionResult _GetBridgeImprovementCostBySegment(string segmentid)
        {
            // Use the latest inspection year
            int inspectionyear = db.MajorInspectionYears.Max(m => m.InspectionYear);

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
                              damagePercent = (double)dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();
            
            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }

        public ActionResult _GetBridgeImprovementCostBySection(string sectionid)
        {
            // Use the latest inspection year
            int inspectionyear = db.MajorInspectionYears.Max(m => m.InspectionYear);

            // Get all bridges in the given section
            List<Bridge> bridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       where seg.SectionId == sectionid
                                       select br).ToList();

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
                              damagePercent = (double)dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();
            
            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }

        public ActionResult _GetBridgeImprovementCostByDistrict(string districtid)
        {
            // Use the latest inspection year
            int inspectionyear = db.MajorInspectionYears.Max(m => m.InspectionYear);

            // Get all bridges in the given district
            List<Bridge> bridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       where sec.DistrictId == districtid
                                       select br).ToList();

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
                              damagePercent = (double)dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();
            
            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }

        public ActionResult _GetBridgeImprovementCostAll()
        {
            // Use the latest inspection year
            int inspectionyear = db.MajorInspectionYears.Max(m => m.InspectionYear);

            // Get all bridges in the given segment
            List<Bridge> bridgeList = (from s in db.Bridges
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
                              damagePercent = (double)dmg.DmgPerc,
                              totalRepairCost = (double)(dmg.RepairCostSubStruct + dmg.RepairCostSuperStruct + dmg.RepairCostAncillaries)
                          });

            return PartialView(result.OrderByDescending(s => s.damagePercent).ToList());
        }

        public ActionResult BridgesAndCulvertsRepairCostByDistrict()
        {
            // Use the latest inspection year
            int inspectionyear = db.MajorInspectionYears.Max(m => m.InspectionYear);

            // Get all damage inspection results for bridges in the given inspection year
            List<ResultInspMajor> bridgeDmgResultList = db.ResultInspMajors.Where(s => s.InspectionYear == inspectionyear).ToList();

            // Get all damage inspection results for culvert
            List<ResultInspCulvert> culvertDmgResultList = db.ResultInspCulverts.ToList();

            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();
            List<District> districts = db.Districts.ToList();

            // Get bridge improvement cost by district
            var bridgeRepairCost = (from res in bridgeDmgResultList
                                   join br in bridges on res.BridgeId equals br.BridgeId into table1
                                   from br in table1.ToList()
                                   join seg in segments on br.SegmentId equals seg.SegmentId into table2
                                   from seg in table2.ToList()
                                   join sec in sections on seg.SectionId equals sec.SectionId into table3
                                   from sec in table3.ToList()
                                   join dis in districts on sec.DistrictId equals dis.DistrictId into table4
                                   from dis in table4.ToList()
                                   select new
                                   {
                                       District = dis,
                                       BridgeImprovementCost = (res.RepairCostSubStruct.Value + res.RepairCostSuperStruct.Value + res.RepairCostAncillaries.Value) 
                                   }).ToList();

            var bridgeRepairCostbydistrict = from imp in bridgeRepairCost
                                             group imp by imp.District into g
                                             select new
                                             {
                                                 District = g.Key,
                                                 BridgeCount = g.Count(),
                                                 BridgeImprovementCost = g.Sum(c => c.BridgeImprovementCost)
                                             };

            // Get culvert improvement cost by district
            var culvertRepairCost = (from res in culvertDmgResultList
                                    join cul in culverts on res.CulvertId equals cul.CulvertId into table1
                                    from cul in table1.ToList()
                                    join seg in segments on cul.SegmentId equals seg.SegmentId into table2
                                    from seg in table2.ToList()
                                    join sec in sections on seg.SectionId equals sec.SectionId into table3
                                    from sec in table3.ToList()
                                    join dis in districts on sec.DistrictId equals dis.DistrictId into table4
                                    from dis in table4.ToList()
                                    select new
                                    {
                                        District = dis,
                                        CulvertImprovementCost = res.RepairCost
                                    }).ToList();

            var culvertRepairCostbydistrict = from imp in culvertRepairCost
                                              group imp by imp.District into g
                                              select new
                                              {
                                                  District = g.Key,
                                                  CulvertCount = g.Count(),
                                                  CulvertImprovementCost = g.Sum(c => c.CulvertImprovementCost)
                                              };
            
            // Join bridge count by district and culvert improvement cost by district
            var noofbridgesandculvertsbydistrict = (from br in bridgeRepairCostbydistrict
                                                    join cul in culvertRepairCostbydistrict on br.District equals cul.District into table1
                                                    from cul in table1.ToList()
                                                    select new BridgeCulvertDistrictSectionSegmentViewModel
                                                    {
                                                        Districts = br.District,
                                                        BridgeCount = br.BridgeCount,
                                                        BridgeImprovementCost = br.BridgeImprovementCost,
                                                        CulvertCount = cul.CulvertCount,
                                                        CulvertImprovementCost = (double) cul.CulvertImprovementCost
                                                    }).ToList();

            return View(noofbridgesandculvertsbydistrict);
        }
    }
}