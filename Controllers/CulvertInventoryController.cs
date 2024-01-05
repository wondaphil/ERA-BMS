using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    //Only logged in user should have access
    [Authorize(Roles = "Admin, User")]
    public class CulvertInventoryController : Controller
    {
        BMSEntities db = new BMSEntities();

        // GET: CulvertInventory
        public ActionResult Index(string id = "")
        {
            Culvert culvert = db.Culverts.Find(id);
            CulvertGeneralInfo genInfo = db.CulvertGeneralInfoes.Find(id);
            CulvertStructure culvertStr = db.CulvertStructures.Find(id);
            List<CulvertMedia> culvertMedia = db.CulvertMedias.Where(m => m.CulvertId == id)
                .OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList(); // Sort by media type and then by description

            CulvertViewModel inventory = new CulvertViewModel()
            {
                GenInfo = genInfo,
                CulvertStr = culvertStr,
                CulvertMedia = culvertMedia,
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

                // For a dropdownlist that has a list of segments in the current section and the current segment selected
                ViewBag.Segments = LocationModel.GetSegmentList(culvert.Segment.SectionId);
                ViewBag.SegmentId = culvert.Segment.SegmentId;

                // For a dropdownlist that has a list of sections in the current district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(culvert.Segment.Section.DistrictId);
                ViewBag.SectionId = culvert.Segment.SectionId;

                // For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.Districts = LocationModel.GetDistrictList();
                ViewBag.DistrictId = culvert.Segment.Section.DistrictId;

                // For displaying district, section, and segment names and culvert no. on reports
                ViewBag.DistrictName = culvert.Segment.Section.District.DistrictName;
                ViewBag.SectionName = culvert.Segment.Section.SectionName;
                ViewBag.SegmentName = culvert.Segment.SegmentName;

                //For general info
                TempData["ParapetMaterialTypeId"] = (genInfo == null) ? new SelectList(db.ParapetMaterialTypes, "MaterialTypeId", "MaterialTypeName").ToList() : new SelectList(db.ParapetMaterialTypes, "MaterialTypeId", "MaterialTypeName", genInfo.ParapetMaterialTypeId).ToList();
                TempData["UtmZoneId"] = (genInfo == null) ? new SelectList(db.UtmZoneEthiopias, "UtmZoneId", "UtmZone").ToList() : new SelectList(db.UtmZoneEthiopias, "UtmZoneId", "UtmZone", genInfo.UtmZoneId).ToList();

                //For culvert structure
                TempData["CulvertTypeId"] = (culvertStr == null) ? new SelectList(db.CulvertTypes, "CulvertTypeId", "CulvertTypeName").ToList() : new SelectList(db.CulvertTypes, "CulvertTypeId", "CulvertTypeName", culvertStr.CulvertId).ToList();
                TempData["AbutmentTypeId"] = (culvertStr == null) ? new SelectList(db.AbutmentTypes, "AbutmentTypeId", "AbutmentTypeName").ToList() : new SelectList(db.AbutmentTypes, "AbutmentTypeId", "AbutmentTypeName", culvertStr.AbutmentTypeId).ToList();
                TempData["EndWallTypeIdIn"] = (culvertStr == null) ? new SelectList(db.culEndWallTypes, "EndWallTypeId", "EndWallTypeName").ToList() : new SelectList(db.culEndWallTypes, "EndWallTypeId", "EndWallTypeName", culvertStr.EndWallTypeIdIn).ToList();
                TempData["EndWallTypeIdOut"] = (culvertStr == null) ? new SelectList(db.culEndWallTypes, "EndWallTypeId", "EndWallTypeName").ToList() : new SelectList(db.culEndWallTypes, "EndWallTypeId", "EndWallTypeName", culvertStr.EndWallTypeIdOut).ToList();

                //For culvert media
                string culvertno = culvert.CulvertNo.ToString();
                string segmentName = culvert.Segment.SegmentName.ToString();
                string sectionName = culvert.Segment.Section.SectionName.ToString();
                string districtName = culvert.Segment.Section.District.DistrictName.ToString();

                TempData["MediaTypes"] = new SelectList(db.MediaTypes, "MediaTypeId", "MediaTypeName").ToList();
                TempData["MediaPath"] = "/media/culvert/" + districtName.Trim() + "/" + sectionName.Trim() + "/" + segmentName.Trim() + "/" + culvertno.Trim() + "/";

                return View(inventory);
            }
        }

        //GET
        public ActionResult GeneralInfo([Optional] string culvertid /* drop down value */)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulvertGeneralInfo culvertGenInfo = db.CulvertGeneralInfoes.Find(culvertid);

            TempData["ParapetMaterialTypeId"] = db.ParapetMaterialTypes.Select(c => new SelectListItem
                                                    { Text = c.MaterialTypeName, Value = c.MaterialTypeId.ToString() }).ToList();
            TempData["UtmZoneId"] = db.UtmZoneEthiopias.Select(c => new SelectListItem
                                        { Text = c.UtmZone.ToString(), Value = c.UtmZoneId.ToString() }).ToList();
            
            TempData["CulvertId"] = culvertid;
            
            //If the culvert does not have a GeneralInfo entry, return a "New" view
            //But if the culvert already has a GeneralInfo entry, return a "Edit" view
            if (culvertGenInfo == null)
            {
                return PartialView("GeneralInfoNew", culvertGenInfo);
            }

            return PartialView("GeneralInfo", culvertGenInfo);
        }

        public CulvertGeneralInfo GeneralInfoSave([Bind(Include = "CulvertId,KMFromAddis,XCoord,YCoord,UtmZoneId,ConstructionYear,Designer,Contractor,Supervisor,ConstructionCost,AssetReplacementCost,DetourPossible,Altitude,RoadWidth,HeadWallLength,FillHeight,ParapetMaterialTypeId,ParapetLength,NoOfLanes,Remark")] CulvertGeneralInfo genInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genInfo).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return genInfo;
        }

        public ActionResult GeneralInfoNew([Bind(Include = "CulvertId,KMFromAddis,XCoord,YCoord,UtmZoneId,ConstructionYear,Designer,Contractor,Supervisor,ConstructionCost,AssetReplacementCost,DetourPossible,Altitude,RoadWidth,HeadWallLength,FillHeight,ParapetMaterialTypeId,ParapetLength,NoOfLanes,Remark")] CulvertGeneralInfo genInfo)
        {
            if (ModelState.IsValid)
            {
                db.CulvertGeneralInfoes.Add(genInfo);
                db.SaveChanges();
            }
            TempData["ParapetMaterialTypeId"] = db.ParapetMaterialTypes.Select(c => new SelectListItem
                                                        { Text = c.MaterialTypeName, Value = c.MaterialTypeId.ToString() }).ToList();
            TempData["UtmZoneId"] = db.UtmZoneEthiopias.Select(c => new SelectListItem
                                        { Text = c.UtmZone.ToString(), Value = c.UtmZoneId.ToString() }).ToList();

            return View(genInfo);
        }

        //GET
        public ActionResult StructuralInfo([Optional] string culvertid /* drop down value */)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulvertStructure culvertStructure = db.CulvertStructures.Find(culvertid);

            TempData["AbutmentTypeId"] = db.AbutmentTypes.Select(c => new SelectListItem
                                                { Text = c.AbutmentTypeName, Value = c.AbutmentTypeId.ToString() }).ToList();
            TempData["EndWallTypeIdIn"] = db.culEndWallTypes.Select(c => new SelectListItem
                                                { Text = c.EndWallTypeName, Value = c.EndWallTypeId.ToString() }).ToList();
            TempData["EndWallTypeIdOut"] = db.culEndWallTypes.Select(c => new SelectListItem
                                                { Text = c.EndWallTypeName, Value = c.EndWallTypeId.ToString() }).ToList();
            TempData["CulvertTypeId"] = db.CulvertTypes.Select(c => new SelectListItem
                                                { Text = c.CulvertTypeName, Value = c.CulvertTypeId.ToString() }).ToList();

            TempData["CulvertId"] = culvertid;

            //If the culvert does not have a StructuralInfo entry, return a "New" view
            //But if the culvert already has a StructuralInfo entry, return a "Edit" view
            if (culvertStructure == null)
            {
                return PartialView("StructuralInfoNew", culvertStructure);
            }

            return PartialView("StructuralInfo", culvertStructure);
        }

        public CulvertStructure StructuralInfoSave([Bind(Include = "CulvertId,CulvertTypeId,Width,Height,LengthInside,NoOfBarrels,BarrelsDistance,LengthTotal,AbutmentTypeId,AbutmentHeight,EndWallTypeIdOut,EndWallTypeIdIn")] CulvertStructure strInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(strInfo).State = EntityState.Modified;
                db.SaveChanges();
            }

            return strInfo;
        }

        public ActionResult StructuralInfoNew([Bind(Include = "CulvertId,CulvertTypeId,Width,Height,LengthInside,NoOfBarrels,BarrelsDistance,LengthTotal,AbutmentTypeId,AbutmentHeight,EndWallTypeIdOut,EndWallTypeIdIn")] CulvertStructure strInfo)
        {
            if (ModelState.IsValid)
            {
                db.CulvertStructures.Add(strInfo);
                db.SaveChanges();
            }

            TempData["AbutmentTypeId"] = db.AbutmentTypes.Select(c => new SelectListItem
                                                { Text = c.AbutmentTypeName, Value = c.AbutmentTypeId.ToString() }).ToList();
            TempData["EndWallTypeIdIn"] = db.culEndWallTypes.Select(c => new SelectListItem
                                                { Text = c.EndWallTypeName, Value = c.EndWallTypeId.ToString() }).ToList();
            TempData["EndWallTypeIdOut"] = db.culEndWallTypes.Select(c => new SelectListItem
                                                { Text = c.EndWallTypeName, Value = c.EndWallTypeId.ToString() }).ToList();
            TempData["CulvertTypeId"] = db.CulvertTypes.Select(c => new SelectListItem
                                                { Text = c.CulvertTypeName, Value = c.CulvertTypeId.ToString() }).ToList();

            return View(strInfo);
        }

        ////GET
        //public ActionResult CulvertImage([Optional] string culvertid)
        //{
        //    int mediatypeid = 1; //MediaTypeId is 1 for image

        //    List<CulvertMedia> culvertImageList = (from s in db.CulvertMedias
        //                                           where s.CulvertId == culvertid && s.MediaTypeId == mediatypeid
        //                                           select s).ToList();

        //    return PartialView(culvertImageList);
        //}

        //GET
        public ActionResult CulvertMedia([Optional] string culvertid)
        {
            //MediaTypeId 1 for image, 2 for video, 3 for drawing
            List<CulvertMedia> culvertMediaList = (from cul in db.CulvertMedias
                                                   where cul.CulvertId == culvertid
                                                   select cul).OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList();
                                                    // Sort by media type and then by description

            TempData["MediaTypes"] = db.MediaTypes.Select(c => new SelectListItem
                                        { Text = c.MediaTypeName, Value = c.MediaTypeId.ToString() }).ToList();
            TempData["CulvertId"] = culvertid;

            /**** 
                e.g. MediaPath =  "/media/culvert/Alemgena/Modjo/Addis-Modjo/A1-1-001/down stream.jpg" 
            ****/

            string culvertno = db.Culverts.Find(culvertid).CulvertNo.ToString();
            string segmentName = db.Culverts.Find(culvertid).Segment.SegmentName.ToString();
            string sectionName = db.Culverts.Find(culvertid).Segment.Section.SectionName.ToString();
            string districtName = db.Culverts.Find(culvertid).Segment.Section.District.DistrictName.ToString();
            TempData["MediaPath"] = "/media/culvert/" + districtName.Trim() + "/" + sectionName.Trim() + "/" + segmentName.Trim() + "/" + culvertno.Trim() + "/";

            return PartialView(culvertMediaList);
        }

        [HttpPost]
        public JsonResult CulvertMediaInsert(CulvertMedia culvertMedia)
        {
            // Save media path and description to database
            culvertMedia.Id = Guid.NewGuid().ToString();
            db.CulvertMedias.Add(culvertMedia);
            db.SaveChanges();

            //Upload file to server
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(culvertMedia.MediaFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(culvertMedia.MediaFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);
                ViewBag.Success = true;
            }

            return Json(culvertMedia);
        }

        [HttpPost]
        public ActionResult CulvertMediaUpdate(CulvertMedia culvertMedia)
        {
            
            CulvertMedia updatedCulvertMedia = db.CulvertMedias.Find(culvertMedia.Id);

            updatedCulvertMedia.Description = culvertMedia.Description;
            updatedCulvertMedia.MediaFilePath = culvertMedia.MediaFilePath;
            
            db.SaveChanges();

            //Upload file to server
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(culvertMedia.MediaFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(culvertMedia.MediaFilePath)); // Path excluding the file name
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
        public ActionResult GetLastCulvertMedia(string culvertid)
        {
            var lastCulvertMedia = (from c in db.CulvertMedias
                                   where c.CulvertId == culvertid
                                   select new
                                   {
                                       Id = c.Id,
                                       CulvertId = c.CulvertId,
                                       MediaTypeId = c.MediaTypeId,
                                       Description = c.Description,
                                       MediaFilePath = c.MediaFilePath,
                                       MediaDate = c.MediaDate
                                   }).ToList().LastOrDefault();

            return Json(lastCulvertMedia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CulvertMediaDelete(string id)
        {
            CulvertMedia culvertMedia = db.CulvertMedias.Find(id);
            if (culvertMedia != null)
            {
                db.CulvertMedias.Remove(culvertMedia);
                db.SaveChanges();
            }

            return new EmptyResult();
        }

    }
}