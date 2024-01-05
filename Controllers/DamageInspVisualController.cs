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
    public class DamageInspVisualController : Controller
    {
        private BMSEntities db = new BMSEntities();
        private static DateTime globalInspectionDate = DateTime.Now;


        // GET: DamageInspVisual
        public ActionResult Index(string id = "")
        {
            Bridge bridge = db.Bridges.Find(id);
            var dmgInspVisual = db.DamageInspVisuals.Where(d => d.BridgeId == id);

            InspVisualSubStructuresViewModel subStr = new InspVisualSubStructuresViewModel()
            {
                PierAndFoundation = dmgInspVisual.Where(d => d.StructureItemId == 1).ToList(),
                AbutmentAndWingWall = dmgInspVisual.Where(d => d.StructureItemId == 2).ToList(),
                Embankment = dmgInspVisual.Where(d => d.StructureItemId == 3).ToList(),
                RipRap = dmgInspVisual.Where(d => d.StructureItemId == 4).ToList()
            };
            InspVisualSuperStructuresViewModel supStr = new InspVisualSuperStructuresViewModel()
            {
                DeckSlab = dmgInspVisual.Where(d => d.StructureItemId == 5).ToList(),
                ConcreteGirder = dmgInspVisual.Where(d => d.StructureItemId == 6).ToList(),
                SteelTrussGirder = dmgInspVisual.Where(d => d.StructureItemId == 7).ToList()
            };
            InspVisualAncillariesViewModel ancill = new InspVisualAncillariesViewModel()
            {
                Pavement = dmgInspVisual.Where(d => d.StructureItemId == 8).ToList(),
                CurbAndRailing = dmgInspVisual.Where(d => d.StructureItemId == 9).ToList(),
                Drainage = dmgInspVisual.Where(d => d.StructureItemId == 10).ToList(),
                Bearing = dmgInspVisual.Where(d => d.StructureItemId == 11).ToList(),
                ExpansionJoint = dmgInspVisual.Where(d => d.StructureItemId == 12).ToList()
            };

            VisualInspectionViewModel visualinspection = new VisualInspectionViewModel()
            {
                SubStructure = subStr,
                SuperStructure = supStr,
                Ancillaries = ancill
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

                TempData["DamageRankList"] = GetDamageSeverityList();

                TempData["DamageType1"] = GetDamageTypeSelectList(1);
                TempData["DamageType2"] = GetDamageTypeSelectList(2);
                TempData["DamageType3"] = GetDamageTypeSelectList(3);
                TempData["DamageType4"] = GetDamageTypeSelectList(4);
                TempData["DamageType5"] = GetDamageTypeSelectList(5);
                TempData["DamageType6"] = GetDamageTypeSelectList(6);
                TempData["DamageType7"] = GetDamageTypeSelectList(7);
                TempData["DamageType8"] = GetDamageTypeSelectList(8);
                TempData["DamageType9"] = GetDamageTypeSelectList(9);
                TempData["DamageType10"] = GetDamageTypeSelectList(10);
                TempData["DamageType11"] = GetDamageTypeSelectList(11);
                TempData["DamageType12"] = GetDamageTypeSelectList(12);

                List<SelectListItem> dmgSeverity = db.DamageSeverities.Select(c => new SelectListItem
                {
                    Text = c.DamageSeverityName,
                    Value = c.DamageSeverityId.ToString()
                }
                                                                ).ToList();

                //Add the first unselected item
                dmgSeverity.Insert(0, new SelectListItem { Text = "", Value = "0" });
                TempData["DamageSeverity"] = dmgSeverity;

                return View(visualinspection);
            }
        }

        //Return a select list for a "Damage Type" dropdown list item for the given structure item id
        public List<SelectListItem> GetDamageTypeSelectList(int structItemId)
        {
            List<SelectListItem> dmgType = db.DamageTypes.Where(s => s.StructureItemId == structItemId).Select(c => new SelectListItem
                                                                    {
                                                                        Text = c.DamageTypeName,
                                                                        Value = c.DamageTypeId.ToString()
                                                                    }
                                                                ).ToList();

            //Add the first unselected item
            dmgType.Insert(0, new SelectListItem { Text = "", Value = "0" });

            return dmgType;
        }

        //Return a list of DamageInspVisual for the given structure item id
        public List<DamageInspVisual> GetDamageInspVisualList(int structItemId)
        {
            return (from s in db.DamageInspVisuals
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

        //Return a list of Damage Severities ("Major", "Minor", "No Defect", "Total Damage" from DamageSeverity table)
        public List<string> GetDamageSeverityList()
        {
            List<DamageSeverity> damageSeverities = db.DamageSeverities.ToList();

            var list = damageSeverities.Select(r => r.DamageSeverityName).ToList();
            
            return list;
        }

        public ActionResult SubStructure([Optional] string bridgeid, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //List <DamageInspVisual> pierAndFoundationList = GetDamageInspVisualList(4).Where(s => s.BridgeId == bridgeid).Where(s => s.InspectionDate == inspectiondate).ToList();
            List<DamageInspVisual> pierAndFoundationList = GetDamageInspVisualList(1).Where(s => s.BridgeId == bridgeid).ToList();
            List <DamageInspVisual> abutmentAndWingWallList = GetDamageInspVisualList(2).Where(s => s.BridgeId == bridgeid).ToList();
            List <DamageInspVisual> embankmentList = GetDamageInspVisualList(3).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> ripRapList = GetDamageInspVisualList(4).Where(s => s.BridgeId == bridgeid).ToList();
            //List <DamageInspVisual> ripRapList = GetDamageInspVisualList(4).Where(s => s.BridgeId == bridgeid).Where(s => s.InspectionDate == inspectiondate).ToList();

            if (pierAndFoundationList == null || abutmentAndWingWallList == null || embankmentList == null || ripRapList == null)
            {
                return HttpNotFound();
            }

            var subStructViewModel = new InspVisualSubStructuresViewModel
            {
                PierAndFoundation = pierAndFoundationList,
                AbutmentAndWingWall = abutmentAndWingWallList,
                Embankment = embankmentList,
                RipRap = ripRapList
            };

            TempData["BridgeId"] = bridgeid;
            TempData["Inspectiondate"] = inspectiondate;

            TempData["DamageType1"] = GetDamageTypeSelectList(1);
            TempData["DamageType2"] = GetDamageTypeSelectList(2);
            TempData["DamageType3"] = GetDamageTypeSelectList(3);
            TempData["DamageType4"] = GetDamageTypeSelectList(4);
            
            List<SelectListItem> dmgSeverity = db.DamageSeverities.Select(c => new SelectListItem
                                                    {
                                                        Text = c.DamageSeverityName,
                                                        Value = c.DamageSeverityId.ToString()
                                                    }
                                                                ).ToList();

            //Add the first unselected item
            dmgSeverity.Insert(0, new SelectListItem { Text = "", Value = "0" });
            TempData["DamageSeverity"] = dmgSeverity;

            return PartialView(subStructViewModel);
        }

        public ActionResult SuperStructure([Optional] string bridgeid, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<DamageInspVisual> deckSlabList = GetDamageInspVisualList(5).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> concreteGirderList = GetDamageInspVisualList(6).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> steelTrussGirderList = GetDamageInspVisualList(7).Where(s => s.BridgeId == bridgeid).ToList();
            
            if (deckSlabList == null || concreteGirderList == null || steelTrussGirderList == null)
            {
                return HttpNotFound();
            }

            var superStructViewModel = new InspVisualSuperStructuresViewModel
            {
                DeckSlab = deckSlabList,
                ConcreteGirder = concreteGirderList,
                SteelTrussGirder = steelTrussGirderList,
            }; 
            
            TempData["BridgeId"] = bridgeid;
            TempData["Inspectiondate"] = inspectiondate;

            TempData["DamageType5"] = GetDamageTypeSelectList(5);
            TempData["DamageType6"] = GetDamageTypeSelectList(6);
            TempData["DamageType7"] = GetDamageTypeSelectList(7);
            
            List<SelectListItem> dmgSeverity = db.DamageSeverities.Select(c => new SelectListItem
                                                                    {
                                                                        Text = c.DamageSeverityName,
                                                                        Value = c.DamageSeverityId.ToString()
                                                                    }
                                                                ).ToList();

            //Add the first unselected item
            dmgSeverity.Insert(0, new SelectListItem { Text = "", Value = "0" });
            TempData["DamageSeverity"] = dmgSeverity;

            return PartialView(superStructViewModel);
        }

        public ActionResult Ancillaries([Optional] string bridgeid, [Optional] DateTime inspectiondate)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<DamageInspVisual> pavementList = GetDamageInspVisualList(8).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> curbAndRailingList = GetDamageInspVisualList(9).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> drainageList = GetDamageInspVisualList(10).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> bearingList = GetDamageInspVisualList(11).Where(s => s.BridgeId == bridgeid).ToList();
            List<DamageInspVisual> expansionJointList = GetDamageInspVisualList(12).Where(s => s.BridgeId == bridgeid).ToList();
            
            if (pavementList == null || curbAndRailingList == null || drainageList == null || bearingList == null || expansionJointList == null)
            {
                return HttpNotFound();
            }

            var ancillariesViewModel = new InspVisualAncillariesViewModel
            {
                Pavement = pavementList,
                CurbAndRailing = curbAndRailingList,
                Drainage = drainageList,
                Bearing = bearingList,
                ExpansionJoint = expansionJointList
            }; 
            
            TempData["BridgeId"] = bridgeid;
            TempData["Inspectiondate"] = inspectiondate;

            TempData["DamageType8"] = GetDamageTypeSelectList(8);
            TempData["DamageType9"] = GetDamageTypeSelectList(9);
            TempData["DamageType10"] = GetDamageTypeSelectList(10);
            TempData["DamageType11"] = GetDamageTypeSelectList(11);
            TempData["DamageType12"] = GetDamageTypeSelectList(12);

            List<SelectListItem> dmgSeverity = db.DamageSeverities.Select(c => new SelectListItem
                                                                    {
                                                                        Text = c.DamageSeverityName,
                                                                        Value = c.DamageSeverityId.ToString()
                                                                    }
                                                                ).ToList();

            //Add the first unselected item
            dmgSeverity.Insert(0, new SelectListItem { Text = "", Value = "0" });
            TempData["DamageSeverity"] = dmgSeverity;

            return PartialView(ancillariesViewModel);
        }

        // Damage Severity ("Major", "Minor", "No Defect", "Total Damage" from DamageSeverity table)
        public JsonResult SelectDamageSeverity()
        {
            List<DamageSeverity> damageSeverities = db.DamageSeverities.ToList();
            var list = damageSeverities.Select(d => new[] { d.DamageSeverityName, d.DamageSeverityName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        
        public string UpdateDamageSeverity(string id, string value /*, DateTime inspdate*/)
        {
            DamageInspVisual damageInspVisual = (from c in db.DamageInspVisuals
                                                 where c.Id == id
                                                 select c).FirstOrDefault();

            DamageSeverity dmgSeverity = (from c in db.DamageSeverities
                                          where c.DamageSeverityName == value
                                          select c).FirstOrDefault();

            damageInspVisual.DamageSeverityId = dmgSeverity.DamageSeverityId;
            damageInspVisual.InspectionDate = globalInspectionDate; //inspdate;
            
            db.SaveChanges();

            return value;
        }

        public string UpdateDamageType(string id, string value /*, DateTime inspdate*/)
        {
            DamageInspVisual damageInspVisual = (from c in db.DamageInspVisuals
                                                 where c.Id == id
                                                 select c).FirstOrDefault();

            DamageType damageType = (from c in db.DamageTypes
                                     where c.DamageTypeName == value
                                          select c).FirstOrDefault();

            damageInspVisual.DamageTypeId = damageType.DamageTypeId;
            damageInspVisual.InspectionDate = globalInspectionDate; // inspdate;

            db.SaveChanges();

            return value;
        }

        // Update the global inspection date variable and Tempdata using the newly selected inspection date
        public ActionResult UpdateInspectionDate(DateTime newInspectionDate)
        {
            globalInspectionDate = newInspectionDate;
            TempData["InspectionDate"] = newInspectionDate;
            
            return new EmptyResult();
        }

        // Pier and Foundation (StructureItemId = 1)
        public JsonResult SelectDamageTypePier()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 1).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Abutment and Wingwall (StructureItemId = 2)
        public JsonResult SelectDamageTypeAbutment()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 2).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Embankment (StructureItemId = 3)
        public JsonResult SelectDamageTypeEmbankment()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 3).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Rip Rap (StructureItemId = 4)
        public JsonResult SelectDamageTypeRipRap()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 4).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Deck Slab (StructureItemId = 5)
        public JsonResult SelectDamageTypeDeckSlab()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 5).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // ConcreteGirder (StructureItemId = 6)
        public JsonResult SelectDamageTypeConcreteGirder()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 6).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // SteelTrussGirder (StructureItemId = 7)
        public JsonResult SelectDamageTypeSteelTrussGirder()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 7).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Pavement (StructureItemId = 8)
        public JsonResult SelectDamageTypePavement()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 8).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Curb and Railing (StructureItemId = 9)
        public JsonResult SelectDamageTypeCurbAndRailing()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 9).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Drainage (StructureItemId = 10)
        public JsonResult SelectDamageTypeDrainage()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 10).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // Bearing (StructureItemId = 11)
        public JsonResult SelectDamageTypeBearing()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 11).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        // ExpansionJoint (StructureItemId = 12)
        public JsonResult SelectDamageTypeExpansionJoint()
        {
            List<DamageType> damageTypes = db.DamageTypes.Where(d => d.StructureItemId == 12).ToList();
            var list = damageTypes.Select(d => new[] { d.DamageTypeName, d.DamageTypeName });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDamage(DamageInspVisual damage)
        {
            // If there is an already exisiting damage entry with the same damagetype for the given bridge, then delete it
            DamageInspVisual toDelete = db.DamageInspVisuals.Where(d => d.BridgeId == damage.BridgeId 
                                                                        && d.DamageTypeId == damage.DamageTypeId
                                                                        && d.StructureItemId == damage.StructureItemId).FirstOrDefault();
            if (toDelete != null)
                db.DamageInspVisuals.Remove(toDelete);

            damage.Id = Guid.NewGuid().ToString();
            db.DamageInspVisuals.Add(damage);
            db.SaveChanges();

            return Json(damage);
        }

        //GET
        public ActionResult GetLastAddedDamage(string bridgeid)
        {
            var lastAddedDamage =  (from c in db.DamageInspVisuals
                                    where c.BridgeId == bridgeid
                                    select new
                                    {
                                        Id = c.Id,
                                        BridgeId = c.BridgeId,
                                        DamageTypeId = c.DamageTypeId,
                                        DamageSeverityId = c.DamageSeverityId,
                                        InspectionDate = c.InspectionDate
                                    }).ToList().LastOrDefault();

            return Json(lastAddedDamage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDamage(string id)
        {
            DamageInspVisual dmgInspVisual = (from c in db.DamageInspVisuals
                                              where c.Id == id
                                              select c).FirstOrDefault();
            db.DamageInspVisuals.Remove(dmgInspVisual);
            db.SaveChanges();

            return new EmptyResult();
        }
    }
}