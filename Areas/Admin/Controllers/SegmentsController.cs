using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;
 
namespace ERA_BMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SegmentsController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Admin/Segments
        public ActionResult Index(string id, StatusMessageId? messageId) // id is Section Id
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            var SegmentList = (from seg in db.Segments
                              where seg.SectionId == id
                              select seg).ToList();

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == null || SegmentList.Count == 0)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
            }
            else
            {
                Section section = db.Sections.Find(id);

                //For a dropdownlist that has a list of sections in that district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(section.DistrictId);
                ViewBag.SectionId = id;

                //For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = section.DistrictId;

                ViewBag.DistrictName = section.District.DistrictName;
                ViewBag.SectionName = section.SectionName;
            }

            return View(SegmentList);
        }

        public ActionResult _GetSegmentsList(string sectionid, StatusMessageId? messageId)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<Segment> SegmentList = (from s in db.Segments
                                         where s.SectionId == sectionid
                                         select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(SegmentList);
        }

        // GET: Admin/Segments/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Segment segment = db.Segments.Find(id);
            if (segment == null)
            {
                return HttpNotFound();
            }

            //Get district name that encompasses the current segment
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;
            ViewBag.SectionId = segment.SectionId;

            return View(segment);
        }

        //public string LastSegmentId()
        //{
        //    string lastSegmentId = db.Segments.Select(s => s.SegmentId).Max().ToString();

        //    // SegmentId is a four digit unique incrementing string in the format "1001" or "1151"
        //    // When a new Segment is to be registered, it takes the next value to last Segment id from the "Segment" table
        //    // So convert it to int, increment it by 1 and convert it back to string

        //    string assignedSegmentId = (Int32.Parse(lastSegmentId) + 1).ToString();

        //    // The returned result is a json string like [{"SegmentId": "12345"}]
        //    return "[{\"SegmentId\": " + "\"" + assignedSegmentId + "\"}]";
        //}

        public string LastSegmentId()
        {
            //// SegmentId is a GUID value
            //// When a new segment is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string
            ///
            string assignedSegmentId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"SegmentId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"SegmentId\": " + "\"" + assignedSegmentId + "\"}]";
        }

        // GET: Admin/Segments/New
        [Authorize(Roles = "Admin")]
        public ActionResult New(string id) // id is Segment Id
        {
            ViewBag.Districts = LocationModel.GetDistrictList(); // For a dropdownlist that has a list of all districts

            if (id == null)
            {
                ViewBag.Sections = LocationModel.GetSectionList(); // For an empty sections dropdownlist
            }
            else
            {
                Section section = db.Sections.Find(id);

                //For a dropdownlist that has a list of sections in that district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(section.DistrictId);
                ViewBag.SectionId = section.SectionId;

                //For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = section.DistrictId;

                ViewBag.DistrictName = section.District.DistrictName;
                ViewBag.SectionName = section.SectionName;
            }

            
            // For a dropdownlist that has a list of road class
            var roadClasses = (from rd in db.RoadClasses
                                    select new DropDownViewModel { Value = rd.RoadClassId.ToString(), Text = rd.RoadClassName }).ToList();
            roadClasses.Insert(0, new DropDownViewModel { Text = "", Value = "" }); 

            ViewBag.RoadClassId = roadClasses;
            
            // For a dropdownlist that has a list of surface types
            var surfaceTypes = (from sur in db.RoadSurfaceTypes
                                    select new DropDownViewModel { Value = sur.RoadSurfaceTypeId.ToString(), Text = sur.RoadSurfaceTypeName }).ToList();
            surfaceTypes.Insert(0, new DropDownViewModel { Text = "", Value = "" }); 

            ViewBag.RoadSurfaceTypeId = surfaceTypes;
            
            // For a dropdownlist that has a list of design standards
            var designstd = (from des in db.DesignStandards
                                       select new DropDownViewModel { Value = des.DesignStandardId.ToString(), Text = des.DesignStandardName }).ToList();
            designstd.Insert(0, new DropDownViewModel { Text = "", Value = "" });
            ViewBag.DesignStandardId = designstd;
            
            // For a dropdownlist that has a list of regional governments
            var regionalgov = (from reg in db.RegionalGovernments
                                           select new DropDownViewModel { Value = reg.RegionalGovernmentId.ToString(), Text = reg.RegionalGovernmentName }).ToList();
            regionalgov.Insert(0, new DropDownViewModel { Text = "", Value = "" }); 
            ViewBag.RegionalGovernmentId = regionalgov;

            //ViewBag.RoadSurfaceTypeId = new SelectList(db.RoadSurfaceTypes, "RoadSurfaceTypeId", "RoadSurfaceTypeName");
            //ViewBag.DesignStandardId = new SelectList(db.DesignStandards, "DesignStandardId", "DesignStandardName");
            //ViewBag.RegionalGovernmentId = new SelectList(db.RegionalGovernments, "RegionalGovernmentId", "RegionalGovernmentName");

            return View();
        }

        // POST: Admin/Segments/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult New([Bind(Include = "SegmentId,SegmentNo,SegmentName,RoadId,RevisedRoadId,AsphaltLength,GravelLength,Length,Width,ConstructionYear,RoadClassId,RoadSurfaceTypeId,AverageDailyTraffic,RegionalGovernmentId,DesignStandardId,SectionId,Remark")] Segment segment)
        {
            if (ModelState.IsValid)
            {
                db.Segments.Add(segment);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 3 }); // id = 3 for duplicate segment error
                }

                return RedirectToAction("Details", new { id = segment.SegmentId });
            }

            ViewBag.SectionId = new SelectList(db.Sections, "SectionId", "SectionName", segment.SectionId);
            ViewBag.Sections = LocationModel.GetSectionList(); // For an empty sections dropdownlist
            ViewBag.Districts = LocationModel.GetDistrictList(); // For a dropdownlist that has a list of all districts
            
            return View(segment);
        }

        // GET: Admin/Segments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //Get the current segement with the given id
            Segment segment = db.Segments.Find(id);

            if (segment == null)
            {
                return HttpNotFound();
            }

            // For a dropdownlist that has a list of sections in the current section selected
            ViewBag.Sections = LocationModel.GetSectionList(segment.Section.DistrictId);
            ViewBag.SectionId = segment.SectionId;

            // For a dropdownlist that has a list of all districts and the current district selected
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = segment.Section.DistrictId;

            // For a dropdownlist that has a list of road class
            var roadClasses = (from rd in db.RoadClasses
                               select new DropDownViewModel { Value = rd.RoadClassId.ToString(), Text = rd.RoadClassName }).ToList();
            roadClasses.Insert(0, new DropDownViewModel { Text = "", Value = "" });

            ViewBag.RoadClasses = roadClasses;
            ViewBag.RoadClassId = segment.RoadClassId;

            // For a dropdownlist that has a list of surface types
            var surfaceTypes = (from sur in db.RoadSurfaceTypes
                                select new DropDownViewModel { Value = sur.RoadSurfaceTypeId.ToString(), Text = sur.RoadSurfaceTypeName }).ToList();
            surfaceTypes.Insert(0, new DropDownViewModel { Text = "", Value = "" });

            ViewBag.RoadSurfaceTypes = surfaceTypes;
            ViewBag.RoadSurfaceTypeId = segment.RoadSurfaceTypeId;

            // For a dropdownlist that has a list of design standards
            var designstd = (from des in db.DesignStandards
                             select new DropDownViewModel { Value = des.DesignStandardId.ToString(), Text = des.DesignStandardName }).ToList();
            designstd.Insert(0, new DropDownViewModel { Text = "", Value = "" });
            ViewBag.DesignStandards = designstd;
            ViewBag.DesignStandardId = segment.DesignStandardId;

            // For a dropdownlist that has a list of regional governments
            var regionalgov = (from reg in db.RegionalGovernments
                               select new DropDownViewModel { Value = reg.RegionalGovernmentId.ToString(), Text = reg.RegionalGovernmentName }).ToList();
            regionalgov.Insert(0, new DropDownViewModel { Text = "", Value = "" });
            ViewBag.RegionalGovernments = regionalgov;
            ViewBag.RegionalGovernmentId = segment.RegionalGovernmentId;

            return View(segment);
        }

        // POST: Admin/Segments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SegmentId,SegmentNo,SegmentName,RoadId,RevisedRoadId,AsphaltLength,GravelLength,Length,Width,ConstructionYear,RoadClassId,RoadSurfaceTypeId,AverageDailyTraffic,RegionalGovernmentId,DesignStandardId,SectionId,Remark")] Segment segment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(segment).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 3 }); // id = 3 for duplicate segment error
                }

                return RedirectToAction("Details", new { id = segment.SegmentId });
            }

            // For a dropdownlist that has a list of sections in the current section selected
            ViewBag.Sections = LocationModel.GetSectionList(segment.Section.DistrictId);
            ViewBag.SectionId = segment.SectionId;

            // For a dropdownlist that has a list of all districts and the current district selected
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = segment.Section.DistrictId;
            
            return View(segment);
        }

        //// GET: Admin/Segments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Segment segment = db.Segments.Find(id);
            if (segment == null)
            {
                return HttpNotFound();
            }
            
            //Get district name that encompasses the current segment
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;
            ViewBag.SectionId = segment.SectionId;

            return View(segment);
        }

        // POST: Admin/Segments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            Segment segment = db.Segments.Find(id);
            string sectionId = segment.SectionId;

            db.Segments.Remove(segment);
            try
            {
                db.SaveChanges();
            }
            catch (Exception) // catches all exceptions
            {
                return RedirectToAction("DeleteError", "ErrorHandler", new { id = 3 }); // id = 3 for road segment delete error
            }
            return RedirectToAction("Index", "Segments", new { id = sectionId });
        }

        public ActionResult UpdateSegmentNo(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid segment no
            if (value != "")
            {
                Segment seg = db.Segments.Find(id);

                seg.SegmentNo = value.ToString().Trim();
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.DuplicateDataError), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateRoadId(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid road id
            if (value != "")
            {
                Segment seg = db.Segments.Find(id);

                seg.RoadId = value.ToString().Trim();
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.DuplicateDataError), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateNewRoadId(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid new road id
            if (value != "")
            {
                Segment seg = db.Segments.Find(id);

                seg.RevisedRoadId = value.ToString().Trim();
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.DuplicateDataError), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSegmentName(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid segment no
            if (value != "")
            {
                Segment seg = db.Segments.Find(id);

                seg.SegmentName = value.ToString().Trim();
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                    catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                                && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.DuplicateDataError), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateLength(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment seg = db.Segments.Find(id);
            double len;

            // Check if user has entered empty value
            if (value.Trim() == null || value.Trim() == "")
                seg.Length = null;
            else if (double.TryParse(value, out len)) // Check if value is a valid length of type double
                seg.Length = len;
            try
            {
                db.SaveChanges();

                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateWidth(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment seg = db.Segments.Find(id);
            double wid;

            // Check if user has entered empty value
            if (value.Trim() == null || value.Trim() == "")
                seg.Width = null;
            else if (double.TryParse(value, out wid)) // Check if value is a valid width of type double
                seg.Width = wid;
            try
            {
                db.SaveChanges();

                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateRoadClass(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment seg = db.Segments.Find(id);
            RoadClass des = db.RoadClasses.Where(d => d.RoadClassName == value).FirstOrDefault();

            // Check if user has selected empty option from dropdown
            if (des == null)
                seg.RoadClassId = null;
            else
                seg.RoadClassId = des.RoadClassId;

            try
            {
                db.SaveChanges();
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectRoadClass()
        {
            List<RoadClass> des = db.RoadClasses.ToList();
            var list = des.Select(d => new[] { d.RoadClassName, d.RoadClassName }).ToList();
            list.Insert(0, new[] { "", "" });
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult UpdateDesignStandard(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment seg = db.Segments.Find(id);
            DesignStandard des = db.DesignStandards.Where(d => d.DesignStandardName == value).FirstOrDefault();

            // Check if user has selected empty option from dropdown
            if (des == null)
                seg.DesignStandardId = null;
            else
                seg.DesignStandardId = des.DesignStandardId;

            try
            {
                db.SaveChanges();
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectDesignStandard()
        {
            List<DesignStandard> des = db.DesignStandards.ToList();
            var list = des.Select(d => new[] { d.DesignStandardName, d.DesignStandardName }).ToList();
            list.Insert(0, new [] { "", "" });
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAverageDailyTraffic(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid length
            if (value.All(Char.IsDigit))
            {
                Segment seg = db.Segments.Find(id);

                seg.AverageDailyTraffic = int.Parse(value);
                try
                {
                    db.SaveChanges();

                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSurfaceType(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment seg = db.Segments.Find(id);
            RoadSurfaceType surf = db.RoadSurfaceTypes.Where(d => d.RoadSurfaceTypeName == value).FirstOrDefault();

            // Check if user has selected empty option from dropdown
            if (surf == null)
                seg.RoadSurfaceTypeId = null;
            else
                seg.RoadSurfaceTypeId = surf.RoadSurfaceTypeId;
            try
            {
                db.SaveChanges();
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectSurfaceType()
        {
            List<RoadSurfaceType> des = db.RoadSurfaceTypes.ToList();
            var list = des.Select(d => new[] { d.RoadSurfaceTypeName, d.RoadSurfaceTypeName }).ToList();
            list.Insert(0, new[] { "", "" });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateRegionalGovernment(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment seg = db.Segments.Find(id);
            RegionalGovernment reg = db.RegionalGovernments.Where(d => d.RegionalGovernmentName == value).FirstOrDefault();

            // Check if user has selected empty option from dropdown
            if (reg == null)
                seg.RegionalGovernmentId = null;
            else
                seg.RegionalGovernmentId = reg.RegionalGovernmentId;
            try
            {
                db.SaveChanges();
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SelectRegionalGovernment()
        {
            List<RegionalGovernment> des = db.RegionalGovernments.ToList();
            var list = des.Select(d => new[] { d.RegionalGovernmentName, d.RegionalGovernmentName }).ToList();
            list.Insert(0, new[] { "", "" });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateRemark(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            Segment segment = db.Segments.Find(id);

            segment.Remark = value;
            try
            {
                db.SaveChanges();

                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.ChangeDataSuccess), value = value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { Message = StatusMessageViewModel.GetMessage(StatusMessageId.Error), value = origvalue }, JsonRequestBehavior.AllowGet);
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
