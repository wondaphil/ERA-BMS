using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class CulvertLocationController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: CulvertLocation
        public ActionResult Index()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;
            ViewBag.TotalRecords = districtList.Count();

            return View();
        }

        public ActionResult _GetCulvertLocationAll()
        {
            List<Culvert> culvertLocation = (from cul in db.Culverts
                                                 select cul).ToList();

            return PartialView(culvertLocation);
        }

        public ActionResult _GetCulvertLocationByDistrict(string districtid /* drop down value */)
        {
            List<Culvert> culvertLocation = (from cul in db.Culverts
                                                 join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                                                 from seg in table1.ToList()
                                                 join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                                 from sec in table2.ToList()
                                                 where sec.DistrictId == districtid
                                                 select cul).ToList();

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(culvertLocation);
        }

        public ActionResult _GetCulvertLocationBySection(string sectionid /* drop down value */)
        {
            List<Culvert> culvertLocation = (from cul in db.Culverts
                                                 join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                                                 from seg in table1.ToList()
                                                 where seg.SectionId == sectionid
                                                 select cul).ToList();

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(culvertLocation);
        }

        public ActionResult _GetCulvertLocationBySegment(string segmentid /* drop down value */)
        {
            List<Culvert> culvertLocation = (from cul in db.Culverts
                                             where cul.SegmentId == segmentid
                                             select cul).ToList();

            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(culvertLocation);
        }

        public ActionResult CulvertLocationByRoute()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();

            return View();
        }

        public ActionResult _GetCulvertLocationByMainRoute(string mainrouteid /* drop down value */)
        {
            List<CulvertSubRouteViewModel> CulvertLocation = (from br in db.Culverts
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

            return PartialView(CulvertLocation);
        }

        public ActionResult _GetCulvertLocationBySubRoute(string subrouteid /* drop down value */)
        {
            List<CulvertSubRouteViewModel> CulvertLocation = (from br in db.Culverts
                                                              join sub in db.SubRoutes on br.SubRouteId equals sub.SubRouteId into table1
                                                              from sub in table1.ToList()
                                                              where sub.SubRouteId == subrouteid
                                                              select new CulvertSubRouteViewModel
                                                              {
                                                                  SubRoutes = sub,
                                                                  Culverts = br
                                                              }).ToList();

            ViewBag.SubRouteNo = db.SubRoutes.Find(subrouteid).SubRouteNo.ToString();
            ViewBag.SubRouteName = db.SubRoutes.Find(subrouteid).SubRouteName.ToString();

            return PartialView(CulvertLocation);
        }

        public ActionResult CulvertMap(string id) // culvert id
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

            // Just get the first image 
            CulvertMedia image = (from s in db.CulvertMedias
                                  where s.CulvertId == culvert.CulvertId && s.MediaTypeId == 1 // MediaTypeId = 1 for image
                                  select s).FirstOrDefault(); 

            ViewBag.ImageFilePath = (image != null) ? image.MediaFilePath : null;

            if (culvert.CulvertGeneralInfo != null)
            {
                if (culvert.CulvertGeneralInfo.XCoord != null && culvert.CulvertGeneralInfo.YCoord != null && culvert.CulvertGeneralInfo.UtmZoneId != null)
                {
                    ViewBag.XCoord = culvert.CulvertGeneralInfo.XCoord;
                    ViewBag.YCoord = culvert.CulvertGeneralInfo.YCoord;
                    ViewBag.UtmZone = culvert.CulvertGeneralInfo.UtmZoneEthiopia.UtmZone;
                    ViewBag.LatitudeDecimal = culvert.CulvertGeneralInfo.LatitudeDecimal.Value.ToString("#,0.000000");
                    ViewBag.LongitudeDecimal = culvert.CulvertGeneralInfo.LongitudeDecimal.Value.ToString("#,0.000000");
                    ViewBag.LatitudeDMS = culvert.CulvertGeneralInfo.LatitudeDMS;
                    ViewBag.LongitudeDMS = culvert.CulvertGeneralInfo.LongitudeDMS;
                }
            }
            else 
            {
                ViewBag.XCoord = ViewBag.YCoord = null;
            }

            if (culvert.CulvertStructure != null)
            {
                ViewBag.CulvertLength = culvert.CulvertStructure.LengthTotal;
                ViewBag.CulvertType = culvert.CulvertStructure.CulvertType.CulvertTypeName;
            }

            return View(culvert);
        }

        public ActionResult CulvertMapBySegment(string id) // road segment id
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Culvert> culverts = db.Culverts.Where(cul => cul.SegmentId == id).ToList();
            
            if (culverts == null)
            {
                return HttpNotFound();
            }

            ViewBag.SegmentNo = db.Segments.Find(id).SegmentNo.ToString();
            ViewBag.SegmentName = db.Segments.Find(id).SegmentName.ToString();

            var culvertLocList = (from cul in culverts
                                 select new CulvertLocationViewModel
                                 {
                                     CulvertId = cul.CulvertId,
                                     OldCulvertId = cul.CulvertNo,
                                     RevisedCulvertId = cul.RevisedCulvertNo,
                                     District = cul.Segment.Section.District.DistrictName,
                                     Segment = "[" + cul.Segment.RevisedRoadId + "] " + cul.Segment.SegmentName,
                                 }).ToList();

            foreach (var culvertLoc in culvertLocList)
            {
                CulvertGeneralInfo genInfo = db.CulvertGeneralInfoes.Find(culvertLoc.CulvertId);
                CulvertStructure culStr = db.CulvertStructures.Find(culvertLoc.CulvertId);
                CulvertMedia culMedia = db.CulvertMedias.Where(s => s.CulvertId == culvertLoc.CulvertId && s.MediaTypeId == 1).FirstOrDefault();
                culvertLoc.ImageFilePath = culMedia != null ? culMedia.MediaFilePath : "";

                culvertLoc.XCoord = genInfo != null ? genInfo.XCoord : null;
                culvertLoc.YCoord = genInfo != null ? genInfo.YCoord : null;
                culvertLoc.UtmZone = genInfo != null ? (int?)genInfo.UtmZoneEthiopia.UtmZone : null;

                if (genInfo != null)
                {
                    if (genInfo.XCoord != null && genInfo.YCoord != null && genInfo.UtmZoneId != null)
                    {
                        culvertLoc.LatitudeDecimal = genInfo.LatitudeDecimal;
                        culvertLoc.LongitudeDecimal = genInfo.LongitudeDecimal;
                        culvertLoc.LatitudeDMS = genInfo.LatitudeDMS;
                        culvertLoc.LongitudeDMS = genInfo.LongitudeDMS;
                    }

                    if (culStr != null)
                    {
                        culvertLoc.CulvertLength = culStr.LengthTotal;
                        culvertLoc.CulvertType = culStr.CulvertType.CulvertTypeName;
                    }
                }
            }

            ViewBag.CulvertLocationList = culvertLocList;

            List<Bridge> bridges = db.Bridges.Where(brg => brg.SegmentId == id).ToList();

            if (bridges == null)
            {
                // if there are no bridges, simply pass empty list
                ViewBag.BridgeLocationList = new List<BridgeLocationViewModel>();
            }
            else
            { 
                var bridgeLocList = (from brg in bridges
                                     select new BridgeLocationViewModel
                                     {
                                         BridgeId = brg.BridgeId,
                                         OldBridgeId = brg.BridgeNo,
                                         RevisedBridgeId = brg.RevisedBridgeNo,
                                         BridgeName = brg.BridgeName,
                                     }).ToList();

                foreach (var bridgeLoc in bridgeLocList)
                {
                    BridgeGeneralInfo brgGenInfo = db.BridgeGeneralInfoes.Find(bridgeLoc.BridgeId);
                    
                    bridgeLoc.XCoord = brgGenInfo != null ? brgGenInfo.XCoord : null;
                    bridgeLoc.YCoord = brgGenInfo != null ? brgGenInfo.YCoord : null;
                    bridgeLoc.UtmZone = brgGenInfo != null ? (int?)brgGenInfo.UtmZoneEthiopia.UtmZone : null;

                    if (brgGenInfo != null)
                    {
                        if (brgGenInfo.XCoord != null && brgGenInfo.YCoord != null && brgGenInfo.UtmZoneId != null)
                        {
                            bridgeLoc.LatitudeDecimal = brgGenInfo.LatitudeDecimal;
                            bridgeLoc.LongitudeDecimal = brgGenInfo.LongitudeDecimal;
                            bridgeLoc.BridgeLength = brgGenInfo.BridgeLength;
                        }
                    }
                }

                ViewBag.BridgeLocationList = bridgeLocList;
            }

            return View();
        }

        public ActionResult CulvertMapBySubRoute(string id) // sub route id
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Culvert> culverts = db.Culverts.Where(cul => cul.SubRouteId == id).ToList();
            
            if (culverts == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubRouteNo = db.SubRoutes.Find(id).SubRouteNo.ToString();
            ViewBag.SubRouteName = db.SubRoutes.Find(id).SubRouteName.ToString();

            var culvertLocList = (from cul in culverts
                                 select new CulvertLocationViewModel
                                 {
                                     CulvertId = cul.CulvertId,
                                     OldCulvertId = cul.CulvertNo,
                                     RevisedCulvertId = cul.RevisedCulvertNo,
                                     District = cul.Segment.Section.District.DistrictName,
                                     Segment = "[" + cul.Segment.RevisedRoadId + "] " + cul.Segment.SegmentName,
                                 }).ToList();

            foreach (var culvertLoc in culvertLocList)
            {
                CulvertGeneralInfo genInfo = db.CulvertGeneralInfoes.Find(culvertLoc.CulvertId);
                CulvertStructure culStr = db.CulvertStructures.Find(culvertLoc.CulvertId);
                CulvertMedia culMedia = db.CulvertMedias.Where(s => s.CulvertId == culvertLoc.CulvertId && s.MediaTypeId == 1).FirstOrDefault();
                culvertLoc.ImageFilePath = culMedia != null ? culMedia.MediaFilePath : "";

                culvertLoc.XCoord = genInfo != null ? genInfo.XCoord : null;
                culvertLoc.YCoord = genInfo != null ? genInfo.YCoord : null;
                culvertLoc.UtmZone = genInfo != null ? (int?)genInfo.UtmZoneEthiopia.UtmZone : null;

                if (genInfo != null)
                {
                    if (genInfo.XCoord != null && genInfo.YCoord != null && genInfo.UtmZoneId != null)
                    {
                        culvertLoc.LatitudeDecimal = genInfo.LatitudeDecimal;
                        culvertLoc.LongitudeDecimal = genInfo.LongitudeDecimal;
                        culvertLoc.LatitudeDMS = genInfo.LatitudeDMS;
                        culvertLoc.LongitudeDMS = genInfo.LongitudeDMS;
                    }

                    if (culStr != null)
                    {
                        culvertLoc.CulvertLength = culStr.LengthTotal;
                        culvertLoc.CulvertType = culStr.CulvertType.CulvertTypeName;
                    }
                }
            }

            ViewBag.CulvertLocationList = culvertLocList;

            List<Bridge> bridges = db.Bridges.Where(brg => brg.SubRouteId == id).ToList();

            if (bridges == null)
            {
                // if there are no bridges, simply pass empty list
                ViewBag.BridgeLocationList = new List<BridgeLocationViewModel>();
            }
            else
            { 
                var bridgeLocList = (from brg in bridges
                                     select new BridgeLocationViewModel
                                     {
                                         BridgeId = brg.BridgeId,
                                         OldBridgeId = brg.BridgeNo,
                                         RevisedBridgeId = brg.RevisedBridgeNo,
                                         BridgeName = brg.BridgeName,
                                     }).ToList();

                foreach (var bridgeLoc in bridgeLocList)
                {
                    BridgeGeneralInfo brgGenInfo = db.BridgeGeneralInfoes.Find(bridgeLoc.BridgeId);
                    
                    bridgeLoc.XCoord = brgGenInfo != null ? brgGenInfo.XCoord : null;
                    bridgeLoc.YCoord = brgGenInfo != null ? brgGenInfo.YCoord : null;
                    bridgeLoc.UtmZone = brgGenInfo != null ? (int?)brgGenInfo.UtmZoneEthiopia.UtmZone : null;

                    if (brgGenInfo != null)
                    {
                        if (brgGenInfo.XCoord != null && brgGenInfo.YCoord != null && brgGenInfo.UtmZoneId != null)
                        {
                            bridgeLoc.LatitudeDecimal = brgGenInfo.LatitudeDecimal;
                            bridgeLoc.LongitudeDecimal = brgGenInfo.LongitudeDecimal;
                            bridgeLoc.BridgeLength = brgGenInfo.BridgeLength;
                        }
                    }
                }

                ViewBag.BridgeLocationList = bridgeLocList;
            }

            return View();
        }

        public ActionResult UpdateCoordinate(string CulvertId, double XCoordinate, double YCoordinate, int UtmZone)
        {
            CulvertGeneralInfo updatedGenInfo = db.CulvertGeneralInfoes.Find(CulvertId);

            updatedGenInfo.XCoord = XCoordinate;
            updatedGenInfo.YCoord = YCoordinate;
            updatedGenInfo.UtmZoneId = db.UtmZoneEthiopias.Where(s => s.UtmZone == UtmZone).FirstOrDefault().UtmZoneId;

            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult ChangeZone(string CulvertId, int NextUtmZone)
        {
            CulvertGeneralInfo updatedGenInfo = db.CulvertGeneralInfoes.Find(CulvertId);

            updatedGenInfo.UtmZoneId = db.UtmZoneEthiopias.Where(s => s.UtmZone == NextUtmZone).FirstOrDefault().UtmZoneId;

            db.SaveChanges();

            return new EmptyResult();
        }


    }
}
