using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    //Only logged in user should have access
    [Authorize(Roles = "Admin, User")]
    public class DamageInspMajorController : Controller
    {
        private BMSEntities db = new BMSEntities();
        
        // GET: DamageInspMajor
        //public ActionResult Index(int? year, string id = "")
        public ActionResult Index(string id, int? val) // id = BridgeId, val = InspectionYear
        {
            List<SelectListItem> inspectionYearList = (from inspYears in db.MajorInspectionYears
                                                       orderby inspYears.InspectionYear descending
                                                       select new
                                                       {
                                                           inspYears.InspectionYear
                                                       }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
                                                       {
                                                           Text = c.InspectionYear.ToString(),
                                                           Value = c.InspectionYear.ToString()
                                                       }
                                                            ).ToList();

            //List<SelectListItem> inspectionYearList = (from inspYears in db.MajorInspectionYears
            //                                           select new
            //                                           {
            //                                               inspYears.InspectionYear).ToList();
            //                                            }).Distinct().OrderByDescending(s => s.InspectionYear).Select(c => new SelectListItem
            //                                            {
            //                                                Text = c.InspectionYear.ToString(),
            //                                                Value = c.InspectionYear.ToString()
            //                                            }
            //                                    ).ToList();

            // If inspection year is given, take that. Otherwise, take the last inspection year
            var lastInspectionYear = Int32.Parse(inspectionYearList.Select(s => s.Value).Max()); // last inpection year is the max in the year list
            var inspectionyear = (val != null) ? val : lastInspectionYear;

            ViewBag.InspectionYearList = inspectionYearList;
            ViewBag.InspectionYear = inspectionyear;
            
            Bridge bridge = db.Bridges.Find(id);
            var dmgInspMajor = db.DamageInspMajors.Where(d => d.BridgeId == id && d.InspectionYear == inspectionyear);

            InspMajorSubStructuresViewModel subStr = new InspMajorSubStructuresViewModel()
            {
                PierAndFoundation = dmgInspMajor.Where(d => d.StructureItemId == 1).ToList(),
                AbutmentAndWingWall = dmgInspMajor.Where(d => d.StructureItemId == 2).ToList(),
                Embankment = dmgInspMajor.Where(d => d.StructureItemId == 3).ToList(),
                RipRap = dmgInspMajor.Where(d => d.StructureItemId == 4).ToList()
            };
            InspMajorSuperStructuresViewModel supStr = new InspMajorSuperStructuresViewModel()
            {
                DeckSlab = dmgInspMajor.Where(d => d.StructureItemId == 5).ToList(),
                ConcreteGirder = dmgInspMajor.Where(d => d.StructureItemId == 6).ToList(),
                SteelTrussGirder = dmgInspMajor.Where(d => d.StructureItemId == 7).ToList()
            };
            InspMajorAncillariesViewModel ancill = new InspMajorAncillariesViewModel()
            {
                Pavement = dmgInspMajor.Where(d => d.StructureItemId == 8).ToList(),
                CurbAndRailing = dmgInspMajor.Where(d => d.StructureItemId == 9).ToList(),
                Drainage = dmgInspMajor.Where(d => d.StructureItemId == 10).ToList(),
                Bearing = dmgInspMajor.Where(d => d.StructureItemId == 11).ToList(),
                ExpansionJoint = dmgInspMajor.Where(d => d.StructureItemId == 12).ToList()
            };
            BridgeObservation obs = db.BridgeObservations.Where(ob => ob.BridgeId == id && ob.InspectionYear == inspectionyear).FirstOrDefault();
            List<BridgeComment> comm = db.BridgeComments.Where(c => c.BridgeId == id && c.InspectionYear == inspectionyear).ToList();

            MajorInspectionViewModel majorinspection = new MajorInspectionViewModel()
            {
                SubStructure = subStr,
                SuperStructure = supStr,
                Ancillaries = ancill,
                Observations = obs,
                Comments = comm
            };

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

                TempData["InspectionYear"] = inspectionyear;
                TempData["InspectionDate"] = DateTime.Now.ToString("yyyy-MM-dd");

                //For sub structure
                TempData["PierDamageTypeList"] = GetDamageTypeList(1);
                TempData["AbutmentDamageTypeList"] = GetDamageTypeList(2);
                TempData["EmbankmentDamageTypeList"] = GetDamageTypeList(3);
                TempData["RipRapDamageTypeList"] = GetDamageTypeList(4);
                
                //For super structure
                TempData["DeckSlabDamageTypeList"] = GetDamageTypeList(5);
                TempData["ConcreteGirderDamageTypeList"] = GetDamageTypeList(6);
                TempData["SteelTrussGirderDamageTypeList"] = GetDamageTypeList(7);
                
                //For ancillaries
                TempData["PavementDamageTypeList"] = GetDamageTypeList(8);
                TempData["CurbAndRailingDamageTypeList"] = GetDamageTypeList(9);
                TempData["DrainageDamageTypeList"] = GetDamageTypeList(10);
                TempData["BearingDamageTypeList"] = GetDamageTypeList(11);
                TempData["ExpansionJointDamageTypeList"] = GetDamageTypeList(12);
                
                TempData["DamageRankList"] = GetDamageRankList();

                //For observation
                
                TempData["Comment"] = comm.ToList();
                List<SelectListItem> recommendationSelectList = db.MaintenanceUrgencies.Select(c => new SelectListItem
                                    { Text = c.UrgencyId.ToString() + " - " + c.UrgencyName, Value = c.UrgencyId.ToString() }).ToList();

                //Add the first unselected item
                recommendationSelectList.Insert(0, new SelectListItem { Text = "-- No Intervention Selected --", Value = "-1" });
                
                if (obs != null)
                {
                    foreach (var item in recommendationSelectList)
                    {
                        if (item.Value == obs.UrgencyId.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                    TempData["WaterWayAdequacy"] = obs.WaterWayAdequacy;
                }
                else
                {
                    // Select the first option as default
                    recommendationSelectList[0].Selected = true;
                    //TempData["WaterWayAdequacy"] = false;
                }
                TempData["MaintenanceUrgencyList"] = recommendationSelectList; // new SelectList(db.MaintenanceUrgencies, "UrgencyId", "UrgencyName").ToList();

                return View(majorinspection);
            }
        }

        //Return a list of "Major Damage Inspections" for the given structure item id
        public List<DamageInspMajor> GetDamageInspMajorList(int structItemId)
        {
            return (from s in db.DamageInspMajors
                    where s.StructureItemId == structItemId
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

        //Return a list of Damage Ranks (A+, A, B, C, D)
        public List<string> GetDamageRankList()
        {
            List<DamageRank> damageRanks = db.DamageRanks.ToList();
            
            var list = damageRanks.Select(r => r.DamageRankName).ToList();
            list.Remove("D"); // Rank D is not needed  but it is still in the database

            return list;
        }

        public ActionResult SubStructure([Optional] string bridgeid, [Optional] int inspectionyear, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<DamageInspMajor> pierAndFoundationList = GetDamageInspMajorList(1).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> abutmentAndWingWallList = GetDamageInspMajorList(2).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> embankmentList = GetDamageInspMajorList(3).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> ripRapList = GetDamageInspMajorList(4).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            
            if (pierAndFoundationList == null || abutmentAndWingWallList == null || embankmentList == null || ripRapList == null)
            {
                return HttpNotFound();
            }

            var subStructViewModel = new InspMajorSubStructuresViewModel
            {
                PierAndFoundation = pierAndFoundationList,
                AbutmentAndWingWall = abutmentAndWingWallList,
                Embankment = embankmentList,
                RipRap = ripRapList
            };

            TempData["BridgeId"] = bridgeid;
            TempData["InspectionYear"] = inspectionyear;
            TempData["InspectionDate"] = inspectiondate;

            TempData["PierDamageTypeList"] = GetDamageTypeList(1);
            TempData["AbutmentDamageTypeList"] = GetDamageTypeList(2);
            TempData["EmbankmentDamageTypeList"] = GetDamageTypeList(3);
            TempData["RipRapDamageTypeList"] = GetDamageTypeList(4);

            TempData["DamageRankList"] = GetDamageRankList();

            return PartialView(subStructViewModel);
        }

        public ActionResult SuperStructure([Optional] string bridgeid, [Optional] int inspectionyear, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<DamageInspMajor> deckSlabList = GetDamageInspMajorList(5).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> concreteGirderList = GetDamageInspMajorList(6).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> steelTrussGirderList = GetDamageInspMajorList(7).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            
            if (deckSlabList == null || concreteGirderList == null || steelTrussGirderList == null)
            {
                return HttpNotFound();
            }

            var superStructViewModel = new InspMajorSuperStructuresViewModel
            {
                DeckSlab = deckSlabList,
                ConcreteGirder = concreteGirderList,
                SteelTrussGirder = steelTrussGirderList,
            };

            TempData["BridgeId"] = bridgeid;
            TempData["InspectionYear"] = inspectionyear;
            TempData["InspectionDate"] = inspectiondate;
            
            TempData["DeckSlabDamageTypeList"] = GetDamageTypeList(5);
            TempData["ConcreteGirderDamageTypeList"] = GetDamageTypeList(6);
            TempData["SteelTrussGirderDamageTypeList"] = GetDamageTypeList(7);

            TempData["DamageRankList"] = GetDamageRankList();

            return PartialView(superStructViewModel);
        }

        public ActionResult Ancillaries([Optional] string bridgeid, [Optional] int inspectionyear, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<DamageInspMajor> pavementList = GetDamageInspMajorList(8).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> curbAndRailingList = GetDamageInspMajorList(9).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> drainageList = GetDamageInspMajorList(10).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> bearingList = GetDamageInspMajorList(11).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();
            List<DamageInspMajor> expansionJointList = GetDamageInspMajorList(12).Where(s => s.BridgeId == bridgeid && s.InspectionYear == inspectionyear).ToList();

            if (pavementList == null || curbAndRailingList == null || drainageList == null || bearingList == null || expansionJointList == null)
            {
                return HttpNotFound();
            }

            var ancillariesViewModel = new InspMajorAncillariesViewModel
            {
                Pavement = pavementList,
                CurbAndRailing = curbAndRailingList,
                Drainage = drainageList,
                Bearing = bearingList,
                ExpansionJoint = expansionJointList
            };

            TempData["BridgeId"] = bridgeid;
            TempData["InspectionYear"] = inspectionyear;
            TempData["InspectionDate"] = inspectiondate;

            TempData["PavementDamageTypeList"] = GetDamageTypeList(8);
            TempData["CurbAndRailingDamageTypeList"] = GetDamageTypeList(9);
            TempData["DrainageDamageTypeList"] = GetDamageTypeList(10);
            TempData["BearingDamageTypeList"] = GetDamageTypeList(11);
            TempData["ExpansionJointDamageTypeList"] = GetDamageTypeList(12);

            TempData["DamageRankList"] = GetDamageRankList();

            return PartialView(ancillariesViewModel);
        }

        public void UpdateInspectionResult(string bridgeid, int inspectionyear, DateTime inspectiondate)
        {
            ResultInspMajor resInspMajor = db.ResultInspMajors.Where(c => c.BridgeId == bridgeid && c.InspectionYear == inspectionyear).FirstOrDefault();
            List<DamageInspMajor> dmgInspMajor = db.DamageInspMajors.Where(c => c.BridgeId == bridgeid && c.InspectionYear == inspectionyear).ToList();

            // If there is NO major inspection data for the given bridgeid and inspectionyear 
            // but there is a result data, then just DELETE the result data from the database!
            if (dmgInspMajor.Count == 0 && resInspMajor != null)
            {
                db.ResultInspMajors.Remove(resInspMajor);
                db.SaveChanges();
                return;
            }

            DamageInspectionModel inspection = new DamageInspectionModel(bridgeid, inspectionyear);

            double subStrDamageRate = inspection.CalculateBridgePartDamageRate(1);
            double superStrDamageRate = inspection.CalculateBridgePartDamageRate(2);
            double ancillariesDamageRate = inspection.CalculateBridgePartDamageRate(3);
            
            //// Damage percent of the entire bridge is taken as the maximum of the three bridge parts
            //double totDamageRate = (new List<double> { subStrDamageRate, superStrDamageRate, ancillariesDamageRate }).Max();
            
            // Damage percent of the entire bridge is taken as the sum of the damage percent of three bridge parts
            double totDamageRate = subStrDamageRate + superStrDamageRate + ancillariesDamageRate;
            
            double subStrDamageCost = inspection.CalculateBridgePartDamageCost(1);
            double superStrDamageCost = inspection.CalculateBridgePartDamageCost(2);
            double ancillariesDamageCost = inspection.CalculateBridgePartDamageCost(3);

            // Now, there is some major inspection data for the given bridgeid and inspectionyear
            //  - If there is no result data, CREATE new result data. 
            //  - If there exists a result data, just UPDATE it
            if (resInspMajor == null)
            {
                ResultInspMajor newResInspMajor = new ResultInspMajor()
                {
                    Id = Guid.NewGuid().ToString(),
                    BridgeId = bridgeid,
                    InspectionYear = inspectionyear,
                    InspectionDate = inspectiondate,
                    DmgPercSubStructure = subStrDamageRate,
                    DmgPercSuperStructure = superStrDamageRate,
                    DmgPercAncillaries = ancillariesDamageRate,
                    DmgPerc = totDamageRate,
                    RepairCostSubStruct = subStrDamageCost,
                    RepairCostSuperStruct = superStrDamageCost,
                    RepairCostAncillaries = ancillariesDamageCost
                };   
                
                db.ResultInspMajors.Add(newResInspMajor);
                db.SaveChanges();
            }
            else
            {
                resInspMajor.DmgPercSubStructure = subStrDamageRate;
                resInspMajor.DmgPercSuperStructure = superStrDamageRate;
                resInspMajor.DmgPercAncillaries = ancillariesDamageRate;
                resInspMajor.DmgPerc = totDamageRate;
                resInspMajor.RepairCostSubStruct = subStrDamageCost;
                resInspMajor.RepairCostSuperStruct = superStrDamageCost;
                resInspMajor.RepairCostAncillaries = ancillariesDamageCost;
                resInspMajor.InspectionDate = inspectiondate;
                db.SaveChanges();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //// Use only to update the bridge result inspection table if damage rate and cost parametera are changeed////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        public string PopulateBridgeInspectionResult(int inspectionyear)
        {
            string success = "";

            var leftOuterJoinObs = (from insp in db.DamageInspMajors
                                    join obs in db.BridgeObservations on insp.BridgeId equals obs.BridgeId into table1
                                    from maj_obs in table1.DefaultIfEmpty()
                                    where insp.InspectionYear == inspectionyear /*|| maj_obs.InspectionYear == inspectionyear*/
                                    //select new { insp.BridgeId, insp.InspectionDate } ).Distinct().ToList();
                                    select new { insp.BridgeId, insp.InspectionDate }).GroupBy(x => x.BridgeId).Select(x => x.FirstOrDefault()).ToList();


            var rightOuterJoinObs = (from obs in db.BridgeObservations
                                     join insp in db.DamageInspMajors on obs.BridgeId equals insp.BridgeId into table2
                                     from com_maj in table2.DefaultIfEmpty()
                                     where obs.InspectionYear == inspectionyear /*|| com_maj.InspectionYear == inspectionyear*/
                                     select new { obs.BridgeId, obs.InspectionDate }).GroupBy(x => x.BridgeId).Select(x => x.FirstOrDefault()).ToList();

            var fullOuterJoinObs = leftOuterJoinObs.Union(rightOuterJoinObs).ToList();

            var leftOuterJoinCom = (from insp in db.DamageInspMajors
                                    join com in db.BridgeComments on insp.BridgeId equals com.BridgeId into table3
                                    from maj_com in table3.DefaultIfEmpty()
                                    where insp.InspectionYear == inspectionyear /*|| maj_com.InspectionYear == inspectionyear*/
                                    select new { insp.BridgeId, insp.InspectionDate }).GroupBy(x => x.BridgeId).Select(x => x.FirstOrDefault()).ToList();

            var rightOuterJoinCom = (from com in db.BridgeComments
                                     join insp in db.DamageInspMajors on com.BridgeId equals insp.BridgeId into table4
                                     from com_maj in table4.DefaultIfEmpty()
                                     where com.InspectionYear == inspectionyear /*|| com_maj.InspectionYear == inspectionyear*/
                                     select new { com.BridgeId, com.InspectionDate }).GroupBy(x => x.BridgeId).Select(x => x.FirstOrDefault()).ToList();

            var fullOuterJoinCom = leftOuterJoinCom.Union(rightOuterJoinCom).GroupBy(x => x.BridgeId).Select(x => x.FirstOrDefault()).ToList();

            var inspectedBridges = fullOuterJoinObs.Union(fullOuterJoinCom).GroupBy(x => x.BridgeId).Select(x => x.FirstOrDefault()).ToList();
            //List<string> bridgeIds = db.DamageInspMajors.Where(s => s.InspectionYear == inspectionyear).Select(s => s.BridgeId).Distinct().ToList();

            List<ResultInspMajor> newResultList = new List<ResultInspMajor>();

            foreach (var bridge in inspectedBridges)
            {
                DamageInspectionModel brgInsp = new DamageInspectionModel(bridge.BridgeId, inspectionyear);

                DateTime inspectiondate = (bridge.InspectionDate != null) ? (DateTime)bridge.InspectionDate : DateTime.Now;
                ResultInspMajor resInspMajor = brgInsp.ResultList.Where(c => c.InspectionDate != null).FirstOrDefault();
                //List<DamageInspMajor> dmgInspMajor = db.DamageInspMajors.Where(c => c.BridgeId == bridge.BridgeId && c.InspectionYear == inspectionyear).ToList();

                double subStrDamageRate = brgInsp.CalculateBridgePartDamageRate(1);
                double superStrDamageRate = brgInsp.CalculateBridgePartDamageRate(2);
                double ancillariesDamageRate = brgInsp.CalculateBridgePartDamageRate(3);

                //// Damage percent is taken as the maximum of the three bridge parts
                //double maxDamageRate = (new List<double> { subStrDamageRate, superStrDamageRate, ancillariesDamageRate }).Max();

                // Damage percent of the entire bridge is taken as the sum of the damage percent of the three bridge parts
                double totDamageRate = subStrDamageRate + superStrDamageRate + ancillariesDamageRate;

                double subStrDamageCost = brgInsp.CalculateBridgePartDamageCost(1);
                double superStrDamageCost = brgInsp.CalculateBridgePartDamageCost(2);
                double ancillariesDamageCost = brgInsp.CalculateBridgePartDamageCost(3);

                // CREATE new result data. 

                ResultInspMajor newResInspMajor = new ResultInspMajor()
                {
                    Id = Guid.NewGuid().ToString(),
                    BridgeId = bridge.BridgeId,
                    InspectionYear = inspectionyear,
                    InspectionDate = inspectiondate,
                    DmgPercSubStructure = subStrDamageRate,
                    DmgPercSuperStructure = superStrDamageRate,
                    DmgPercAncillaries = ancillariesDamageRate,
                    DmgPerc = totDamageRate,
                    RepairCostSubStruct = subStrDamageCost,
                    RepairCostSuperStruct = superStrDamageCost,
                    RepairCostAncillaries = ancillariesDamageCost
                };

                newResultList.Add(newResInspMajor);
            }

            // First remove all results in the given inspection year in the database
            var itemtoremove = db.ResultInspMajors.Where(r => r.InspectionYear == inspectionyear);
            db.ResultInspMajors.RemoveRange(itemtoremove);

            // Then write the new results of the given inspection year to the database
            foreach (var result in newResultList)
            {
                db.ResultInspMajors.Add(result);
            }

            // Commit all database changes made so far and save them to the database
            db.SaveChanges();

            return success;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult _GetProcessBridgeInspectionData(int inspectionyear)
        {
            PopulateBridgeInspectionResult(inspectionyear);

            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ProcessBridgeInspectionData()
        {
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

            //Add the first unselected item
            inspectionYearList.Insert(0, new SelectListItem { Text = "--Select--", Value = "0" });

            ViewBag.InspectionYearList = inspectionYearList;

            return View();
        }

        public string UpdateDamageData(string id, string origvalue, string value, string bridgeid, int inspyear, DateTime inspdate, string dmgtypeid, string dmgrank, int stritemid)
        {
            // No change
            if (origvalue == value)
                return value;

            double result;

            // If input value is not numeric (space, special character, etc), just keep the original value
            if (!double.TryParse(value, out result))
                return origvalue;

            // If input value is a negative numeric value, just keep the original value
            if (result < 0)
                return origvalue;

            if (value == "0")
            {
                // Already existing and its value is set to 0, so delete it
                var rnkId = db.DamageRanks.Where(c => c.DamageRankName == dmgrank).FirstOrDefault().DamageRankId;
                var typeId = Int32.Parse(dmgtypeid);

                DamageInspMajor dmgInspMajor = db.DamageInspMajors.Where(c => c.BridgeId == bridgeid && c.InspectionYear == inspyear
                                                  && c.StructureItemId == stritemid && c.DamageRankId == rnkId && c.DamageTypeId == typeId).FirstOrDefault();
                if (dmgInspMajor != null)
                {
                    // delete record from damage table
                    db.DamageInspMajors.Remove(dmgInspMajor);
                    db.SaveChanges();
                }
            }
            else
            {
                // Already existing and its value is changed to some value, so update the Damage Area
                var rnkId = db.DamageRanks.Where(c => c.DamageRankName == dmgrank).FirstOrDefault().DamageRankId;
                var typeId = Int32.Parse(dmgtypeid);

                DamageInspMajor dmgInspMajor = db.DamageInspMajors.Where(c => c.BridgeId == bridgeid && c.InspectionYear == inspyear
                                                  && c.StructureItemId == stritemid && c.DamageRankId == rnkId && c.DamageTypeId == typeId).FirstOrDefault();
                if (dmgInspMajor != null)
                {
                    // Update damage area in damage table
                    dmgInspMajor.DamageArea = Double.Parse(value);
                    dmgInspMajor.InspectionDate = inspdate;
                    dmgInspMajor.InspectionYear = inspyear;
                    db.SaveChanges();
                }
                // New, its value is set to some value, so insert a new record
                else
                {
                    DamageRank dmgRank = db.DamageRanks.Where(c => c.DamageRankName == dmgrank).FirstOrDefault();
                    // add a new record with the new damage area in damage table
                    dmgInspMajor = new DamageInspMajor()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BridgeId = bridgeid,
                        StructureItemId = stritemid,
                        DamageTypeId = Int32.Parse(dmgtypeid),
                        DamageRankId = dmgRank.DamageRankId,
                        InspectionYear = inspyear,
                        InspectionDate = inspdate,
                        DamageArea = Double.Parse(value)
                    };
                    db.DamageInspMajors.Add(dmgInspMajor);
                    db.SaveChanges();
                }

                //DamageInspMajor dmgInspMajor = db.DamageInspMajors.Where(c => c.BridgeId == bridgeid && c.InspectionYear == inspyear
                //                                  && c.StructureItemId == stritemid && c.DamageTypeId == typeId).FirstOrDefault();
                //// Delete any existing record for the given structure item and for the given damage type
                //if (dmgInspMajor != null)
                //{
                //    db.DamageInspMajors.Remove(dmgInspMajor); 
                //    db.SaveChanges();
                //}

                //// New, its value is set to some value, so insert a new record
                //DamageRank dmgRank = db.DamageRanks.Where(c => c.DamageRankName == dmgrank).FirstOrDefault();
                //// add a new record with the new damage area in damage table
                //dmgInspMajor = new DamageInspMajor()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    BridgeId = bridgeid,
                //    StructureItemId = stritemid,
                //    DamageTypeId = Int32.Parse(dmgtypeid),
                //    DamageRankId = dmgRank.DamageRankId,
                //    InspectionYear = inspyear,
                //    InspectionDate = inspdate,
                //    DamageArea = Double.Parse(value)
                //};
                //db.DamageInspMajors.Add(dmgInspMajor);
                //db.SaveChanges();
            }
            
            // Update record in result table
            UpdateInspectionResult(bridgeid, inspyear, inspdate);

            return value;
        }

        public ActionResult GetLastAddedDamageId(string bridgeid)
        {
            var lastAddedDamage = (from c in db.DamageInspMajors
                                   where c.BridgeId == bridgeid
                                   select new
                                   {
                                       Id = c.Id,
                                   }).ToList().LastOrDefault();

            return Json(lastAddedDamage, JsonRequestBehavior.AllowGet);
        }


        //GET
        public ActionResult RecommendationAndComments([Optional] string bridgeid, [Optional] int inspectionyear, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var commentList = (from s in db.BridgeComments
                               where s.BridgeId == bridgeid && s.InspectionYear == inspectionyear
                               select s);

            var observation = (from s in db.BridgeObservations
                               where s.BridgeId == bridgeid && s.InspectionYear == inspectionyear
                               select s).FirstOrDefault();

            List<SelectListItem> recommendationSelectList = db.MaintenanceUrgencies.Select(c => new SelectListItem
                                            { Text = c.UrgencyId.ToString() + " - " + c.UrgencyName, Value = c.UrgencyId.ToString() }).ToList();

            //Add the first unselected item
            recommendationSelectList.Insert(0, new SelectListItem { Text = "-- No Recommendation Selected --", Value = "-1" });
            
            if (observation != null)
            {
                foreach (var item in recommendationSelectList)
                {
                    if (item.Value == observation.UrgencyId.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
                TempData["WaterWayAdequacy"] = observation.WaterWayAdequacy;
            }
            else
            {
                // Select the first option as default
                recommendationSelectList[0].Selected = true;
                //TempData["WaterWayAdequacy"] = false;
            }

            ViewBag.BridgeId = bridgeid;
            ViewBag.InspectionYear = inspectionyear;
            ViewBag.Inspectiondate = inspectiondate;
            TempData["MaintenanceUrgencyList"] = recommendationSelectList;
            TempData["Comment"] = commentList.ToList();
            
            return PartialView();
        }

        public string UpdateComment(string id, string value, DateTime inspdate)
        {
            BridgeComment comment = (from c in db.BridgeComments
                                     where c.Id == id
                                     select c).FirstOrDefault();

            comment.Comment = value;
            comment.InspectionDate = inspdate;

            // Update result table too
            var inspRes = db.ResultInspMajors.Where(res => res.BridgeId == comment.BridgeId && res.InspectionYear == comment.InspectionYear).FirstOrDefault();
            inspRes.InspectionDate = inspdate;

            db.SaveChanges();

            return value;
        }

        //GET
        public ActionResult GetLastAddedComment(string bridgeid)
        {
            var lastAddedComment = (from c in db.BridgeComments
                                    where c.BridgeId == bridgeid
                                   select new
                                   {
                                       Id = c.Id,
                                       BridgeId = c.BridgeId,
                                       InspectionDate = c.InspectionDate,
                                       InspectionYear = c.InspectionYear,
                                   }).ToList().LastOrDefault();
            
            return Json(lastAddedComment, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddComment(string bridgeid, int inspectionyear, DateTime inspectiondate, string comment)
        {
            List<BridgeComment> commentList = (from c in db.BridgeComments
                                               where c.BridgeId == bridgeid && c.InspectionYear == inspectionyear
                                               select c).ToList();
            BridgeComment brgComment = new BridgeComment();

            if (comment.Trim() != "")
            {
                brgComment.Id = Guid.NewGuid().ToString();
                brgComment.BridgeId = bridgeid;
                brgComment.InspectionYear = inspectionyear;
                brgComment.InspectionDate = inspectiondate;
                brgComment.Comment = comment;

                db.BridgeComments.Add(brgComment);
            }

            // Update Result table too
            var inspRes = db.ResultInspMajors.Where(res => res.BridgeId == bridgeid && res.InspectionYear == inspectionyear).FirstOrDefault();
            var newInspRes = new ResultInspMajor();
            if (inspRes != null) //only update result inspection date
            {
                inspRes.InspectionDate = inspectiondate;
            }
            else //create new result record
            {
                newInspRes.Id = Guid.NewGuid().ToString();
                newInspRes.BridgeId = bridgeid;
                newInspRes.InspectionDate = inspectiondate;
                newInspRes.InspectionYear = inspectionyear;
                newInspRes.DmgPercSubStructure = 0.0;
                newInspRes.DmgPercSuperStructure = 0.0;
                newInspRes.DmgPercAncillaries = 0.0;
                newInspRes.DmgPerc = 0.0;
                newInspRes.RepairCostSubStruct = 0.00;
                newInspRes.RepairCostSuperStruct = 0.00;
                newInspRes.RepairCostAncillaries = 0.00;
                db.ResultInspMajors.Add(newInspRes);
            }

            db.SaveChanges();
            
            return Json(brgComment);
        }

        public ActionResult DeleteComment(string id)
        {
            BridgeComment comment = db.BridgeComments.Find(id);

            db.BridgeComments.Remove(comment);
            db.SaveChanges();

            return new EmptyResult();
        }

        public JsonResult AddObservation(string bridgeid, int inspectionyear, DateTime inspectiondate, int urgencyid, bool waterwayadequacy)
        {
            // If no intervention type is selected, do not save it to database
            if (urgencyid == -1)
            {
                return Json(new EmptyResult());
            }
            else
            {
                BridgeObservation observation = (from c in db.BridgeObservations
                                                 where c.BridgeId == bridgeid && c.InspectionYear == inspectionyear
                                                 select c).FirstOrDefault();

                BridgeObservation obs = new BridgeObservation();
                var inspRes = db.ResultInspMajors.Where(res => res.BridgeId == bridgeid && res.InspectionYear == inspectionyear).FirstOrDefault();

                // If observation already exists for the given bridge in the given inspection year, just update it
                if (observation != null)
                {
                    observation.UrgencyId = urgencyid;
                    observation.WaterWayAdequacy = waterwayadequacy;
                    observation.InspectionDate = inspectiondate;
                    obs = observation;

                    // Update result table inspection date
                    inspRes.InspectionDate = inspectiondate;

                    db.SaveChanges();
                }
                // Otherwise, create a new record
                else
                {
                    BridgeObservation observationNew = new BridgeObservation();

                    observationNew.Id = Guid.NewGuid().ToString();
                    observationNew.BridgeId = bridgeid;
                    observationNew.InspectionYear = inspectionyear;
                    observationNew.InspectionDate = inspectiondate;
                    observationNew.UrgencyId = urgencyid;
                    observationNew.WaterWayAdequacy = waterwayadequacy;
                    obs = observationNew;

                    db.BridgeObservations.Add(observationNew);

                    // Update Result table too
                    
                    var newInspRes = new ResultInspMajor();
                    if (inspRes != null) //only update result inspection date
                    {
                        inspRes.InspectionDate = inspectiondate;
                    }
                    else //create new result record
                    {
                        newInspRes.Id = Guid.NewGuid().ToString();
                        newInspRes.BridgeId = bridgeid;
                        newInspRes.InspectionDate = inspectiondate;
                        newInspRes.InspectionYear = inspectionyear;
                        newInspRes.DmgPercSubStructure = 0.0;
                        newInspRes.DmgPercSuperStructure = 0.0;
                        newInspRes.DmgPercAncillaries = 0.0;
                        newInspRes.DmgPerc = 0.0;
                        newInspRes.RepairCostSubStruct = 0.00;
                        newInspRes.RepairCostSuperStruct = 0.00;
                        newInspRes.RepairCostAncillaries = 0.00;
                        db.ResultInspMajors.Add(newInspRes);
                    }

                    db.SaveChanges();
                }
                return Json(obs);
            }
        }
    }
}