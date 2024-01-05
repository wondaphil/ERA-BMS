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
using System.IO;

namespace ERA_BMS.Controllers
{
    //Only logged in user should have access
    [Authorize(Roles = "Admin, User")]
    public class BridgeInventoryController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: BridgeInventory
        public ActionResult Index(string id = "")
        {
            Bridge bridge = db.Bridges.Find(id);
            BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(id);
            SuperStructure superStr = db.SuperStructures.Find(id);
            Abutment abutment = db.Abutments.Find(id);
            Ancillary ancillaries = db.Ancillaries.Find(id);
            List<Pier> piers = db.Piers.Where(p => p.BridgeId == id).ToList();
            List<BridgeDoc> bridgeDoc = db.BridgeDocs.Where(d => d.BridgeId == id).ToList();
            List<BridgeMedia> bridgeMedia = db.BridgeMedias.Where(m => m.BridgeId == id)
                    .OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList(); // Sort by media type and then by description

            BridgeViewModel inventory = new BridgeViewModel()
            {
                GenInfo = genInfo,
                SuperStr = superStr,
                Abutment = abutment,
                Piers = piers,
                Ancillaries = ancillaries,
                BridgeDoc = bridgeDoc,
                BridgeMedia = bridgeMedia
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
                ViewBag.Districts = LocationModel.GetDistrictList();
                ViewBag.DistrictId = bridge.Segment.Section.DistrictId;

                // For displaying district, section, and segment names and bridge no. on reports
                ViewBag.DistrictName = bridge.Segment.Section.District.DistrictName;
                ViewBag.SectionName = bridge.Segment.Section.SectionName;
                ViewBag.SegmentName = bridge.Segment.SegmentName;
                ViewBag.BridgeName = bridge.BridgeName;

                //For general info
                TempData["RoadAlignmentId"] = (genInfo == null) ? new SelectList(db.RoadAlignmentTypes, "RoadAlignmentTypeId", "RoadAlignmentTypeName").ToList() : new SelectList(db.RoadAlignmentTypes, "RoadAlignmentTypeId", "RoadAlignmentTypeName", genInfo.RoadAlignmentId).ToList();
                TempData["UtmZoneId"] = (genInfo == null) ? new SelectList(db.UtmZoneEthiopias, "UtmZoneId", "UtmZone").ToList() : new SelectList(db.UtmZoneEthiopias, "UtmZoneId", "UtmZone", genInfo.UtmZoneId).ToList();

                //For super structure
                TempData["BridgeTypeId"] = (superStr == null) ? new SelectList(db.BridgeTypes, "BridgeTypeId", "BridgeTypeName").ToList() : new SelectList(db.BridgeTypes, "BridgeTypeId", "BridgeTypeName", superStr.BridgeId).ToList();
                TempData["GirderTypeId"] = (superStr == null) ? new SelectList(db.GirderTypes, "GirderTypeId", "GirderTypeName").ToList() : new SelectList(db.GirderTypes, "GirderTypeId", "GirderTypeName", superStr.GirderTypeId).ToList();
                TempData["SpanSupportTypeId"] = (superStr == null) ? new SelectList(db.SpanSupportTypes, "SpanSupportTypeId", "SpanSupportTypeName").ToList() : new SelectList(db.SpanSupportTypes, "SpanSupportTypeId", "SpanSupportTypeName", superStr.SpanSupportTypeId).ToList();
                TempData["DeckSlabTypeId"] = (superStr == null) ? new SelectList(db.DeckSlabTypes, "DeckSlabTypeId", "DeckSlabTypeName").ToList() : new SelectList(db.DeckSlabTypes, "DeckSlabTypeId", "DeckSlabTypeName", superStr.DeckSlabTypeId).ToList();

                //For abutment and foundation
                TempData["AbutmentTypeIdA1"] = (abutment == null) ? new SelectList(db.AbutmentTypes, "AbutmentTypeId", "AbutmentTypeName").ToList() : new SelectList(db.AbutmentTypes, "AbutmentTypeId", "AbutmentTypeName", abutment.AbutmentTypeIdA1).ToList();
                TempData["AbutmentTypeIdA2"] = (abutment == null) ? new SelectList(db.AbutmentTypes, "AbutmentTypeId", "AbutmentTypeName").ToList() : new SelectList(db.AbutmentTypes, "AbutmentTypeId", "AbutmentTypeName", abutment.AbutmentTypeIdA2).ToList();
                TempData["FoundationTypeIdA1"] = (abutment == null) ? new SelectList(db.FoundationTypes, "FoundationTypeId", "FoundationTypeName").ToList() : new SelectList(db.FoundationTypes, "FoundationTypeId", "FoundationTypeName", abutment.FoundationTypeIdA1).ToList();
                TempData["FoundationTypeIdA2"] = (abutment == null) ? new SelectList(db.FoundationTypes, "FoundationTypeId", "FoundationTypeName").ToList() : new SelectList(db.FoundationTypes, "FoundationTypeId", "FoundationTypeName", abutment.FoundationTypeIdA2).ToList();
                TempData["SoilTypeIdA1"] = (abutment == null) ? new SelectList(db.SoilTypes, "SoilTypeId", "SoilTypeName").ToList() : new SelectList(db.SoilTypes, "SoilTypeId", "SoilTypeName", abutment.SoilTypeA1).ToList();
                TempData["SoilTypeIdA2"] = (abutment == null) ? new SelectList(db.SoilTypes, "SoilTypeId", "SoilTypeName").ToList() : new SelectList(db.SoilTypes, "SoilTypeId", "SoilTypeName", abutment.SoilTypeA2).ToList();

                //For ancillaries
                TempData["ExpansionJointTypeId"] = (ancillaries == null) ? new SelectList(db.ExpansionJointTypes, "ExpansionJointTypeId", "ExpansionJointTypeName").ToList() : new SelectList(db.ExpansionJointTypes, "ExpansionJointTypeId", "ExpansionJointTypeName", ancillaries.ExpansionJointTypeId).ToList();
                TempData["GuardRailingTypeId"] = (ancillaries == null) ? new SelectList(db.GuardRailingTypes, "GuardRailingTypeId", "GuardRailingTypeName").ToList() : new SelectList(db.GuardRailingTypes, "GuardRailingTypeId", "GuardRailingTypeName", ancillaries.GuardRailingTypeId).ToList();
                TempData["AbutmentBearingTypeId"] = (ancillaries == null) ? new SelectList(db.BearingTypes, "BearingTypeId", "BearingTypeName").ToList() : new SelectList(db.BearingTypes, "BearingTypeId", "BearingTypeName", ancillaries.BearingType).ToList();
                TempData["PiersBearingTypeId"] = (ancillaries == null) ? new SelectList(db.BearingTypes, "BearingTypeId", "BearingTypeName").ToList() : new SelectList(db.BearingTypes, "BearingTypeId", "BearingTypeName", ancillaries.BearingType1).ToList();
                TempData["SurfaceTypeId"] = (ancillaries == null) ? new SelectList(db.SurfaceTypes, "SurfaceTypeId", "SurfaceTypeName").ToList() : new SelectList(db.SurfaceTypes, "SurfaceTypeId", "SurfaceTypeName", ancillaries.SurfaceTypeId).ToList();

                //For pier and foundation drop down list
                TempData["PierTypesSelectList"] = new SelectList(db.PierTypes, "PierTypeId", "PierTypeName").ToList();
                TempData["FoundationTypesSelectList"] = new SelectList(db.FoundationTypes, "FoundationTypeId", "FoundationTypeName").ToList();

                // For pier and foundation json object in jquery
                TempData["PierTypeList"] = db.PierTypes.Select(c => new { Id = c.PierTypeId, Name = c.PierTypeName }).ToList();
                TempData["FoundationTypeList"] = db.FoundationTypes.Select(c => new { Id = c.FoundationTypeId, Name = c.FoundationTypeName }).ToList();

                //For bridge doc
                string bridgeno = bridge.BridgeNo.ToString();
                string segmentName = bridge.Segment.SegmentName.ToString();
                string sectionName = bridge.Segment.Section.SectionName.ToString();
                string districtName = bridge.Segment.Section.District.DistrictName.ToString();

                //For bridge doc
                TempData["DocTypes"] = new SelectList(db.DocTypes, "DocTypeId", "DocTypeName").ToList();
                TempData["DocPath"] = "/media/bridge/" + districtName.Trim() + "/" + sectionName.Trim() + "/" + segmentName.Trim() + "/" + bridgeno.Trim() + "/";

                //For bridge media
                TempData["MediaTypes"] = new SelectList(db.MediaTypes, "MediaTypeId", "MediaTypeName").ToList();
                TempData["MediaPath"] = "/media/bridge/" + districtName.Trim() + "/" + sectionName.Trim() + "/" + segmentName.Trim() + "/" + bridgeno.Trim() + "/";

                return View(inventory);
            }
        }

