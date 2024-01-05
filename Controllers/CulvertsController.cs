using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.ViewModels;
using ERA_BMS.Models;
using System.Data.SqlClient;

namespace ERA_BMS.Controllers
{
    public class CulvertsController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Culverts
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(string id, StatusMessageId? messageId) // id is Segement Id
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            var CulvertList = (from cul in db.Culverts
                               where cul.SegmentId == id
                               select cul).Include(c => c.Segment).OrderBy(s => s.CulvertNo).ToList();

            ViewBag.Districts = LocationModel.GetDistrictList(); 
            
            if (id == null || CulvertList.Count == 0)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Culverts = new List<SelectListItem>(); // For an empty culverts dropdownlist
            }
            else
            {
                Segment segment = db.Segments.Find(id);

                ViewBag.Segments = LocationModel.GetSegmentList(segment.SectionId);
                ViewBag.SegmentId = id;

                //For a dropdownlist that has a list of sections in that district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(segment.Section.DistrictId);
                ViewBag.SectionId = segment.SectionId;

                //For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = segment.Section.DistrictId;

                ViewBag.DistrictName = segment.Section.District.DistrictName;
                ViewBag.SectionName = segment.Section.SectionName;
                ViewBag.SegmentName = segment.SegmentName;
            }

            return View(CulvertList);
        }

        public ActionResult _GetCulvertIndex(string segmentid, StatusMessageId? messageId /* drop down value */)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<Culvert> CulvertList = (from s in db.Culverts
                                         where s.SegmentId == segmentid
                                         select s).OrderBy(s => s.CulvertNo).ToList();

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(CulvertList);
        }

        public ActionResult CulvertList()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public ActionResult _GetCulvertListAll()
        {
            List<Culvert> CulvertList = (from cul in db.Culverts
                                         select cul).ToList();

            return PartialView(CulvertList);
        }

        public ActionResult _GetCulvertListByDistrict(string districtid /* drop down value */)
        {
            List<Culvert> CulvertList = (from cul in db.Culverts
                                         join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                                         from seg in table1.ToList()
                                         join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                         from sec in table2.ToList()
                                         where sec.DistrictId == districtid
                                         select cul).ToList();

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(CulvertList);
        }

        public ActionResult _GetCulvertListBySection(string sectionid /* drop down value */)
        {
            List<Culvert> CulvertList = (from cul in db.Culverts
                                         join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                                         from seg in table1.ToList()
                                         where seg.SectionId == sectionid
                                         select cul).ToList();

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(CulvertList);
        }

        public ActionResult _GetCulvertListBySegment(string segmentid /* drop down value */)
        {
            List<Culvert> CulvertList = (from s in db.Culverts
                                         where s.SegmentId == segmentid
                                         select s).ToList();

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(CulvertList);
        }

        public ActionResult CulvertListByRoute()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();

            return View();
        }

        public ActionResult _GetCulvertListByMainRoute(string mainrouteid /* drop down value */)
        {
            List<CulvertSubRouteViewModel> CulvertList = (from br in db.Culverts
                                                        join sub in db.SubRoutes on br.SubRouteId equals sub.SubRouteId into table1
                                                        from sub in table1.ToList()
                                                        join main in db.MainRoutes on sub.MainRouteId equals main.MainRouteId into table2
                                                        from main in table2.ToList()
                                                        where main.MainRouteId == mainrouteid
                                                        select new CulvertSubRouteViewModel
                                                        {
                                                            SubRoutes = sub,
                                                            Culverts = br
                                                        }).ToList();

            ViewBag.MainRouteNo = db.MainRoutes.Find(mainrouteid).MainRouteNo.ToString();
            ViewBag.MainRouteName = db.MainRoutes.Find(mainrouteid).MainRouteName.ToString();

            return PartialView(CulvertList);
        }

        public ActionResult _GetCulvertListBySubRoute(string subrouteid /* drop down value */)
        {
            List<CulvertSubRouteViewModel> CulvertList = (from cul in db.Culverts
                                                         join sub in db.SubRoutes on cul.SubRouteId equals sub.SubRouteId into table1
                                                         from sub in table1.ToList()
                                                         where sub.SubRouteId == subrouteid
                                                         select new CulvertSubRouteViewModel
                                                         {
                                                             SubRoutes = sub,
                                                             Culverts = cul
                                                         }).ToList();

            ViewBag.SubRouteNo = db.SubRoutes.Find(subrouteid).SubRouteNo.ToString();
            ViewBag.SubRouteName = db.SubRoutes.Find(subrouteid).SubRouteName.ToString();

            return PartialView(CulvertList);
        }

        // Get a list of culvert numbers starting with term (for AutoComplete)
        public ActionResult GetCulvertNoListContaining(string term)
        {
            var result = (from cul in db.Culverts
                          where cul.CulvertNo.ToLower().StartsWith(term)
                          select cul.CulvertNo).Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // GET: Culverts/Details/5
        //Only logged in user should have access
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culvert culvert = db.Culverts.Find(id);
            if (culvert == null)
            {
                return HttpNotFound();
            }
            
            Segment segment = db.Segments.Find(culvert.SegmentId);

            //Get district, section and segment that encompasses the current culvert
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;
            ViewBag.SectionName = segment.Section.SectionName;
            ViewBag.SegmentId = culvert.SegmentId;

            return View(culvert);
        }

        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public string LastCulvertNo(string subrouteid)
        {
            // CulvertId is in the format "A1-1-C-013" or "C10-7-C-023" where "A1-1" and "C10-7" are sub route no.
            // If there is no culvert in the subroute, suggest 001 (e.g. A1-1-C-001 or C10-7-C-001"
            // Otherwise, find the culvert no. of the lastly added culvert in the subroute, extract the last three digits and increment it by 1
            // But some CulvertId end in "N" and are of the form "A10-6-C-003N" or "B71-1-C-002N"
            // In that case truncate "N"

            // Get the lastly added culvert no. in the given sub route
            var culvertList = db.Culverts.Where(s => s.SubRouteId == subrouteid).Select(s => s.CulvertNo).ToList();

            // If no culvert so far registered on the given sub route 
            if (culvertList.Count == 0)
            {
                // suggest -001 as culvert no (because it is the first culvert for the subroute)
                string firstCulvertNo = db.SubRoutes.Find(subrouteid).SubRouteNo.ToString() + "-C-001";
                return "[{\"CulvertNo\": " + "\"" + firstCulvertNo + "\"}]";
            }

            // The lastly added culvert
            string lastCulvertId = culvertList.Max().ToString();

            // Truncate the last N
            if (lastCulvertId.Last() == 'N')
                lastCulvertId = lastCulvertId.Remove(lastCulvertId.Length - 1, 1);

            //string lastDigits = lastCulvertId.Substring(lastCulvertId.LastIndexOf('-') + 1);
            string lastDigits = lastCulvertId.Split('-').Last();

            int indexOfLastDigit = lastCulvertId.IndexOf(lastDigits);
            string withoutlastDigits = lastCulvertId.Remove(indexOfLastDigit, lastDigits.Length);

            string suggestedCulvertNo;
            int i = 1;

            // If the suggested culvert no. already exists, keep on incrementing it 
            do
            {
                suggestedCulvertNo = withoutlastDigits + (Int32.Parse(lastDigits) + i).ToString().PadLeft(3, '0');
                i++;
            } while (db.Culverts.Any(cul => cul.CulvertNo == suggestedCulvertNo)); // repeat if culvert no. already exists

            // A json string of the form [{"CulvertId": "A1-1-C-014"}]
            return "[{\"CulvertNo\": " + "\"" + suggestedCulvertNo + "\"}]";

        }

        ////Only logged in user should have access
        //[Authorize(Roles = "Admin, User")]
        //public string LastCulvertId()
        //{
        //    string lastCulvertId = db.Culverts.Select(s => s.CulvertId).Max().ToString();

        //    // CulvertId is a five digit unique incrementing string in the format "12345" or "43025"
        //    // When a new culvert is to be registered, it takes the next value to last culvert id from the entire "Culvert" table
        //    // So convert it to int, increment it by 1 and convert it back to string

        //    string assignedCulvertId = (Int32.Parse(lastCulvertId) + 1).ToString();

        //    // The returned result is a json string like [{"CulvertId": "12345"}]
        //    return "[{\"CulvertId\": " + "\"" + assignedCulvertId + "\"}]";
        //}

        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public string LastCulvertId()
        {
            //// CulvertId is a GUID value
            //// When a new culvert is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string

            string assignedCulvertId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"CulvertId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"CulvertId\": " + "\"" + assignedCulvertId + "\"}]";
        }

        /*
        // GET: Culverts/New
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult New()
        {
            ViewBag.Segments = LocationModel.GetSegmentList(); // For an empty segments dropdownlist
            ViewBag.Sections = LocationModel.GetSectionList(); // For an empty sections dropdownlist
            ViewBag.Districts = LocationModel.GetDistrictList(); // For a dropdownlist that has a list of all districts

            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList(); // For a dropdownlist that has a list of all main routes
            ViewBag.SubRoutes = LocationModel.GetSubRouteNameList(); // For an empty sub routes dropdownlist

            return View();
        }
        */

        // GET: Culverts/New
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult New(string id) // id is Road Segment Id
        {
            ViewBag.Districts = LocationModel.GetDistrictList(); // For a dropdownlist that has a list of all districts

            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList(); // For a dropdownlist that has a list of all main routes
            ViewBag.SubRoutes = LocationModel.GetSubRouteNameList(); // For an empty sub routes dropdownlist

            if (id == null)
            {
                ViewBag.Segments = LocationModel.GetSegmentList(); // For an empty segments dropdownlist
                ViewBag.Sections = LocationModel.GetSectionList(); // For an empty sections dropdownlist
            }
            else
            {
                Segment segment = db.Segments.Find(id);

                ViewBag.Segments = LocationModel.GetSegmentList(segment.SectionId);
                ViewBag.SegmentId = id;

                //For a dropdownlist that has a list of sections in that district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(segment.Section.DistrictId);
                ViewBag.SectionId = segment.SectionId;

                //For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.DistrictId = segment.Section.DistrictId;

                ViewBag.DistrictName = segment.Section.District.DistrictName;
                ViewBag.SectionName = segment.Section.SectionName;
                ViewBag.SegmentName = segment.SegmentName;
            }

            return View();
        }

        // POST: Culverts/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult New([Bind(Include = "CulvertId,CulvertNo,RevisedCulvertNo,SegmentId,SubRouteId")] Culvert culvert)
        {
            if (ModelState.IsValid && culvert.SegmentId != "0" && culvert.SubRouteId != "")
            {
                db.Culverts.Add(culvert);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 7 }); // id = 7 for duplicate culvert error
                }

                return RedirectToAction("Details", new { id = culvert.CulvertId });
            }

            ViewBag.SubRouteId = new SelectList(db.SubRoutes, "SubRouteId", "SubRouteName", culvert.SubRouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", culvert.SegmentId);

            return View(culvert);
        }

        // GET: Culverts/Edit/5
        //Only admin user should have access
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culvert culvert = db.Culverts.Find(id);
            if (culvert == null)
            {
                return HttpNotFound();
            }
            Segment segment = db.Segments.Find(culvert.SegmentId);

            // For a dropdownlist that has a list of segments in the current section and selected
            ViewBag.Segments = LocationModel.GetSegmentList(culvert.Segment.SectionId);
            ViewBag.SegmentId = culvert.Segment.SegmentId;

            // For a dropdownlist that has a list of sections in the current section selected
            ViewBag.Sections = LocationModel.GetSectionList(culvert.Segment.Section.DistrictId);
            ViewBag.SectionId = culvert.Segment.SectionId;

            // For a dropdownlist that has a list of all districts and the current district selected
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = culvert.Segment.Section.DistrictId;

            SubRoute subroute = db.SubRoutes.Find(culvert.SubRouteId);

            // For a dropdownlist that has a list of all main routes and the current main route selected
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();
            ViewBag.MainRouteId = culvert.SubRoute.MainRouteId;

            // For a dropdownlist that has a list of sub routes in the current sub route selected
            ViewBag.SubRoutes = LocationModel.GetSubRouteNameList(culvert.SubRoute.MainRoute.MainRouteId);
            ViewBag.SubRouteId = culvert.SubRouteId;

            return View(culvert);
        }

        // POST: Culverts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Only logged in user should have access
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "CulvertId,CulvertNo,RevisedCulvertNo,SegmentId,SubRouteId")] Culvert culvert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(culvert).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 7 }); // id = 7 for duplicate culvert error
                }

                return RedirectToAction("Details", new { id = culvert.CulvertId });
            }
            ViewBag.SubRouteId = new SelectList(db.SubRoutes, "SubRouteId", "SubRouteName", culvert.SubRouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", culvert.SegmentId);
            return View(culvert);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCulvert()
        {
            return View();
        }

        // GET: Culverts/Delete/5
        //Only admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string revisedculvertno)
        {
            if (revisedculvertno == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Culvert culvert = db.Culverts.Where(c => c.RevisedCulvertNo == revisedculvertno).FirstOrDefault();
            if (culvert == null)
            {
                ViewBag.Success = false;
                ViewBag.NoInventory = true;
            }
            else
            {
                var culvertid = culvert.CulvertId;
                CulvertGeneralInfo genInfo = db.CulvertGeneralInfoes.Find(culvertid);
                CulvertStructure culStr = db.CulvertStructures.Find(culvertid);
                if (genInfo == null || culStr == null) // culvert exists but no or incomplete inventory data
                    ViewBag.NoInventory = true;
                else // culvert exists and has complete inventory data
                    ViewBag.NoInventory = false;

                ViewBag.Success = true;
            }

            return PartialView(culvert);
        }

        //Only admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string culvertid)
        {
            Culvert culvert = db.Culverts.Find(culvertid);
            db.Culverts.Remove(culvert);
            db.SaveChanges();
            return RedirectToAction("DeleteCulvert");
        }

        public ActionResult UpdateCulvertNo(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid culvert no
            if (value != "")
            {
                Culvert cul = db.Culverts.Find(id);

                cul.CulvertNo = value.ToString().Trim();
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

        public ActionResult UpdateNewCulvertId(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid new road id
            if (value != "")
            {
                Culvert cul = db.Culverts.Find(id);

                cul.RevisedCulvertNo = value.ToString().Trim();
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
        }//Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
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
