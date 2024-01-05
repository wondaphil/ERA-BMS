using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class DamageInspCulvertController : Controller
    {
        private BMSEntities db = new BMSEntities();
        
        // GET: DamageInspCulvertStr
        public ActionResult Index(string id = "")
        {
            Culvert culvert = db.Culverts.Find(id);
            var culDmgInspStr = db.culDamageInspStructures.Where(c => c.CulvertId == id);
            culDamageInspHydraulic culDmgInspHydr = db.culDamageInspHydraulics.Find(id);

            InspCulvertStructureViewModel culStr = new InspCulvertStructureViewModel()
            {
                Culvert = culDmgInspStr.Where(d => d.StructureItemId == 1).ToList(),
                Abutment = culDmgInspStr.Where(d => d.StructureItemId == 2).ToList(),
                GuardParapet = culDmgInspStr.Where(d => d.StructureItemId == 3).ToList(),
                WingWall = culDmgInspStr.Where(d => d.StructureItemId == 4).ToList(),
                HeadWall = culDmgInspStr.Where(d => d.StructureItemId == 5).ToList(),
                PavedWaterWay = culDmgInspStr.Where(d => d.StructureItemId == 6).ToList()
            };
            ObservationAndRecommendation observation = db.ObservationAndRecommendations.Find(id);

            CulvertInspectionViewModel culvertinspection = new CulvertInspectionViewModel()
            {
                CulvertStructure = culStr,
                HydraulicDamage = culDmgInspHydr,
                Observations = observation,
            };

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
                TempData["InspectionDate"] = DateTime.Now.ToString("yyyy-MM-dd");

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

                //For culvert structure
                TempData["CulvertDamageTypeList"] = GetStructureDamageTypeList(1);
                TempData["AbutmentDamageTypeList"] = GetStructureDamageTypeList(2);
                TempData["GuardParapetDamageTypeList"] = GetStructureDamageTypeList(3);
                TempData["WingWallDamageTypeList"] = GetStructureDamageTypeList(4);
                TempData["HeadWallDamageTypeList"] = GetStructureDamageTypeList(5);
                TempData["PavedWaterWayDamageTypeList"] = GetStructureDamageTypeList(6);

                var dmgTypeList = (from dmg in db.culHydrDamageTypes
                                  select new { dmg.DamageTypeId, dmg.DamageTypeName }).ToList();

                // Add the first unselected item
                dmgTypeList.Add(new { DamageTypeId = -1, DamageTypeName = "-- Not Selected --" });
                
                // Sort them to bring the unselected option to top
                dmgTypeList = dmgTypeList.OrderBy(s => s.DamageTypeId).ToList();

                // Change the generic list to SelectListItem list
                List<SelectListItem> hydDmgList = dmgTypeList.Select(m => new SelectListItem { Text = m.DamageTypeName, Value = m.DamageTypeId.ToString() }).ToList();

                // Declare seven variables and assign them the same value
                List<SelectListItem> OverTopping, Constriction, EmbankmentScour, ChannelScour, ChannelObstruction, Siltation, Vegitation;
                OverTopping = Constriction = EmbankmentScour = ChannelScour = ChannelObstruction = Siltation = Vegitation = hydDmgList;

                if (culDmgInspHydr != null) 
                {
                    // Check the select list based on the value
                    OverTopping[(int)culDmgInspHydr.OverTopping].Selected = true;
                    Constriction[(int)culDmgInspHydr.OverTopping].Selected = true;
                    EmbankmentScour[(int)culDmgInspHydr.OverTopping].Selected = true;
                    ChannelScour[(int)culDmgInspHydr.OverTopping].Selected = true;
                    ChannelObstruction[(int)culDmgInspHydr.OverTopping].Selected = true;
                    Siltation[(int)culDmgInspHydr.OverTopping].Selected = true;
                    Vegitation[(int)culDmgInspHydr.OverTopping].Selected = true;
                }

                //For hydraulic and channel damage
                TempData["OverTopping"] = (culDmgInspHydr == null) ? hydDmgList : OverTopping;
                TempData["Constriction"] = (culDmgInspHydr == null) ? hydDmgList : Constriction;
                TempData["EmbankmentScour"] = (culDmgInspHydr == null) ? hydDmgList : EmbankmentScour;
                TempData["ChannelScour"] = (culDmgInspHydr == null) ? hydDmgList : ChannelScour;
                TempData["ChannelObstruction"] = (culDmgInspHydr == null) ? hydDmgList : ChannelObstruction;
                TempData["Siltation"] = (culDmgInspHydr == null) ? hydDmgList : Siltation;
                TempData["Vegitation"] = (culDmgInspHydr == null) ? hydDmgList : Vegitation;

                //For observation
                List <SelectListItem> urgencySelectList = db.MaintenanceUrgencies.Select(c => new SelectListItem
                        { Text = c.UrgencyId.ToString() + " - " + c.UrgencyName, Value = c.UrgencyId.ToString()}).ToList();

                //Add the first unselected item
                urgencySelectList.Insert(0, new SelectListItem { Text = "-- No Intervention Selected --", Value = "-1" });
                
                if (observation != null)
                {
                    foreach (var item in urgencySelectList)
                    {
                        if (item.Value == observation.UrgencyIndex.ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                    TempData["Recommendation"] = observation.InspectorRecommendation;
                }
                else
                {
                    // Select the first option as default
                    urgencySelectList[0].Selected = true;
                }
                TempData["UrgencyList"] = urgencySelectList; // new SelectList(db.MaintenanceUrgencies, "UrgencyId", "UrgencyName").ToList();

                return View(culvertinspection);
            }
        }

        // If the date entered is not valid, then just take today's date
        public DateTime ValidateInspectionDate(DateTime datevalue)
        {
            DateTime result;
            if (DateTime.TryParse(datevalue.ToString(), out result))
                return datevalue;
            else
                return DateTime.Now;
        }

        //Return a list of Culvert Structure Damage for the given structure item id
        public List<culDamageInspStructure> GetCulvertStructureDamageList(int structItemId)
        {
            return (from s in db.culDamageInspStructures
                    where s.StructureItemId == structItemId
                    select s).ToList();
        }

        //Return a "Damage Type" list for the given structure item id
        public List<string[]> GetStructureDamageTypeList(int structItemId)
        {
            List<culDamageRateAndCost> strDamageTypes = db.culDamageRateAndCosts.Where(d => d.StructureItemId == structItemId).ToList();
            var list = strDamageTypes.Select(d => new[] { d.DamageTypeId.ToString(), d.DamageTypeName });
            
            return list.ToList();
        }

        public ActionResult StructureDamage([Optional] string culvertid, [Optional] DateTime inspectiondate)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<culDamageInspStructure> culvertList = GetCulvertStructureDamageList(1).Where(s => s.CulvertId == culvertid).ToList();
            List<culDamageInspStructure> abutmentList = GetCulvertStructureDamageList(2).Where(s => s.CulvertId == culvertid).ToList();
            List<culDamageInspStructure> guardParapetList = GetCulvertStructureDamageList(3).Where(s => s.CulvertId == culvertid).ToList();
            List<culDamageInspStructure> wingWallList = GetCulvertStructureDamageList(4).Where(s => s.CulvertId == culvertid).ToList();
            List<culDamageInspStructure> headWallList = GetCulvertStructureDamageList(5).Where(s => s.CulvertId == culvertid).ToList();
            List<culDamageInspStructure> pavedWaterWayList = GetCulvertStructureDamageList(6).Where(s => s.CulvertId == culvertid).ToList();

            if (culvertList == null || abutmentList == null || guardParapetList == null || wingWallList == null
                    || headWallList == null || pavedWaterWayList == null)
            {
                return HttpNotFound();
            }

            var inspCulvertStructViewModel = new InspCulvertStructureViewModel
            {
                Culvert = culvertList,
                Abutment = abutmentList,
                GuardParapet = guardParapetList,
                WingWall = wingWallList,
                HeadWall = headWallList,
                PavedWaterWay = pavedWaterWayList
            };

            TempData["CulvertId"] = culvertid;
            TempData["InspectionDate"] = ValidateInspectionDate(inspectiondate);
            
            TempData["CulvertDamageTypeList"] = GetStructureDamageTypeList(1);
            TempData["AbutmentDamageTypeList"] = GetStructureDamageTypeList(2);
            TempData["GuardParapetDamageTypeList"] = GetStructureDamageTypeList(3);
            TempData["WingWallDamageTypeList"] = GetStructureDamageTypeList(4);
            TempData["HeadWallDamageTypeList"] = GetStructureDamageTypeList(5);
            TempData["PavedWaterWayDamageTypeList"] = GetStructureDamageTypeList(6);

            return PartialView(inspCulvertStructViewModel);
        }

        public double CalculateCulvertStructureDamage(string culvertid, int stritemid)
        {
            // Get a list of damage major inspection for the given culvertid and structure item id
            // Get a list of damage rate and cost for the given structure item id
            List<culDamageInspStructure> dmgInspCulvert = db.culDamageInspStructures.Where(s => s.CulvertId == culvertid && s.StructureItemId == stritemid).ToList();
            List<culDamageRateAndCost> dmgRateCost = db.culDamageRateAndCosts.Where(s => s.StructureItemId == stritemid).ToList();
            double rate = 0.0;

            // Sum up damage rate for each damage type (e.g. Cracking, Void, etc)
            foreach (var insp in dmgInspCulvert)
            {
                rate += (double)dmgRateCost.Where(rt => rt.DamageTypeId == insp.DamageTypeId).FirstOrDefault().DamageValue;
            }
            return rate;
        }

        public double CalculateCulvertStructureCost(string culvertid, int stritemid)
        {
            // Get a list of damage major inspection for the given culvertid, inspection year and structure item id
            // Get a list of damage rate and cost for the given structure item id
            List<culDamageInspStructure> dmgInspCulvert = db.culDamageInspStructures.Where(s => s.CulvertId == culvertid && s.StructureItemId == stritemid).ToList();
            List<culDamageRateAndCost> dmgRateCost = db.culDamageRateAndCosts.Where(s => s.StructureItemId == stritemid).ToList();
            double cost = 0.0;

            // Sum up damage cost (unit cost times damage area) for each damage type (e.g. Cracking, Void, etc) and damage rank (e.g. A+, A, B, etc...)
            foreach (var insp in dmgInspCulvert)
            {
                cost += (double)(dmgRateCost.Where(cst => cst.DamageTypeId == insp.DamageTypeId).FirstOrDefault().UnitRepairCost * insp.Quantity);
            }
            return cost;
        }

        /// <summary>
        /// Calculates total damage rate of the given culvert part by summing up damage rates of structure items in the culvert part
        /// </summary>
        /// <param name="culvertid">Culvert Id</param>
        /// <param name="inspectionyear">Inspection Year (2003, 2006, 2010, 2013, 2016, etc..)</param>
        /// <param name="brgpartid">Culvert Part Id (1, 2 or 3)</param>
        /// <returns></returns>
        public double CalculateCulvertDamageRate(string culvertid)
        {
            // Get a list of structure items for a culvert
            //     Culvert => structure items=1,2,3,4,5,6
            List<culStructureItem> culStrItem = db.culStructureItems.ToList();
            double rate = 0.0;

            // Sum up damage rate for each structure item
            foreach (var str in culStrItem)
            {
                rate += CalculateCulvertStructureDamage(culvertid, str.StructureItemId);
            }

            return rate;
        }

        /// <summary>
        /// Calculates total damage cost of the given culvert part by summing up damage costs of structure items in the culvert part
        /// </summary>
        /// <param name="culvertid">Culvert Id</param>
        /// <param name="inspectionyear">Inspection Year (2003, 2006, 2010, 2013, 2016, etc..)</param>
        /// <param name="brgpartid">Culvert Part Id (1, 2 or 3)</param>
        /// <returns></returns>
        public double CalculateCulvertDamageCost(string culvertid)
        {
            // Get a list of structure items for a culvert
            //     Culvert => structure items=1,2,3,4,5,6
            List<culStructureItem> culStrItem = db.culStructureItems.ToList();
            double cost = 0.0;

            // Sum up damage cost for each structure item in the given culvert part
            foreach (var str in culStrItem)
            {
                cost += CalculateCulvertStructureCost(culvertid, str.StructureItemId);
            }

            return cost;
        }

        public void UpdateCulvertInspectionResult(string culvertid, DateTime inspectiondate)
        {
            ResultInspCulvert resInspCulvert = db.ResultInspCulverts.Where(c => c.CulvertId == culvertid).FirstOrDefault();
            List<culDamageInspStructure> dmgInspMajor = db.culDamageInspStructures.Where(c => c.CulvertId == culvertid).ToList();

            // If there is NO inspection data for the given culvertid 
            // but there is a result data, then just DELETE the result data from the database!
            if (dmgInspMajor.Count == 0 && resInspCulvert != null)
            {
                db.ResultInspCulverts.Remove(resInspCulvert);
                db.SaveChanges();
                return;
            }

            double strDamageRate = CalculateCulvertDamageRate(culvertid);
            double strDamageCost = CalculateCulvertDamageCost(culvertid);

            // Now, there is some culvert inspection data for the given culvertid
            //  - If there is no result data, CREATE new result data. 
            //  - If there exists a result data, just UPDATE it
            if (resInspCulvert == null)
            {
                ResultInspCulvert newResInspCulvert = new ResultInspCulvert()
                {
                    CulvertId = culvertid,
                    InspectionDate = ValidateInspectionDate(inspectiondate),
                    DamagePerc = strDamageRate,
                    RepairCost = strDamageCost
                };

                db.ResultInspCulverts.Add(newResInspCulvert);
                db.SaveChanges();
            }
            else
            {
                resInspCulvert.InspectionDate = ValidateInspectionDate(inspectiondate);
                resInspCulvert.DamagePerc = strDamageRate;
                resInspCulvert.RepairCost = strDamageCost;

                db.SaveChanges();
            }
        }

        ////////////////////////////////////////////////////////////////////////////
        //// Use only initially to populate the culvert result inspection table ////
        ////////////////////////////////////////////////////////////////////////////
        public string PopulateCulvertInspectionResult()
        {
            string success = "";

            List<string> culvertIds = db.culDamageInspStructures.Select(s => s.CulvertId).Distinct().ToList();
            foreach (string culvertid in culvertIds)
            {
                ResultInspCulvert resInspCulvert = db.ResultInspCulverts.Where(c => c.CulvertId == culvertid).FirstOrDefault();
                List<culDamageInspStructure> dmgInspStr = db.culDamageInspStructures.Where(c => c.CulvertId == culvertid).ToList();
                var inspectiondate = dmgInspStr.FirstOrDefault().InspectionDate;
                UpdateCulvertInspectionResult(culvertid, (DateTime) inspectiondate);

                // If there is NO inspection data for the given culvertid 
                // but there is a result data, then just DELETE the result data from the database!
                if (dmgInspStr.Count == 0 && resInspCulvert != null)
                {
                    db.ResultInspCulverts.Remove(resInspCulvert);
                    //db.SaveChanges();
                }
                else
                {
                    double strDamageRate = CalculateCulvertDamageRate(culvertid);
                    double strDamageCost = CalculateCulvertDamageCost(culvertid);

                    // Now, there is some major inspection data for the given bridgeid and inspectionyear
                    //  - If there is no result data, CREATE new result data. 
                    //  - If there exists a result data, just UPDATE it
                    if (resInspCulvert == null)
                    {
                        ResultInspCulvert newResInspCulvert = new ResultInspCulvert()
                        {
                            CulvertId = culvertid,
                            InspectionDate = ValidateInspectionDate((DateTime) inspectiondate),
                            DamagePerc = strDamageRate,
                            RepairCost = strDamageCost
                        };

                        db.ResultInspCulverts.Add(newResInspCulvert);
                        //db.SaveChanges();
                    }
                    else
                    {
                        resInspCulvert.DamagePerc = strDamageRate;
                        resInspCulvert.RepairCost = strDamageCost;

                        //db.SaveChanges();
                    }
                }
            }
            db.SaveChanges();
            
            return success;
        }

        public string UpdateDamageQuantity(string id, string origvalue, string value, string culvertid, DateTime inspdate, int dmgtypeid, int stritemid)
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
                // Already existing, its value is set to 0, so delete it
                if (id != "-1")
                {
                    culDamageInspStructure dmgInspCulStr = db.culDamageInspStructures.Where(c => c.CulvertId == culvertid 
                                                  && c.StructureItemId == stritemid && c.DamageTypeId == dmgtypeid).FirstOrDefault();

                    if (dmgInspCulStr != null)
                    {
                        // delete record from damage table
                        db.culDamageInspStructures.Remove(dmgInspCulStr);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                // Already existing, its value is set to some value, so update the Damage Area
                if (id != "-1")
                {
                    culDamageInspStructure dmgInspCulStr = db.culDamageInspStructures.Where(c => c.CulvertId == culvertid
                                                  && c.StructureItemId == stritemid && c.DamageTypeId == dmgtypeid).FirstOrDefault();

                    dmgInspCulStr.Quantity = Double.Parse(value);
                    dmgInspCulStr.InspectionDate = ValidateInspectionDate(inspdate);
                    db.SaveChanges();
                }
                // New, its value is set to some value, so insert the Damage Area
                else
                {
                    culDamageInspStructure dmgInspCulStr = new culDamageInspStructure();
                    dmgInspCulStr.Id = Guid.NewGuid().ToString();
                    dmgInspCulStr.CulvertId = culvertid;
                    dmgInspCulStr.StructureItemId = stritemid;
                    dmgInspCulStr.DamageTypeId = dmgtypeid;
                    dmgInspCulStr.InspectionDate = ValidateInspectionDate(inspdate);
                    dmgInspCulStr.Quantity = Double.Parse(value);
                    db.culDamageInspStructures.Add(dmgInspCulStr);
                    db.SaveChanges();
                }
            }

            // Update record in culvert result table
            UpdateCulvertInspectionResult(culvertid, inspdate);

            return value;
        }
        public ActionResult HydraulicAndChannelDamage([Optional] string culvertid, [Optional] DateTime inspectiondate)
        {
            culDamageInspHydraulic hydrDamage = (from h in db.culDamageInspHydraulics
                                                where h.CulvertId == culvertid
                                                 select h).FirstOrDefault();
            
            var dmgTypeList = db.culHydrDamageTypes.Select(c => new SelectListItem
                                            { Text = c.DamageTypeName, Value = c.DamageTypeId.ToString() }).ToList();
            // Add the first unselected item
            dmgTypeList.Insert(0, new SelectListItem { Text = "-- Not Selected --", Value = "-1" });
            
            TempData["OverTopping"] = dmgTypeList;
            TempData["Constriction"] = dmgTypeList;
            TempData["EmbankmentScour"] = dmgTypeList;
            TempData["ChannelScour"] = dmgTypeList;
            TempData["ChannelObstruction"] = dmgTypeList;
            TempData["Siltation"] = dmgTypeList;
            TempData["Vegitation"] = dmgTypeList;

            TempData["CulvertId"] = culvertid;
            TempData["InspectionDate"] = ValidateInspectionDate(inspectiondate);
            
            if (hydrDamage == null)
            {
                return PartialView("HydraulicAndChannelDamageNew", hydrDamage);
            }

            return PartialView("HydraulicAndChannelDamage", hydrDamage);
        }

        public ActionResult HydraulicAndChannelDamageSave([Bind(Include = "CulvertId,OverTopping,Constriction,EmbankmentScour,ChannelScour,ChannelObstruction,Siltation,Vegitation,InspectionDate")] culDamageInspHydraulic hydrDamage)
        {
            if (ModelState.IsValid)
            {
                // In case no option is selected, set the selection to "Unknown"
                hydrDamage.ChannelObstruction = (hydrDamage.ChannelObstruction == -1) ? 0 : hydrDamage.ChannelObstruction;
                hydrDamage.ChannelScour = (hydrDamage.ChannelScour == -1) ? 0 : hydrDamage.ChannelScour;
                hydrDamage.Constriction = (hydrDamage.Constriction == -1) ? 0 : hydrDamage.Constriction;
                hydrDamage.EmbankmentScour = (hydrDamage.EmbankmentScour == -1) ? 0 : hydrDamage.EmbankmentScour;
                hydrDamage.OverTopping = (hydrDamage.OverTopping == -1) ? 0 : hydrDamage.OverTopping;
                hydrDamage.Siltation = (hydrDamage.Siltation == -1) ? 0 : hydrDamage.Siltation;
                hydrDamage.Vegitation = (hydrDamage.Vegitation == -1) ? 0 : hydrDamage.Vegitation;
                hydrDamage.InspectionDate = ValidateInspectionDate((DateTime) hydrDamage.InspectionDate);

                db.Entry(hydrDamage).State = EntityState.Modified;

                // Save current inspection date to Result table too
                var inspRes = db.ResultInspCulverts.Find(hydrDamage.CulvertId);
                var newInspRes = new ResultInspCulvert();
                if (inspRes != null) //update result record
                {
                    inspRes.InspectionDate = hydrDamage.InspectionDate;
                }
                else //create new result record
                {
                    newInspRes.CulvertId = hydrDamage.CulvertId;
                    newInspRes.InspectionDate = hydrDamage.InspectionDate;
                    newInspRes.DamagePerc = 0.0;
                    newInspRes.RepairCost = 0.00;
                    db.ResultInspCulverts.Add(newInspRes);
                }

                db.SaveChanges();
            }

            return PartialView();
        }
        
        public ActionResult HydraulicAndChannelDamageNew([Bind(Include = "CulvertId,OverTopping,Constriction,EmbankmentScour,ChannelScour,ChannelObstruction,Siltation,Vegitation,InspectionDate")] culDamageInspHydraulic hydrDamage)
        {
            if (ModelState.IsValid)
            {
                hydrDamage.InspectionDate = ValidateInspectionDate((DateTime) hydrDamage.InspectionDate);

                db.culDamageInspHydraulics.Add(hydrDamage);

                // Save current inspection date to Result table too
                var inspRes = db.ResultInspCulverts.Find(hydrDamage.CulvertId);
                var newInspRes = new ResultInspCulvert();
                if (inspRes != null) //update result record
                {
                    inspRes.InspectionDate = hydrDamage.InspectionDate;
                }
                else //create new result record
                {
                    newInspRes.CulvertId = hydrDamage.CulvertId;
                    newInspRes.InspectionDate = hydrDamage.InspectionDate;
                    newInspRes.DamagePerc = 0.0;
                    newInspRes.RepairCost = 0.00;
                    db.ResultInspCulverts.Add(newInspRes);
                }

                db.SaveChanges();
            }

            return PartialView(hydrDamage);
        }
        
        public ActionResult ObservationAndRecommendation([Optional] string culvertid, [Optional] DateTime inspectiondate)
        {
            ObservationAndRecommendation observation = db.ObservationAndRecommendations.Find(culvertid);
            List<SelectListItem> urgencySelectList = db.MaintenanceUrgencies.Select(c => new SelectListItem
                                { Text = c.UrgencyId.ToString() + " - " + c.UrgencyName, Value = c.UrgencyId.ToString() }).ToList();

            //Add the first unselected item
            urgencySelectList.Insert(0, new SelectListItem { Text = "-- No Intervention Selected --", Value = "-1" });

            if (observation != null)
            {
                foreach (var item in urgencySelectList)
                {
                    if (item.Value == observation.UrgencyIndex.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
                TempData["Recommendation"] = (observation.InspectorRecommendation == null) ? "" : observation.InspectorRecommendation.ToString();
            }
            else
            {
                // Select the first option as default
                urgencySelectList[0].Selected = true;
            }

            TempData["UrgencyList"] = urgencySelectList;
            TempData["CulvertId"] = culvertid;
            TempData["InspectionDate"] = ValidateInspectionDate(inspectiondate);
            
            return PartialView();
        }

        public ActionResult ObservationAndRecommendationSave([Bind(Include = "CulvertId,UrgencyIndex,InspectorRecommendation,InspectionDate")] ObservationAndRecommendation obsRecomm)
        {
            if (ModelState.IsValid)
            {
                var obs = db.ObservationAndRecommendations.Find(obsRecomm.CulvertId);
                if (obs == null) 
                {
                    // No existing data for the given culvert so add new row in the db table
                    obsRecomm.InspectionDate = ValidateInspectionDate((DateTime) obsRecomm.InspectionDate);
                    db.ObservationAndRecommendations.Add(obsRecomm);
                }
                else
                {
                    // Data exists for the given culvert so just modify db table
                    obs.UrgencyIndex = obsRecomm.UrgencyIndex;
                    obs.InspectorRecommendation = obsRecomm.InspectorRecommendation;
                    obs.InspectionDate = ValidateInspectionDate((DateTime) obsRecomm.InspectionDate);
                    //db.Entry(obsRecomm).State = EntityState.Modified;
                }

                // Save current inspection date to Result table too
                var inspRes = db.ResultInspCulverts.Find(obsRecomm.CulvertId);
                var newInspRes = new ResultInspCulvert();
                if (inspRes != null) //update result record
                {
                    inspRes.InspectionDate = obsRecomm.InspectionDate;
                }
                else //create new result record
                {
                    newInspRes.CulvertId = obsRecomm.CulvertId;
                    newInspRes.InspectionDate = obsRecomm.InspectionDate;
                    newInspRes.DamagePerc = 0.0;
                    newInspRes.RepairCost = 0.00;
                    db.ResultInspCulverts.Add(newInspRes);
                }
                db.SaveChanges();
            }

            return PartialView();
        }
    }
}