        //GET
        public ActionResult GeneralInfo([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeid);

            TempData["RoadAlignmentId"] = db.RoadAlignmentTypes.Select(c => new SelectListItem
                                            { Text = c.RoadAlignmentTypeName, Value = c.RoadAlignmentTypeId.ToString() }).ToList();
            TempData["UtmZoneId"] = db.UtmZoneEthiopias.Select(c => new SelectListItem
                                        { Text = c.UtmZone.ToString(), Value = c.UtmZoneId.ToString() }).ToList();
            
            TempData["BridgeId"] = bridgeid;
            
            //If the bridge does not have a GeneralInfo entry, return a "New" view
            //But if the bridge already has a GeneralInfo entry, return a "Edit" view
            if (genInfo == null)
            {
                return PartialView("GeneralInfoNew", genInfo);
                //TempData["RoadAlignmentId"] = new SelectList(db.RoadAlignmentTypes, "RoadAlignmentTypeId", "RoadAlignmentTypeName", genInfo.RoadAlignmentId);
            }

            return PartialView("GeneralInfo", genInfo);
        }

        public BridgeGeneralInfo GeneralInfoSave([Bind(Include = "BridgeId,KMFromAddis,XCoord,YCoord,UtmZoneId,BridgeLength,BridgeWidth,RiverWidth,PresentWaterLevel,HighestWaterLevel,DesignCapacity,BearingCapacity,Topography,Altitude,RoadAlignmentId,ConstructionYear,ReplacedYear,Before1935,Contractor,Designer,ConstructionCost,AssetReplacementCost,SafetySign,DetourPossible,Remark")] BridgeGeneralInfo genInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genInfo).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "BridgeInventory");
            }

            return genInfo;
        }

        public ActionResult GeneralInfoNew([Bind(Include = "BridgeId,KMFromAddis,XCoord,YCoord,UtmZoneId,BridgeLength,BridgeWidth,RiverWidth,PresentWaterLevel,HighestWaterLevel,DesignCapacity,BearingCapacity,Topography,Altitude,RoadAlignmentId,ConstructionYear,ReplacedYear,Before1935,Contractor,Designer,ConstructionCost,AssetReplacementCost,SafetySign,DetourPossible,Remark")] BridgeGeneralInfo genInfo)
        {
            if (ModelState.IsValid)
            {
                db.BridgeGeneralInfoes.Add(genInfo);
                db.SaveChanges();
            }

            TempData["RoadAlignmentId"] = db.RoadAlignmentTypes.Select(c => new SelectListItem
                                                { Text = c.RoadAlignmentTypeName, Value = c.RoadAlignmentTypeId.ToString() }).ToList();
            TempData["UtmZoneId"] = db.UtmZoneEthiopias.Select(c => new SelectListItem
                                        { Text = c.UtmZone.ToString(), Value = c.UtmZoneId.ToString() }).ToList();

            return View(genInfo);
        }

        //GET
        public ActionResult SuperStructure([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SuperStructure superStructure = db.SuperStructures.Find(bridgeid);

            TempData["BridgeTypeId"] = db.BridgeTypes.Select(c => new SelectListItem
                                            { Text = c.BridgeTypeName, Value = c.BridgeTypeId.ToString() }).ToList();
            TempData["GirderTypeId"] = db.GirderTypes.Select(c => new SelectListItem
                                             { Text = c.GirderTypeName, Value = c.GirderTypeId.ToString() }).ToList();
            TempData["SpanSupportTypeId"] = db.SpanSupportTypes.Select(c => new SelectListItem
                                             { Text = c.SpanSupportTypeName, Value = c.SpanSupportTypeId.ToString() }).ToList();
            TempData["DeckSlabTypeId"] = db.DeckSlabTypes.Select(c => new SelectListItem
                                            { Text = c.DeckSlabTypeName, Value = c.DeckSlabTypeId.ToString() }).ToList();

            TempData["BridgeId"] = bridgeid;

            //If the bridge does not have a superStructure entry, return a "New" view
            //But if the bridge already has a superStructure entry, return a "Edit" view
            if (superStructure == null)
            {
                return PartialView("SuperStructureNew", superStructure);
            }

            return PartialView("SuperStructure", superStructure);
        }

        public SuperStructure SuperStructureSave([Bind(Include = "BridgeId,BridgeTypeId,GirderTypeId,NoOfSpan,SpanLengthComposition,TotalSpanLength,CarriageWayWidth,SideWalkWidth,NoOfLane,SpanSupportTypeId,DeckSlabTypeId,SlabThickness,NoOfGirder,GirderDepth,SpacingGirder,GirderBoxWidth")] SuperStructure superStructure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(superStructure).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "BridgeInventory");
            }

            return superStructure;
        }

        public ActionResult SuperStructureNew([Bind(Include = "BridgeId,BridgeTypeId,GirderTypeId,NoOfSpan,SpanLengthComposition,TotalSpanLength,CarriageWayWidth,SideWalkWidth,NoOfLane,SpanSupportTypeId,DeckSlabTypeId,SlabThickness,NoOfGirder,GirderDepth,SpacingGirder,GirderBoxWidth")] SuperStructure superStructure)
        {
            if (ModelState.IsValid)
            {
                db.SuperStructures.Add(superStructure);
                db.SaveChanges();
            }

            TempData["BridgeTypeId"] = db.BridgeTypes.Select(c => new SelectListItem
                                            { Text = c.BridgeTypeName, Value = c.BridgeTypeId.ToString() }).ToList();
            TempData["GirderTypeId"] = db.GirderTypes.Select(c => new SelectListItem
                                            { Text = c.GirderTypeName, Value = c.GirderTypeId.ToString() }).ToList();
            TempData["SpanSupportTypeId"] = db.SpanSupportTypes.Select(c => new SelectListItem
                                                { Text = c.SpanSupportTypeName, Value = c.SpanSupportTypeId.ToString() }).ToList();
            TempData["DeckSlabTypeId"] = db.DeckSlabTypes.Select(c => new SelectListItem
                                                { Text = c.DeckSlabTypeName, Value = c.DeckSlabTypeId.ToString() }).ToList();

            return View(superStructure);
        }

        //GET
        public ActionResult Abutment([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Abutment abutment = db.Abutments.Find(bridgeid);

            TempData["AbutmentTypeIdA1"] = db.AbutmentTypes.Select(c => new SelectListItem
                                                    { Text = c.AbutmentTypeName, Value = c.AbutmentTypeId.ToString() }).ToList();
            TempData["AbutmentTypeIdA2"] = db.AbutmentTypes.Select(c => new SelectListItem
                                                    { Text = c.AbutmentTypeName, Value = c.AbutmentTypeId.ToString() }).ToList();
            TempData["FoundationTypeIdA1"] = db.FoundationTypes.Select(c => new SelectListItem
                                                    { Text = c.FoundationTypeName, Value = c.FoundationTypeId.ToString() }).ToList();
            TempData["FoundationTypeIdA2"] = db.FoundationTypes.Select(c => new SelectListItem
                                                    { Text = c.FoundationTypeName, Value = c.FoundationTypeId.ToString() }).ToList();
            TempData["SoilTypeIdA1"] = db.SoilTypes.Select(c => new SelectListItem
                                                    { Text = c.SoilTypeName, Value = c.SoilTypeId.ToString() }).ToList();
            TempData["SoilTypeIdA2"] = db.SoilTypes.Select(c => new SelectListItem
                                                    { Text = c.SoilTypeName, Value = c.SoilTypeId.ToString() }).ToList();

            TempData["BridgeId"] = bridgeid;

            //If the bridge does not have an abutment entry, return a "New" view
            //But if the bridge already has an abutment entry, return a "Edit" view
            if (abutment == null)
            {
                return PartialView("AbutmentNew", abutment);
            }

            return PartialView("Abutment", abutment);
        }

        public Abutment AbutmentSave([Bind(Include = "BridgeId,AbutmentTypeIdA1,AbutmentTypeIdA2,AbutmentHeightA1,AbutmentHeightA2,AbutmentWidthA1,AbutmentWidthA2,WingWallLengthA1,WingWallLengthA2,FoundationTypeIdA1,FoundationTypeIdA2,FoundationSizeA1,FoundationSizeA2,NoOfAbutmentPilesA1,NoOfAbutmentPilesA2,AbutmentPileDepthA1,AbutmentPileDepthA2,SoilTypeA1,SoilTypeA2,NoOfpier")] Abutment abutment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(abutment).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "BridgeInventory");
            }

            return abutment;
        }

        public ActionResult AbutmentNew([Bind(Include = "BridgeId,AbutmentTypeIdA1,AbutmentTypeIdA2,AbutmentHeightA1,AbutmentHeightA2,AbutmentWidthA1,AbutmentWidthA2,WingWallLengthA1,WingWallLengthA2,FoundationTypeIdA1,FoundationTypeIdA2,FoundationSizeA1,FoundationSizeA2,NoOfAbutmentPilesA1,NoOfAbutmentPilesA2,AbutmentPileDepthA1,AbutmentPileDepthA2,SoilTypeA1,SoilTypeA2,NoOfpier")] Abutment abutment)
        {
            if (ModelState.IsValid)
            {
                db.Abutments.Add(abutment);
                db.SaveChanges();
            }

            TempData["AbutmentTypeIdA1"] = db.AbutmentTypes.Select(c => new SelectListItem
                                                    { Text = c.AbutmentTypeName, Value = c.AbutmentTypeId.ToString() }).ToList();
            TempData["AbutmentTypeIdA2"] = db.AbutmentTypes.Select(c => new SelectListItem
                                                    { Text = c.AbutmentTypeName, Value = c.AbutmentTypeId.ToString() }).ToList();
            TempData["FoundationTypeIdA1"] = db.FoundationTypes.Select(c => new SelectListItem
                                                    { Text = c.FoundationTypeName, Value = c.FoundationTypeId.ToString() }).ToList();
            TempData["FoundationTypeIdA2"] = db.FoundationTypes.Select(c => new SelectListItem
                                                    { Text = c.FoundationTypeName, Value = c.FoundationTypeId.ToString() }).ToList();
            TempData["SoilTypeIdA1"] = db.SoilTypes.Select(c => new SelectListItem
                                                    { Text = c.SoilTypeName, Value = c.SoilTypeId.ToString() }).ToList();
            TempData["SoilTypeIdA2"] = db.SoilTypes.Select(c => new SelectListItem
                                                    { Text = c.SoilTypeName, Value = c.SoilTypeId.ToString() }).ToList();

            return View(abutment);
        }

        //GET
        public ActionResult Pier([Optional] string bridgeid /* drop down value */)
        {
            List<Pier> pierList = db.Piers.Where(s => s.BridgeId == bridgeid).ToList();

            TempData["BridgeId"] = bridgeid;

            // For drop down list
            TempData["PierTypesSelectList"] = new SelectList(db.PierTypes, "PierTypeId", "PierTypeName").ToList();
            TempData["FoundationTypesSelectList"] = new SelectList(db.FoundationTypes, "FoundationTypeId", "FoundationTypeName").ToList();

            // For json object in jquery
            TempData["PierTypeList"] = db.PierTypes.Select(c => new { Id = c.PierTypeId, Name = c.PierTypeName }).ToList();
            TempData["FoundationTypeList"] = db.FoundationTypes.Select(c => new { Id = c.FoundationTypeId, Name = c.FoundationTypeName }).ToList();

            return PartialView(pierList);
        }

        [HttpPost]
        public JsonResult PierInsert(Pier pier)
        {
            pier.Id = Guid.NewGuid().ToString();
            db.Piers.Add(pier);
            db.SaveChanges();

            return Json(pier);
        }

        [HttpPost]
        public ActionResult PierUpdate(Pier pier)
        {
            Pier updatedPier = (from c in db.Piers
                                where c.Id == pier.Id
                                select c).FirstOrDefault();

            updatedPier.PierNo = pier.PierNo;
            updatedPier.PierNo = pier.PierNo;
            updatedPier.PierTypeId = pier.PierTypeId;
            updatedPier.HeightOfPier = pier.HeightOfPier;
            updatedPier.WidthOfPier = pier.WidthOfPier;
            updatedPier.FoundationTypeId = pier.FoundationTypeId;
            updatedPier.FoundationDimension = pier.FoundationDimension;
            updatedPier.NoOfPierPiles = pier.NoOfPierPiles;
            updatedPier.PierPileDepth = pier.PierPileDepth;

            db.SaveChanges();

            return new EmptyResult();
        }

        //GET
        public ActionResult GetLastPier(string bridgeid)
        {
            var lastPier = (from c in db.Piers
                            where c.BridgeId == bridgeid
                            select new
                            {
                                Id = c.Id,
                                BridgeId = c.BridgeId,
                            }).ToList().LastOrDefault();

            return Json(lastPier, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PierDelete(string id)
        {
            Pier pier = db.Piers.Find(id);
            if (pier != null)
            {
                db.Piers.Remove(pier);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        //GET
        public ActionResult Ancillary([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ancillary ancillary = db.Ancillaries.Find(bridgeid);

            TempData["ExpansionJointTypeId"] = db.ExpansionJointTypes.Select(c => new SelectListItem
                                                    { Text = c.ExpansionJointTypeName, Value = c.ExpansionJointTypeId.ToString() }).ToList();
            TempData["GuardRailingTypeId"] = db.GuardRailingTypes.Select(c => new SelectListItem
                                                    { Text = c.GuardRailingTypeName, Value = c.GuardRailingTypeId.ToString() }).ToList();
            TempData["AbutmentBearingTypeId"] = db.BearingTypes.Select(c => new SelectListItem
                                                    { Text = c.BearingTypeName, Value = c.BearingTypeId.ToString() }).ToList();
            TempData["PiersBearingTypeId"] = db.BearingTypes.Select(c => new SelectListItem
                                                    { Text = c.BearingTypeName, Value = c.BearingTypeId.ToString() }).ToList();
            TempData["SurfaceTypeId"] = db.SurfaceTypes.Select(c => new SelectListItem
                                                    { Text = c.SurfaceTypeName, Value = c.SurfaceTypeId.ToString() }).ToList();

            TempData["BridgeId"] = bridgeid;

            //If the bridge does not have an ancillary entry, return a "New" view
            //But if the bridge already has an ancillary entry, return a "Edit" view
            if (ancillary == null)
            {
                return PartialView("AncillaryNew", ancillary);
            }

            return PartialView("Ancillary", ancillary);
        }

        public Ancillary AncillarySave([Bind(Include = "BridgeId,ExpansionJointTypeId,GuardRailingTypeId,AbutmentBearingTypeId,PiersBearingTypeId,SurfaceTypeId")] Ancillary ancillary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ancillary).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "BridgeInventory");
            }

            return ancillary;
        }

        public ActionResult AncillaryNew([Bind(Include = "BridgeId,ExpansionJointTypeId,GuardRailingTypeId,AbutmentBearingTypeId,PiersBearingTypeId,SurfaceTypeId")] Ancillary ancillary)
        {
            if (ModelState.IsValid)
            {
                db.Ancillaries.Add(ancillary);
                db.SaveChanges();
            }

            TempData["ExpansionJointTypeId"] = db.ExpansionJointTypes.Select(c => new SelectListItem
                                                    { Text = c.ExpansionJointTypeName, Value = c.ExpansionJointTypeId.ToString() }).ToList();
            TempData["GuardRailingTypeId"] = db.GuardRailingTypes.Select(c => new SelectListItem
                                                    { Text = c.GuardRailingTypeName, Value = c.GuardRailingTypeId.ToString() }).ToList();
            TempData["AbutmentBearingTypeId"] = db.BearingTypes.Select(c => new SelectListItem
                                                    { Text = c.BearingTypeName, Value = c.BearingTypeId.ToString() }).ToList();
            TempData["PiersBearingTypeId"] = db.BearingTypes.Select(c => new SelectListItem
                                                    { Text = c.BearingTypeName, Value = c.BearingTypeId.ToString() }).ToList();
            TempData["SurfaceTypeId"] = db.SurfaceTypes.Select(c => new SelectListItem
                                                    { Text = c.SurfaceTypeName, Value = c.SurfaceTypeId.ToString() }).ToList();

            return View(ancillary);
        }

        //GET
        public ActionResult BridgeDoc([Optional] string bridgeid /* drop down value */)
        {
            //DocTypeId 1 for History, 2 for Bearing Capacity
            List<BridgeDoc> bridgeDocList = (from br in db.BridgeDocs
                                             where br.BridgeId == bridgeid
                                             select br).OrderBy(d => d.DocTypeId).ToList();
                                            // Sort by doc type

            TempData["DocTypes"] = db.DocTypes.Select(c => new SelectListItem
                                        { Text = c.DocTypeName, Value = c.DocTypeId.ToString() }).ToList(); ;
            TempData["BridgeId"] = bridgeid;

            /**** 
               e.g. DocPath =  "/media/bridge/Alemgena/Modjo/Addis - Modjo/A1-1-001/history.pdf" 
            ****/

            string bridgeno = db.Bridges.Find(bridgeid).BridgeNo.ToString();
            string segmentName = db.Bridges.Find(bridgeid).Segment.SegmentName.ToString();
            string sectionName = db.Bridges.Find(bridgeid).Segment.Section.SectionName.ToString();
            string districtName = db.Bridges.Find(bridgeid).Segment.Section.District.DistrictName.ToString();
            
            TempData["DocPath"] = "/media/bridge/" + districtName.Trim() + "/" + sectionName.Trim() + "/" + segmentName.Trim() + "/" + bridgeno.Trim() + "/";

            return PartialView(bridgeDocList);
        }

        [HttpPost]
        public JsonResult BridgeDocInsert(BridgeDoc bridgeDoc)
        {
            // Save document path and description to database
            bridgeDoc.Id = Guid.NewGuid().ToString();
            db.BridgeDocs.Add(bridgeDoc);
            db.SaveChanges();

            //Upload file to server
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(bridgeDoc.DocFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(bridgeDoc.DocFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);
                ViewBag.Success = true;
            }

            return Json(bridgeDoc);
        }

        [HttpPost]
        public ActionResult BridgeDocUpdate(BridgeDoc bridgeDoc)
        {
            BridgeDoc updatedBridgeDoc = db.BridgeDocs.Find(bridgeDoc.Id);

            updatedBridgeDoc.Description = bridgeDoc.Description;
            updatedBridgeDoc.DocFilePath = bridgeDoc.DocFilePath;
            updatedBridgeDoc.DocTypeId = bridgeDoc.DocTypeId;

            db.SaveChanges();

            //Upload file to server
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(bridgeDoc.DocFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(bridgeDoc.DocFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);
                ViewBag.Success = true;
            }

            return new EmptyResult();
        }

        //GET
        public ActionResult GetLastBridgeDoc(string bridgeid)
        {
            var lastBridgeDoc = (from c in db.BridgeDocs
                                 where c.BridgeId == bridgeid
                                 select new
                                 {
                                     Id = c.Id,
                                     BridgeId = c.BridgeId,
                                 }).ToList().LastOrDefault();

            return Json(lastBridgeDoc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BridgeDocDelete(string id)
        {
            BridgeDoc bridgeDoc = db.BridgeDocs.Find(id);
            if (bridgeDoc != null)
            {
                db.BridgeDocs.Remove(bridgeDoc);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        //GET
        public ActionResult BridgeMedia([Optional] string bridgeid)
        {
            //MediaTypeId 1 for image, 2 for video, 3 for drawing, 6 for damage image, 7 for damage video
            List<BridgeMedia> bridgeMediaList = (from br in db.BridgeMedias
                                                 where br.BridgeId == bridgeid
                                                 select br).OrderBy(m => m.MediaTypeId).ThenBy(m => m.Description).ToList();
                                                // Sort by media type and then by description

            TempData["MediaTypes"] = db.MediaTypes.Select(c => new SelectListItem
                                        { Text = c.MediaTypeName, Value = c.MediaTypeId.ToString() }).ToList(); ;
            TempData["BridgeId"] = bridgeid;

            /**** 
                e.g. MediaPath =  "/media/bridge/Alemgena/Modjo/Addis-Modjo/A1-1-001/down stream.jpg" 
            ****/

            string bridgeno = db.Bridges.Find(bridgeid).BridgeNo.ToString();
            string segmentName = db.Bridges.Find(bridgeid).Segment.SegmentName.ToString();
            string sectionName = db.Bridges.Find(bridgeid).Segment.Section.SectionName.ToString();
            string districtName = db.Bridges.Find(bridgeid).Segment.Section.District.DistrictName.ToString();
            
            TempData["MediaPath"] = "/media/bridge/" + districtName.Trim() + "/" + sectionName.Trim() + "/" + segmentName.Trim() + "/" + bridgeno.Trim() + "/";

            return PartialView(bridgeMediaList);
        }

        [HttpPost]
        public JsonResult BridgeMediaInsert(BridgeMedia bridgeMedia)
        {
            // Save media path and description to database
            bridgeMedia.Id = Guid.NewGuid().ToString();
            db.BridgeMedias.Add(bridgeMedia);
            db.SaveChanges();

            //Upload file to server
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(bridgeMedia.MediaFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(bridgeMedia.MediaFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);
                ViewBag.Success = true;
            }

            return Json(bridgeMedia);
        }

        [HttpPost]
        public ActionResult BridgeMediaUpdate(BridgeMedia bridgeMedia)
        {
            BridgeMedia updatedBridgeMedia = db.BridgeMedias.Find(bridgeMedia.Id);

            updatedBridgeMedia.Description = bridgeMedia.Description;
            updatedBridgeMedia.MediaFilePath = bridgeMedia.MediaFilePath;
            updatedBridgeMedia.MediaTypeId = bridgeMedia.MediaTypeId;
            updatedBridgeMedia.ImageNo = bridgeMedia.ImageNo;

            db.SaveChanges();

            //Upload file to server
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(bridgeMedia.MediaFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(bridgeMedia.MediaFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);
                ViewBag.Success = true;
            }

            return new EmptyResult();
        }

        //GET
        public ActionResult GetLastBridgeMedia(string bridgeid)
        {
            var lastBridgeMedia = (from c in db.BridgeMedias
                                   where c.BridgeId == bridgeid
                                   select new
                                   {
                                       Id = c.Id,
                                       BridgeId = c.BridgeId,
                                   }).ToList().LastOrDefault();

            return Json(lastBridgeMedia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BridgeMediaDelete(string id)
        {
            BridgeMedia bridgeMedia = db.BridgeMedias.Find(id);
            if (bridgeMedia != null)
            {
                db.BridgeMedias.Remove(bridgeMedia);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

        //GET
        [Authorize(Roles = "Admin")]
        public ActionResult BearingCapacity()
        {
            List<Bridge> bridgeList = db.Bridges.ToList();
            List<BridgeGeneralInfo> genInfo = db.BridgeGeneralInfoes.Where(br => br.BearingCapacity == true).ToList();
            List<BridgeDoc> bridgeDoc = db.BridgeDocs.Where(br => br.DocTypeId == 1).ToList();

            var bridges = (from br in bridgeList
                          join gen in genInfo on br.BridgeId equals gen.BridgeId into table1
                          from gen in table1.ToList()
                          join doc in bridgeDoc on gen.BridgeId equals doc.BridgeId into table2
                          from doc in table2.ToList()
                          select new BearingCapacityViewModel
                          {
                              Bridge = br,
                              GeneralInfo = gen,
                              BridgeDoc = doc
                          }).ToList();

            return View(bridges);
        }

        ///GET
        //public ActionResult BridgeImage([Optional] string bridgeid)
        //{
        //    //MediaTypeId = 1 for image
        //    List<BridgeMedia> bridgeImageList = (from s in db.BridgeMedias
        //                                         where s.BridgeId == bridgeid && s.MediaTypeId == 1
        //                                         select s).ToList();

        //    return PartialView(bridgeImageList);
        //}

        ////GET
        //public ActionResult BridgeVideo([Optional] string bridgeid /* drop down value */)
        //{
        //    //MediaTypeId = 2 for video
        //    List<BridgeMedia> bridgeVideoList = (from s in db.BridgeMedias
        //                                         where s.BridgeId == bridgeid && s.MediaTypeId == 2
        //                                         select s).ToList();

        //    return PartialView(bridgeVideoList);
        //}

        ////GET
        //public ActionResult BridgeDrawing([Optional] string bridgeid /* drop down value */)
        //{
        //    //MediaTypeId = 3 for drawing
        //    List<BridgeMedia> bridgeDrawingList = (from s in db.BridgeMedias
        //                                           where s.BridgeId == bridgeid && s.MediaTypeId == 3
        //                                           select s).ToList();

        //    return PartialView(bridgeDrawingList);
        //}

        [HttpPost]
        //public bool _FileUpload(HttpPostedFileBase file)
        public bool _FileUpload()
        {
            //var excelUtility = new ExcelUtilityService();
            bool success = false;
            //Upload file to server
            //if (file != null)
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                file.SaveAs(path + Path.GetFileName(file.FileName));
                ViewBag.Message = "File uploaded successfully.";
                success = true;
            }
            //return Json(bridgeDoc);
            return success;
        }

        // GET
        public ActionResult FileUpload()
        {
            return View();
        }
    }
}