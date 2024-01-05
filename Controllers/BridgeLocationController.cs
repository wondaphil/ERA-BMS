using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CoordinateSharp;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class BridgeLocationController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: BridgeLocation
        public ActionResult Index()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;
            
            return View();
        }

        public ActionResult _GetBridgeLocationAll()
        {
            List<Bridge> BridgeLocation = (from br in db.Bridges
                                           select br).ToList();

            return PartialView(BridgeLocation);
        }

        public ActionResult _GetBridgeLocationByDistrict(string districtid /* drop down value */)
        {
            List<Bridge> BridgeLocation = (from br in db.Bridges
                                           join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                           from seg in table1.ToList()
                                           join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                           from sec in table2.ToList()
                                           where sec.DistrictId == districtid
                                           select br).ToList();

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(BridgeLocation);
        }

        public ActionResult _GetBridgeLocationBySection(string sectionid /* drop down value */)
        {
            List<Bridge> BridgeLocation = (from br in db.Bridges
                                           join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                           from seg in table1.ToList()
                                           where seg.SectionId == sectionid
                                           select br).ToList();

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(BridgeLocation);
        }

        public ActionResult _GetBridgeLocationBySegment(string segmentid /* drop down value */)
        {
            List<Bridge> BridgeLocation = (from br in db.Bridges
                                           where br.SegmentId == segmentid
                                           select br).ToList();

            ViewBag.SegmentId = db.Segments.Find(segmentid).SegmentId.ToString();
            ViewBag.SegmentNo = db.Segments.Find(segmentid).SegmentNo.ToString();
            ViewBag.SegmentName = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.SectionName = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.DistrictName = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(BridgeLocation);
        }

        public ActionResult BridgeLocationByRoute()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();

            return View();
        }

        public ActionResult _GetBridgeLocationByMainRoute(string mainrouteid /* drop down value */)
        {
            List<BridgeSubRouteViewModel> BridgeLocation = (from br in db.Bridges
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

            return PartialView(BridgeLocation);
        }
        

        public ActionResult _GetBridgeLocationBySubRoute(string subrouteid /* drop down value */)
        {
            List<BridgeSubRouteViewModel> BridgeLocation = (from br in db.Bridges
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

            return PartialView(BridgeLocation);
        }

        public ActionResult BridgeMap(string id) // bridge id
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

            List<BridgeMedia> imageList = (from s in db.BridgeMedias
                                           where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 // MediaTypeId = 1 for image
                                           select s).ToList();
            BridgeMedia image = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // ImageNo = 1 for side view

            ViewBag.ImageFilePath = (image != null) ? image.MediaFilePath : null;

            if (bridge.BridgeGeneralInfo != null)
            {
                if (bridge.BridgeGeneralInfo.XCoord != null && bridge.BridgeGeneralInfo.YCoord != null && bridge.BridgeGeneralInfo.UtmZoneId != null)
                {
                    ViewBag.XCoord = bridge.BridgeGeneralInfo.XCoord;
                    ViewBag.YCoord = bridge.BridgeGeneralInfo.YCoord;
                    ViewBag.UtmZone = bridge.BridgeGeneralInfo.UtmZoneEthiopia.UtmZone;
                    ViewBag.LatitudeDecimal = bridge.BridgeGeneralInfo.LatitudeDecimal.Value.ToString("#,0.000000");
                    ViewBag.LongitudeDecimal = bridge.BridgeGeneralInfo.LongitudeDecimal.Value.ToString("#,0.000000");
                    ViewBag.LatitudeDMS = bridge.BridgeGeneralInfo.LatitudeDMS;
                    ViewBag.LongitudeDMS = bridge.BridgeGeneralInfo.LongitudeDMS;
                }

                ViewBag.BridgeLength = bridge.SuperStructure.TotalSpanLength;
            }
            else
            {
                ViewBag.XCoord = ViewBag.YCoord = null;
            }

            if (bridge.SuperStructure != null)
            {
                ViewBag.BridgeType = bridge.SuperStructure.BridgeType.BridgeTypeName;
            }

            return View(bridge);
        }

        public ActionResult BridgeMapBySegment(string id) // road segment id
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Bridge> bridges = db.Bridges.Where(br => br.SegmentId == id).ToList();

            if (bridges == null)
            {
                return HttpNotFound();
            }

            ViewBag.SegmentNo = db.Segments.Find(id).SegmentNo.ToString();
            ViewBag.SegmentName = db.Segments.Find(id).SegmentName.ToString();

            //var coordinates = db.GPSCoordinates.ToList();

            var bridgeLocList = (from br in bridges
                                 select new BridgeLocationViewModel
                                 {
                                     BridgeId = br.BridgeId,
                                     OldBridgeId = br.BridgeNo,
                                     RevisedBridgeId = br.RevisedBridgeNo,
                                     BridgeName = br.BridgeName,
                                     District = br.Segment.Section.District.DistrictName,
                                     //Section = br.Segment.Section.SectionNo + " - " + br.Segment.Section.SectionName,
                                     Segment = "[" + br.Segment.RevisedRoadId + "] " + br.Segment.SegmentName,
                                     //MainRoute = br.SubRoute.MainRoute.MainRouteNo + " - " + br.SubRoute.MainRoute.MainRouteName,
                                     //SubRoute = br.SubRoute.SubRouteNo + " - " + br.SubRoute.SubRouteName
                                 }).ToList();

            foreach (var bridgeLoc in bridgeLocList)
            {
                BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeLoc.BridgeId);
                SuperStructure supStr = db.SuperStructures.Find(bridgeLoc.BridgeId);
                BridgeMedia brgMedia = db.BridgeMedias.Where(s => s.BridgeId == bridgeLoc.BridgeId && s.MediaTypeId == 1 && s.ImageNo == 1).FirstOrDefault();
                bridgeLoc.ImageFilePath = brgMedia != null ? brgMedia.MediaFilePath : "";

                bridgeLoc.XCoord = genInfo != null ? genInfo.XCoord : null;
                bridgeLoc.YCoord = genInfo != null ? genInfo.YCoord : null;
                bridgeLoc.UtmZone = genInfo != null ? (int?)genInfo.UtmZoneEthiopia.UtmZone : null;

                if (genInfo != null)
                {
                    if (genInfo.XCoord != null && genInfo.YCoord != null && genInfo.UtmZoneId != null)
                    {
                        bridgeLoc.LatitudeDecimal = genInfo.LatitudeDecimal;
                        bridgeLoc.LongitudeDecimal = genInfo.LongitudeDecimal;
                        bridgeLoc.LatitudeDMS = genInfo.LatitudeDMS;
                        bridgeLoc.LongitudeDMS = genInfo.LongitudeDMS;
                    }
                }

                if (supStr != null)
                {
                    bridgeLoc.BridgeLength = supStr.TotalSpanLength;
                    bridgeLoc.BridgeType = supStr.BridgeType.BridgeTypeName;
                }
            }

            ViewBag.BridgeLocationList = bridgeLocList;

            return View();
        }

        public ActionResult BridgeMapBySubRoute(string id) // sub route id
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Bridge> bridges = db.Bridges.Where(br => br.SubRouteId == id).ToList();

            if (bridges == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubRouteNo = db.SubRoutes.Find(id).SubRouteNo.ToString();
            ViewBag.SubRouteName = db.SubRoutes.Find(id).SubRouteName.ToString();

            //var coordinates = db.GPSCoordinates.ToList();

            var bridgeLocList = (from br in bridges
                                 select new BridgeLocationViewModel
                                 {
                                     BridgeId = br.BridgeId,
                                     OldBridgeId = br.BridgeNo,
                                     RevisedBridgeId = br.RevisedBridgeNo,
                                     BridgeName = br.BridgeName,
                                     District = br.Segment.Section.District.DistrictName,
                                     //Section = br.Segment.Section.SectionNo + " - " + br.Segment.Section.SectionName,
                                     Segment = "[" + br.Segment.RevisedRoadId + "] " + br.Segment.SegmentName,
                                     //MainRoute = br.SubRoute.MainRoute.MainRouteNo + " - " + br.SubRoute.MainRoute.MainRouteName,
                                     //SubRoute = br.SubRoute.SubRouteNo + " - " + br.SubRoute.SubRouteName
                                 }).ToList();

            foreach (var bridgeLoc in bridgeLocList)
            {
                BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeLoc.BridgeId);
                SuperStructure supStr = db.SuperStructures.Find(bridgeLoc.BridgeId);
                BridgeMedia brgMedia = db.BridgeMedias.Where(s => s.BridgeId == bridgeLoc.BridgeId && s.MediaTypeId == 1 && s.ImageNo == 1).FirstOrDefault();
                bridgeLoc.ImageFilePath = brgMedia != null ? brgMedia.MediaFilePath : "";

                bridgeLoc.XCoord = genInfo != null ? genInfo.XCoord : null;
                bridgeLoc.YCoord = genInfo != null ? genInfo.YCoord : null;
                bridgeLoc.UtmZone = genInfo != null ? (int?)genInfo.UtmZoneEthiopia.UtmZone : null;

                if (genInfo != null)
                {
                    if (genInfo.XCoord != null && genInfo.YCoord != null && genInfo.UtmZoneId != null)
                    {
                        bridgeLoc.LatitudeDecimal = genInfo.LatitudeDecimal;
                        bridgeLoc.LongitudeDecimal = genInfo.LongitudeDecimal;
                        bridgeLoc.LatitudeDMS = genInfo.LatitudeDMS;
                        bridgeLoc.LongitudeDMS = genInfo.LongitudeDMS;
                    }
                }

                if (supStr != null)
                {
                    bridgeLoc.BridgeLength = supStr.TotalSpanLength;
                    bridgeLoc.BridgeType = supStr.BridgeType.BridgeTypeName;
                }
            }

            ViewBag.BridgeLocationList = bridgeLocList;

            return View();
        }

        public ActionResult PileFoundationBridgesMap()
        {
            // Since Pier & Foundation (Piers) has a one-to-many relationship with Bridge, select one distinct item
            var pierList = (from pier in db.Piers
                            where pier.FoundationType.FoundationTypeName.ToLower().Contains("pile")
                            select new
                            {
                                BridgeId = pier.BridgeId,
                                Foundation = pier.FoundationType
                            }).Distinct();

            var bridgeLocList = (from br in db.Bridges.ToList()
                                 join pier in pierList.ToList() on br.BridgeId equals pier.BridgeId into table1
                                 from pier in table1.ToList()
                                 select new BridgeLocationViewModel
                                 {
                                     BridgeId = br.BridgeId,
                                     OldBridgeId = br.BridgeNo,
                                     RevisedBridgeId = br.RevisedBridgeNo,
                                     BridgeName = br.BridgeName,
                                     District = br.Segment.Section.District.DistrictName,
                                     Segment = "[" + br.Segment.RevisedRoadId + "] " + br.Segment.SegmentName,
                                     FoundationType = pier.Foundation.FoundationTypeName
                                 }).ToList();

            foreach (var bridgeLoc in bridgeLocList)
            {
                BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeLoc.BridgeId);
                SuperStructure supStr = db.SuperStructures.Find(bridgeLoc.BridgeId);
                BridgeMedia brgMedia = db.BridgeMedias.Where(s => s.BridgeId == bridgeLoc.BridgeId && s.MediaTypeId == 1 && s.ImageNo == 1).FirstOrDefault();
                bridgeLoc.ImageFilePath = brgMedia != null ? brgMedia.MediaFilePath : "";

                bridgeLoc.XCoord = genInfo != null ? genInfo.XCoord : null;
                bridgeLoc.YCoord = genInfo != null ? genInfo.YCoord : null;
                bridgeLoc.UtmZone = genInfo != null ? (int?)genInfo.UtmZoneEthiopia.UtmZone : null;

                if (genInfo != null)
                {
                    if (genInfo.XCoord != null && genInfo.YCoord != null && genInfo.UtmZoneId != null)
                    {
                        bridgeLoc.LatitudeDecimal = genInfo.LatitudeDecimal;
                        bridgeLoc.LongitudeDecimal = genInfo.LongitudeDecimal;
                        bridgeLoc.LatitudeDMS = genInfo.LatitudeDMS;
                        bridgeLoc.LongitudeDMS = genInfo.LongitudeDMS;
                    }
                }

                if (supStr != null)
                {
                    bridgeLoc.BridgeLength = supStr.TotalSpanLength;
                    bridgeLoc.BridgeType = supStr.BridgeType.BridgeTypeName;
                }
            }

            ViewBag.BridgeLocationList = bridgeLocList;

            return View();
        }

        public ActionResult AbandonedBridgesMap(string bridgeListString)
        {
            AbandonedBridgeViewModel bridgeList = Newtonsoft.Json.JsonConvert.DeserializeObject<AbandonedBridgeViewModel>(bridgeListString);
            List<Bridge> allBridges = new List<Bridge>();
            Bridge abandonedBridge = db.Bridges.Where(b => b.BridgeNo == bridgeList.AbandonedBridgeNo).FirstOrDefault();
            if (abandonedBridge != null)
                allBridges.Add(abandonedBridge); // add the abandoned bridge as the 1st item in the list

            // add the new bridges in the list
            foreach (var newbrg in bridgeList.NewBridgeNos)
            {
                Bridge newBridge = db.Bridges.Where(b => b.BridgeNo == newbrg).FirstOrDefault();

                if (newBridge != null)
                    allBridges.Add(newBridge);
            }
            var bridgeLocList = (from br in allBridges
                                 select new BridgeLocationViewModel
                                 {
                                     BridgeId = br.BridgeId,
                                     OldBridgeId = br.BridgeNo,
                                     RevisedBridgeId = br.RevisedBridgeNo,
                                     BridgeName = br.BridgeName,
                                     District = br.Segment.Section.District.DistrictName,
                                     Segment = "[" + br.Segment.RevisedRoadId + "] " + br.Segment.SegmentName,
                                 }).ToList();

            foreach (var bridgeLoc in bridgeLocList)
            {
                BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(bridgeLoc.BridgeId);
                SuperStructure supStr = db.SuperStructures.Find(bridgeLoc.BridgeId);
                BridgeMedia brgMedia = db.BridgeMedias.Where(s => s.BridgeId == bridgeLoc.BridgeId && s.MediaTypeId == 1 && s.ImageNo == 1).FirstOrDefault();
                bridgeLoc.ImageFilePath = brgMedia != null ? brgMedia.MediaFilePath : "";

                bridgeLoc.XCoord = genInfo != null ? genInfo.XCoord : null;
                bridgeLoc.YCoord = genInfo != null ? genInfo.YCoord : null;
                bridgeLoc.UtmZone = genInfo != null ? (int?)genInfo.UtmZoneEthiopia.UtmZone : null;

                if (genInfo != null)
                {
                    if (genInfo.XCoord != null && genInfo.YCoord != null)
                    {
                        bridgeLoc.LatitudeDecimal = genInfo.LatitudeDecimal;
                        bridgeLoc.LongitudeDecimal = genInfo.LongitudeDecimal;
                        bridgeLoc.LatitudeDMS = genInfo.LatitudeDMS;
                        bridgeLoc.LongitudeDMS = genInfo.LongitudeDMS;
                    }
                }

                if (supStr != null)
                {
                    bridgeLoc.BridgeLength = supStr.TotalSpanLength;
                    bridgeLoc.BridgeType = supStr.BridgeType.BridgeTypeName;
                }
            }

            ViewBag.BridgeLocationList = bridgeLocList;
            ViewBag.AbandonedBridge = abandonedBridge;

            List<Bridge> newBridges = new List<Bridge>();
            foreach (var newbrgno in bridgeList.NewBridgeNos)
            {
                Bridge newbrg = db.Bridges.Where(b => b.BridgeNo == newbrgno).FirstOrDefault();
                newBridges.Add(newbrg);
            }
            ViewBag.NewBridges = newBridges;

            return View();
        }

        public ActionResult UpdateCoordinate(string BridgeId, double XCoordinate, double YCoordinate, int UtmZone)
        {
            BridgeGeneralInfo updatedGenInfo = db.BridgeGeneralInfoes.Find(BridgeId);

            updatedGenInfo.XCoord = XCoordinate;
            updatedGenInfo.YCoord = YCoordinate;
            updatedGenInfo.UtmZoneId = db.UtmZoneEthiopias.Where(s => s.UtmZone == UtmZone).FirstOrDefault().UtmZoneId;

            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult ChangeZone(string BridgeId, int NextUtmZone)
        {
            BridgeGeneralInfo updatedGenInfo = db.BridgeGeneralInfoes.Find(BridgeId);

            updatedGenInfo.UtmZoneId = db.UtmZoneEthiopias.Where(s => s.UtmZone == NextUtmZone).FirstOrDefault().UtmZoneId;

            db.SaveChanges();

            return new EmptyResult();
        }

        public ActionResult CoordinateConversion()
        {
            return View();
        }

        // If DMS is 0, it returns in Decimal Format (e.g. 8.80241005192383)
        // If DMS is 1, it returns in Degree-Minute-Second Format (e.g. N 8º 48' 8.676")
        public string ConvertToLatLang(double xcoord, double ycoord, int longzone, int DMS = 0)
        {
            string latzone = "N";
            UniversalTransverseMercator utm = new UniversalTransverseMercator(latzone, longzone, xcoord, ycoord);
            Coordinate coord = UniversalTransverseMercator.ConvertUTMtoLatLong(utm);

            if (DMS == 0)
                // A json string of the form [{ "Latitude": 8.9795878, "Longitude": 38.7224086 }]
                return "[{\"Latitude\": " + coord.Latitude.DecimalDegree + ", \"Longitude\": " + coord.Longitude.DecimalDegree + "}]";
            else if (DMS == 1)
                // A json string of the form [{ "Latitude": "N 8º 51' 19.992"", "Longitude": "E 39º 13' 58.705"" }]
                return "[{\"LatitudeDMS\": " + coord.Latitude.ToString() + ", \"LongitudeDMS\": " + coord.Longitude.ToString() + "}]";
            else
                return "";
        }

        public string ConvertToUTM(double latitude, double longitude)
        {
            Coordinate coord = new Coordinate(latitude, longitude);

            // A json string of the form [{ "XCoordinate": 469487.0436, "YCoordinate": 992607.0217, "LatZone": "N", "LongZone": 37 }]
            return "[{\"XCoordinate\": " + coord.UTM.Easting + ", \"YCoordinate\": " + coord.UTM.Northing + ", \"UtmZone\": " + coord.UTM.LongZone + "}]";
        }

        public ActionResult RoadsMap()
        {
            var districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;
            ViewBag.TotalRecords = districtList.Count();

            return View();
        }

        public ActionResult _GetSegmentsListBySection(string sectionid /* drop down value */)
        {
            List<Segment> SegmentList = (from s in db.Segments
                                         where s.SectionId == sectionid
                                         select s).ToList();

            //return a partial view so as to return clean html (avoid headers, footers, menu etc)
            return PartialView(SegmentList);
        }

        public ActionResult _GetSegmentsListByDistrict(string districtid /* drop down value */)
        {
            List<Segment> SegmentList = (from seg in db.Segments
                                        join sec in db.Sections on seg.SectionId equals sec.SectionId into table1
                                        from sec in table1.ToList()
                                        where sec.DistrictId == districtid
                                        select seg).ToList();

            return PartialView(SegmentList);
        }



        //public ActionResult _GetRoadsMapBySegment(string id) // road segment id
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    List<Bridge> bridges = db.Bridges.Where(br => br.SegmentId == id).ToList();
        //    //List<Culvert> culverts = db.Culverts.Where(br => br.SegmentId == id).ToList();

        //    if (bridges == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var roadPointList = (from br in bridges
        //                         select new RoadPointViewModel
        //                         {
        //                             lat = br.BridgeGeneralInfo.LatitudeDecimal,
        //                             lng = br.BridgeGeneralInfo.LongitudeDecimal,
        //                         }).ToList();

        //    ViewBag.RoadPointList = SortCoordinates(roadPointList);

        //    return View();
        //}

        /// <summary>
        /// Sort coordinates in the list based on distance to each other
        /// </summary>
        /// <param name="roadPointList">List of road coordinates</param>
        /// <returns>A sorted list of road coordinates</returns>
        //private List<RoadPointViewModel> SortCoordinates(List<RoadPointViewModel> roadPointList)
        //{
        //    var rdPts = roadPointList;

        //    for (int i = 0; i < rdPts.Count; i++)
        //    {
        //        for (int j = i + 2; j < rdPts.Count; j++)
        //        {
        //            double dist1 = CalculateDistance(rdPts[i], rdPts[i + 1]);
        //            double dist2 = CalculateDistance(rdPts[i], rdPts[j]);
        //            if (dist1 > dist2)
        //            {
        //                var swap = rdPts[i + 1];
        //                rdPts[i + 1] = rdPts[j];
        //                rdPts[j] = swap;
        //            }
        //        }
        //    }

        //    return rdPts;
        //}

        /// <summary>
        /// Calculates distance between two coordinate points
        /// </summary>
        /// <param name="loc1">The first location</param>
        /// <param name="loc2">The second location</param>
        /// <returns></returns>
        //private double CalculateDistance(RoadPointViewModel loc1, RoadPointViewModel loc2)
        //{
        //    var location1 = new GeoCoordinate((double) loc1.lat, (double) loc1.lng);
        //    var location2 = new GeoCoordinate((double) loc2.lat, (double) loc2.lng);
            
        //    return location1.GetDistanceTo(location2);
        //}


        //public static void UTMToLatitudeLongitude(double utmX, double utmY, string utmZone, out double latitude, out double longitude)
        //{
        //    bool isNorthHemisphere = utmZone.Last() >= 'N';

        //    var diflat = -0.00066286966871111111111111111111111111;
        //    var diflon = -0.0003868060578;

        //    var zone = int.Parse(utmZone.Remove(utmZone.Length - 1));
        //    var c_sa = 6378137.000000;
        //    var c_sb = 6356752.314245;
        //    var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
        //    var e2cuadrada = Math.Pow(e2, 2);
        //    var c = Math.Pow(c_sa, 2) / c_sb;
        //    var x = utmX - 500000;
        //    var y = isNorthHemisphere ? utmY : utmY - 10000000;

        //    var s = ((zone * 6.0) - 183.0);
        //    var lat = y / (c_sa * 0.9996);
        //    var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
        //    var a = x / v;
        //    var a1 = Math.Sin(2 * lat);
        //    var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
        //    var j2 = lat + (a1 / 2.0);
        //    var j4 = ((3 * j2) + a2) / 4.0;
        //    var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
        //    var alfa = (3.0 / 4.0) * e2cuadrada;
        //    var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
        //    var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
        //    var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
        //    var b = (y - bm) / v;
        //    var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
        //    var eps = a * (1 - (epsi / 3.0));
        //    var nab = (b * (1 - epsi)) + lat;
        //    var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
        //    var delt = Math.Atan(senoheps / (Math.Cos(nab)));
        //    var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

        //    longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
        //    latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        //}
    }
}
