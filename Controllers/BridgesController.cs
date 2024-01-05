using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class BridgesController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: Bridges
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult Index(string id, StatusMessageId? messageId) // id is Segement Id
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            var BridgeList = (from br in db.Bridges
                              where br.SegmentId == id
                              select br).Include(b => b.SubRoute).Include(b => b.Segment).OrderBy(s => s.BridgeNo).ToList();

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == null || BridgeList.Count == 0)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Bridges = new List<SelectListItem>(); // For an empty bridges dropdownlist
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

            return View(BridgeList);
        }

        public ActionResult _GetBridgeIndex(string segmentid, StatusMessageId? messageId /* drop down value */)
        {
            ViewBag.StatusMessage = StatusMessageViewModel.GetMessage(messageId);

            List<Bridge> BridgeList = (from s in db.Bridges
                                       where s.SegmentId == segmentid
                                       select s).ToList();

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(BridgeList);
        }


        // GET: BridgeList
        public ActionResult BridgeList()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;
            
            return View();
        }

        public ActionResult _GetBridgeListAll()
        {
            List<Bridge> BridgeList = (from br in db.Bridges
                                       select br).ToList();

            return PartialView(BridgeList);
        }

        public ActionResult _GetBridgeListByDistrict(string districtid /* drop down value */)
        {
            List<Bridge> BridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       where sec.DistrictId == districtid
                                       select br).ToList();

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(BridgeList);
        }

        public ActionResult _GetBridgeListBySection(string sectionid /* drop down value */)
        {
            List<Bridge> BridgeList = (from br in db.Bridges
                                       join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       where seg.SectionId == sectionid
                                       select br).ToList();

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(BridgeList);
        }

        public ActionResult _GetBridgeListBySegment(string segmentid /* drop down value */)
        {
            List<Bridge> BridgeList = (from br in db.Bridges
                                       where br.SegmentId == segmentid
                                       select br).ToList();

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(BridgeList);
        }

        public ActionResult BridgeListByRoute()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();

            return View();
        }

        public ActionResult _GetBridgeListByMainRoute(string mainrouteid /* drop down value */)
        {
            List<BridgeSubRouteViewModel> BridgeList = (from br in db.Bridges
                                                        join sub in db.SubRoutes on br.SubRouteId equals sub.SubRouteId into table1
                                                        from sub in table1.ToList()
                                                        join main in db.MainRoutes on sub.MainRouteId equals main.MainRouteId into table2
                                                        from main in table2.ToList()
                                                        where main.MainRouteId == mainrouteid
                                                        select new BridgeSubRouteViewModel
                                                        {
                                                            SubRoutes = sub,
                                                            Bridges = br
                                                        }).ToList();

            ViewBag.MainRouteNo = db.MainRoutes.Find(mainrouteid).MainRouteNo.ToString();
            ViewBag.MainRouteName = db.MainRoutes.Find(mainrouteid).MainRouteName.ToString();

            return PartialView(BridgeList);
        }

        public ActionResult _GetBridgeListBySubRoute(string subrouteid /* drop down value */)
        {
            List<BridgeSubRouteViewModel> BridgeList = (from br in db.Bridges
                                                        join sub in db.SubRoutes on br.SubRouteId equals sub.SubRouteId into table1
                                                        from sub in table1.ToList()
                                                        where sub.SubRouteId == subrouteid
                                                        select new BridgeSubRouteViewModel
                                                        {
                                                            SubRoutes = sub,
                                                            Bridges = br
                                                        }).ToList();

            ViewBag.SubRouteNo = db.SubRoutes.Find(subrouteid).SubRouteNo.ToString();
            ViewBag.SubRouteName = db.SubRoutes.Find(subrouteid).SubRouteName.ToString();

            return PartialView(BridgeList);
        }

        // GET: Bridges/Details/5
        //Only logged in user should have access
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Find(id);
            if (bridge == null)
            {
                return HttpNotFound();
            }

            return View(bridge);
        }

        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public string LastBridgeNo(string subrouteid)
        {
            // BridgeNo is in the format "A1-1-013" or "C10-7-023" where "A1-1" and "C10-7" are sub route no.
            // If there is no bridge in the subroute, suggest 001 (e.g. A1-1-001 or C10-7-001"
            // Otherwise, find the bridge no. of the lastly added bridge in the subroute, extract the last three digits and increment it by 1
            // But some BridgeId end in "N" and are of the form "A10-6-003N" or "B71-1-002N"
            // In that case truncate "N"

            // Get the lastly added bridge no. in the given sub route
            var bridgeList = db.Bridges.Where(s => s.SubRouteId == subrouteid).Select(s => s.BridgeNo).ToList();

            // If no bridge so far registered on the given sub route 
            if (bridgeList.Count == 0)
            {
                // suggest -001 as bridge no (because it is the first bridge for the subroute)
                string firstBridgeNo = db.SubRoutes.Find(subrouteid).SubRouteNo.ToString() + "-001";
                return "[{\"BridgeNo\": " + "\"" + firstBridgeNo + "\"}]";
            }

            // The lastly added bridge
            string lastBridgeNo = bridgeList.Max().ToString();

            // Truncate the last N
            if (lastBridgeNo.Last() == 'N')
                lastBridgeNo = lastBridgeNo.Remove(lastBridgeNo.Length - 1, 1);

            //string lastDigits = lastBridgeId.Substring(lastBridgeNo.LastIndexOf('-') + 1);
            string lastDigits = lastBridgeNo.Split('-').Last();

            int indexOfLastDigit = lastBridgeNo.IndexOf(lastDigits);
            string withoutlastDigits = lastBridgeNo.Remove(indexOfLastDigit, lastDigits.Length);

            string suggestedBridgeNo;
            int i = 1;

            // If the suggested bridge no. already exists, keep on incrementing it 
            do
            {
                suggestedBridgeNo = withoutlastDigits + (Int32.Parse(lastDigits) + i).ToString().PadLeft(3, '0');
                i++;
            } while (db.Bridges.Any(br => br.BridgeNo == suggestedBridgeNo)); // repeat if bridge no. exists

            // A json string of the form [{"BridgeNo": "A1-1-014"}]
            return "[{\"BridgeNo\": " + "\"" + suggestedBridgeNo + "\"}]";

        }

        ////Only logged in user should have access
        //[Authorize(Roles = "Admin, User")]
        //public string LastBridgeId()
        //{
        //    string lastBridgeId = db.Bridges.Select(s => s.BridgeId).Max().ToString();

        //    // BridgeId is a five digit unique incrementing string in the format "12345" or "43025"
        //    // When a new bridge is to be registered, it takes the next value to last bridge id from the entire "Bridge" table
        //    // So convert it to int, increment it by 1 and convert it back to string

        //    string assignedBridgeId = (Int32.Parse(lastBridgeId) + 1).ToString();

        //    // The returned result is a json string like [{"BridgeId": "12345"}]
        //    return "[{\"BridgeId\": " + "\"" + assignedBridgeId + "\"}]";
        //}

        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public string LastBridgeId()
        {
            //// BridgeId is a GUID value
            //// When a new bridge is to be registered, it takes a newly generated GUID value
            //// That value has to be converted to string

            string assignedBridgeId = Guid.NewGuid().ToString();

            // The returned result is a json string like [{"BridgeId": "67b4629b-f95a-4ba4-b096-e0d2ae76e2fe"}]
            return "[{\"BridgeId\": " + "\"" + assignedBridgeId + "\"}]";
        }

        /*
        // GET: Bridges/New
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

        // GET: Bridges/New
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
        
        // POST: Bridges/New
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult New([Bind(Include = "BridgeId,BridgeNo,RevisedBridgeNo,BridgeName,SegmentId,SubRouteId")] Bridge bridge)
        {
            if (ModelState.IsValid && bridge.SegmentId != "0" && bridge.SubRouteId != "")
            {
                db.Bridges.Add(bridge);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 6 }); // id = 6 for duplicate bridge error
                }

                return RedirectToAction("Details", new { id = bridge.BridgeId });
            }

            ViewBag.SubRouteId = new SelectList(db.SubRoutes, "SubRouteId", "SubRouteName", bridge.SubRouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", bridge.SegmentId);

            return View(bridge);
        }

        // GET: Bridges/Edit/5
        //Only admin user should have access
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Find(id);
            if (bridge == null)
            {
                return HttpNotFound();
            }
            Segment segment = db.Segments.Find(bridge.SegmentId);

            // For a dropdownlist that has a list of segments in the current section and selected
            ViewBag.Segments = LocationModel.GetSegmentList(bridge.Segment.SectionId);
            ViewBag.SegmentId = bridge.Segment.SegmentId;

            // For a dropdownlist that has a list of sections in the current section selected
            ViewBag.Sections = LocationModel.GetSectionList(bridge.Segment.Section.DistrictId);
            ViewBag.SectionId = bridge.Segment.SectionId;

            // For a dropdownlist that has a list of all districts and the current district selected
            ViewBag.Districts = LocationModel.GetDistrictList();
            ViewBag.DistrictId = bridge.Segment.Section.DistrictId;

            SubRoute subroute = db.SubRoutes.Find(bridge.SubRouteId);
            
            // For a dropdownlist that has a list of all main routes and the current main route selected
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();
            ViewBag.MainRouteId = bridge.SubRoute.MainRouteId;

            // For a dropdownlist that has a list of sub routes in the current sub route selected
            ViewBag.SubRoutes = LocationModel.GetSubRouteNameList(bridge.SubRoute.MainRoute.MainRouteId);
            ViewBag.SubRouteId = bridge.SubRouteId;
            
            return View(bridge);
        }

        // POST: Bridges/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Only logged in user should have access
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit([Bind(Include = "BridgeId,BridgeNo,RevisedBridgeNo,BridgeName,SegmentId,SubRouteId")] Bridge bridge)
        {
            //string bridgeid = db.Bridges.Find(bridge.BridgeId).BridgeId.ToString();
            if (ModelState.IsValid)
            {
                //if (bridgeid != bridge.BridgeId)
                //    db.Entry(bridge).State = EntityState.Added;
                //else
                //    db.Entry(bridge).State = EntityState.Modified;
                db.Entry(bridge).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e) when (e.InnerException?.InnerException is SqlException sqlEx
                                            && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                {
                    return RedirectToAction("DuplicateError", "ErrorHandler", new { id = 6 }); // id = 6 for duplicate bridge error
                }


                return RedirectToAction("Details", new { id = bridge.BridgeId });
            }
            ViewBag.SubRouteId = new SelectList(db.SubRoutes, "SubRouteId", "SubRouteName", bridge.SubRouteId);
            ViewBag.SegmentId = new SelectList(db.Segments, "SegmentId", "SegmentName", bridge.SegmentId);
            return View(bridge);
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBridge()
        {
            return View();
        }

        //Only Admin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string revisedbridgeno)
        {
            if (revisedbridgeno == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bridge bridge = db.Bridges.Where(br => br.RevisedBridgeNo == revisedbridgeno).FirstOrDefault();
            if (bridge == null)
            {
                ViewBag.Success = false;
                ViewBag.NoInventory = true;
            }
            else
            {
                var bridgeid = bridge.BridgeId;

                BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeid);
                SuperStructure supertStr = db.SuperStructures.Find(bridgeid);
                if (genInfo == null || supertStr == null) // bridge exists but no or incomplete inventory data
                    ViewBag.NoInventory = true;
                else // bridge exists and has complete inventory data
                    ViewBag.NoInventory = false;

                ViewBag.Success = true;
            }

            return PartialView(bridge);
        }

        //Only Ammin should have access
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string bridgeid)
        {
            Bridge bridge = db.Bridges.Find(bridgeid);
            
            db.Bridges.Remove(bridge);
            db.SaveChanges();

            return RedirectToAction("DeleteBridge");
        }

        public ActionResult UpdateBridgeNo(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid bridge no
            if (value != "")
            {
                Bridge brg = db.Bridges.Find(id);

                brg.BridgeNo = value.ToString().Trim();
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

        public ActionResult UpdateNewBridgeId(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid new road id
            if (value != "")
            {
                Bridge brg = db.Bridges.Find(id);

                brg.RevisedBridgeNo = value.ToString().Trim();
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

        public ActionResult UpdateBridgeName(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return Json(new { Message = "", value = value }, JsonRequestBehavior.AllowGet);

            // Check if value is a valid bridge no
            if (value != "")
            {
                Bridge brg = db.Bridges.Find(id);

                brg.BridgeName = value.ToString().Trim();
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

        //Only logged in user should have access
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
