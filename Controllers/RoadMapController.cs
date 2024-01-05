using ERA_BMS.Models;
using ERA_BMS.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ERA_BMS.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class RoadMapController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: RoadMap
        public ActionResult Index()
        {
            List<DropDownViewModel> districtList = LocationModel.GetDistrictList();

            districtList.RemoveAt(0); // Remove the default " --Select District-- " item
            districtList.Insert(0, new DropDownViewModel { Text = "ALL", Value = "0" }); //Add "ALL" to select all districts

            ViewBag.Districts = districtList;

            return View();
        }

        public ActionResult _GetSegmentsListBySection(string sectionid /* drop down value */)
        {
            List<Segment> SegmentList = (from s in db.Segments
                                         where s.SectionId == sectionid
                                         select s).ToList();

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName;
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName;

            return PartialView(SegmentList);
        }

        public ActionResult _GetSegmentsListByDistrict(string districtid /* drop down value */)
        {
            List<Segment> SegmentList = (from seg in db.Segments
                                         join sec in db.Sections on seg.SectionId equals sec.SectionId into table1
                                         from sec in table1.ToList()
                                         where sec.DistrictId == districtid
                                         select seg).ToList();

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName;

            return PartialView(SegmentList);
        }

        public ActionResult _GetSegmentsListAll()
        {
            List<Segment> SegmentList = (from seg in db.Segments
                                         select seg).ToList();

            return PartialView(SegmentList);
        }

        public ActionResult _GetRoadMap(string id) // road segment id
        {
            Segment segment = db.Segments.Find(id);
            Section section = db.Sections.Find(segment.SectionId);
            District district = db.Districts.Find(section.DistrictId);

            var segmentMap = new RoadMapViewModel()
            {
                SegmentNo = segment.SegmentNo,
                RoadId = segment.RoadId,
                RevisedRoadId = segment.RevisedRoadId,
                SegmentName = segment.SegmentName,
                RoadLength = segment.Length,
                SectionName = section.SectionName,
                DistrictName = district.DistrictName,
                AADT = segment.AverageDailyTraffic,
            };

            ViewBag.NoMapFile = false;

            SegmentMap map = db.SegmentMaps.Find(id);
            if (map == null)
            {
                ViewBag.NoMapFile = true;
                return View(segmentMap);
            }

            //XDocument doc = XDocument.Load(System.AppContext.BaseDirectory + "kml\\Abiy Adi  - Adwa (New).kml");
            XDocument doc = XDocument.Parse(map.MapFile);
            XElement root = doc.Root;
            XNamespace ns = root.GetDefaultNamespace();
            List<XElement> document = doc.Descendants(ns + "Document").ToList();

            List<XElement> simpleFields = new List<XElement>();
            foreach (XElement extendedData in document)
            {
                simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "LineString").Descendants(ns + "coordinates").ToList();
                break;
            }

            if (simpleFields.Count == 0)
            {
                // Try another kml format
                foreach (XElement extendedData in document)
                {
                    simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "Polygon").Descendants(ns + "outerBoundaryIs").Descendants(ns + "LinearRing").Descendants(ns + "coordinates").ToList();
                    break;
                }
            }
            // e.g.
            // <coordinates>
            //         37.69148904425893,13.79178827573948,0 37.69126536659586,13.79198856487394,0 ...
            // </coordinates>
            string coordinateStr = simpleFields[0].Value.Trim();


            // "37.69148904425893,13.79178827573948,0", "37.69126536659586,13.79198856487394,0", ...
            List<string> listStrLineElements = coordinateStr.Split(' ').ToList();


            // [lng=37.69148904425893,lat=13.79178827573948], [lng=37.69126536659586, lat=13.79198856487394], ...
            var roadPointList = (from c in listStrLineElements
                                 select new CoordinatePointViewModel
                                 {
                                     lat = Convert.ToDouble(c.Split(',')[1]),
                                     lng = Convert.ToDouble(c.Split(',')[0]),
                                 }).ToList();

            segmentMap.RoadPointList = roadPointList;

            ViewBag.BridgeLocationList = GetBridgeLocationList(id);
            ViewBag.CulvertLocationList = GetCulvertLocationList(id);

            return View(segmentMap);
        }

        public ActionResult _GetRoadsMapBySection(string id) // section id
        {
            List<Segment> segments = db.Segments.Where(s => s.SectionId == id).ToList();
            Section section = db.Sections.Find(id);
            District district = db.Districts.Find(section.DistrictId);

            ViewBag.SectionNo = section.SectionNo;
            ViewBag.SectionName = section.SectionName;
            ViewBag.DistrictName = district.DistrictName;

            var segmentMapList = (from seg in segments
                                  select new RoadMapViewModel
                                  {
                                      SegmentId = seg.SegmentId,
                                      SegmentNo = seg.SegmentNo,
                                      RoadId = seg.RoadId,
                                      RevisedRoadId = seg.RevisedRoadId,
                                      SegmentName = seg.SegmentName,
                                      SectionName = seg.Section.SectionNo + " - " + seg.Section.SectionName,
                                      DistrictName = seg.Section.District.DistrictName,
                                      RoadLength = seg.Length,
                                      AADT = seg.AverageDailyTraffic,
                                  }).ToList();

            int count = 0;
            foreach (var segmentMap in segmentMapList)
            {
                string segmentid = segmentMap.SegmentId;
                SegmentMap map = db.SegmentMaps.Find(segmentid);
                if (map == null)
                    continue;

                count++;
                //XDocument doc = XDocument.Load(System.AppContext.BaseDirectory + "kml\\Abiy Adi  - Adwa (New).kml");
                XDocument doc = XDocument.Parse(map.MapFile);
                XElement root = doc.Root;
                XNamespace ns = root.GetDefaultNamespace();
                List<XElement> document = doc.Descendants(ns + "Document").ToList();

                List<XElement> simpleFields = new List<XElement>();
                foreach (XElement extendedData in document)
                {
                    simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "LineString").Descendants(ns + "coordinates").ToList();
                    break;
                }

                if (simpleFields.Count == 0)
                {
                    // Try another kml format
                    foreach (XElement extendedData in document)
                    {
                        simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "Polygon").Descendants(ns + "outerBoundaryIs").Descendants(ns + "LinearRing").Descendants(ns + "coordinates").ToList();
                        break;
                    }
                }
                // e.g.
                // <coordinates>
                //         37.69148904425893,13.79178827573948,0 37.69126536659586,13.79198856487394,0 ...
                // </coordinates>
                string coordinateStr = simpleFields[0].Value.Trim();


                // "37.69148904425893,13.79178827573948,0", "37.69126536659586,13.79198856487394,0", ...
                List<string> listStrLineElements = coordinateStr.Split(' ').ToList();


                // [lng=37.69148904425893,lat=13.79178827573948], [lng=37.69126536659586, lat=13.79198856487394], ...
                var roadPointList = (from c in listStrLineElements
                                     select new CoordinatePointViewModel
                                     {
                                         lat = Convert.ToDouble(c.Split(',')[1]),
                                         lng = Convert.ToDouble(c.Split(',')[0]),
                                     }).ToList();

                segmentMap.RoadPointList = roadPointList;
            }

            // Remove those segments which have no road map points
            segmentMapList.RemoveAll(s => s.RoadPointList == null);

            ViewBag.DistrictBoundaryPointsList = GetDistrictBoundary(district.DistrictId);
            ViewBag.SectionBoundaryPointsList = GetSectionBoundary(id); // get a single section boundary
            ViewBag.RoadsWithMapFile = count;
            ViewBag.TotalRoads = segments.Count;

            var brgLocList = new List<BridgeLocationViewModel>();
            
            foreach (var seg in segments)
            {
                brgLocList.AddRange(GetBridgeLocationList(seg.SegmentId));
            }

            ViewBag.BridgeLocationList = brgLocList;
            
            return View(segmentMapList);
        }

        public ActionResult _GetRoadsMapByDistrict(string id) // district id
        {
            List<Segment> segments = db.Segments.Where(s => s.Section.DistrictId == id).ToList();
            List<Section> sections = db.Sections.Where(s => s.DistrictId == id).ToList();
            District district = db.Districts.Find(sections.FirstOrDefault().DistrictId);

            ViewBag.DistrictNo = district.DistrictNo;
            ViewBag.DistrictName = district.DistrictName;

            var segmentMapList = (from seg in segments
                                  select new RoadMapViewModel
                                  {
                                      SegmentId = seg.SegmentId,
                                      SegmentNo = seg.SegmentNo,
                                      RoadId = seg.RoadId,
                                      RevisedRoadId = seg.RevisedRoadId,
                                      SegmentName = seg.SegmentName,
                                      SectionName = seg.Section.SectionNo + " - " + seg.Section.SectionName,
                                      DistrictName = seg.Section.District.DistrictName,
                                      RoadLength = seg.Length,
                                      AADT = seg.AverageDailyTraffic,
                                  }).ToList();

            int count = 0;
            foreach (var segmentMap in segmentMapList)
            {
                string segmentid = segmentMap.SegmentId;
                SegmentMap map = db.SegmentMaps.Find(segmentid);
                if (map == null)
                    continue;

                count++;
                //XDocument doc = XDocument.Load(System.AppContext.BaseDirectory + "kml\\Abiy Adi  - Adwa (New).kml");
                XDocument doc = XDocument.Parse(map.MapFile);
                XElement root = doc.Root;
                XNamespace ns = root.GetDefaultNamespace();
                List<XElement> document = doc.Descendants(ns + "Document").ToList();

                List<XElement> simpleFields = new List<XElement>();
                foreach (XElement extendedData in document)
                {
                    simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "LineString").Descendants(ns + "coordinates").ToList();
                    break;
                }

                if (simpleFields.Count == 0)
                {
                    // Try another kml format
                    foreach (XElement extendedData in document)
                    {
                        simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "Polygon").Descendants(ns + "outerBoundaryIs").Descendants(ns + "LinearRing").Descendants(ns + "coordinates").ToList();
                        break;
                    }
                }
                // e.g.
                // <coordinates>
                //         37.69148904425893,13.79178827573948,0 37.69126536659586,13.79198856487394,0 ...
                // </coordinates>
                string coordinateStr = simpleFields[0].Value.Trim();


                // "37.69148904425893,13.79178827573948,0", "37.69126536659586,13.79198856487394,0", ...
                List<string> listStrLineElements = coordinateStr.Split(' ').ToList();


                // [lng=37.69148904425893,lat=13.79178827573948], [lng=37.69126536659586, lat=13.79198856487394], ...
                var roadPointList = (from c in listStrLineElements
                                     select new CoordinatePointViewModel
                                     {
                                         lat = Convert.ToDouble(c.Split(',')[1]),
                                         lng = Convert.ToDouble(c.Split(',')[0]),
                                     }).ToList();

                segmentMap.RoadPointList = roadPointList;
            }

            // Remove those segments which have no road map points
            segmentMapList.RemoveAll(s => s.RoadPointList == null);

            ViewBag.DistrictBoundaryPointsList = GetDistrictBoundary(district.DistrictId);
            ViewBag.SectionBoundaryPointsList = GetSectionBoundariesByDistrict(id);
            ViewBag.RoadsWithMapFile = count;
            ViewBag.TotalRoads = segments.Count;

            var brgLocList = new List<BridgeLocationViewModel>();

            foreach (var seg in segments)
            {
                brgLocList.AddRange(GetBridgeLocationList(seg.SegmentId));
            }

            ViewBag.BridgeLocationList = brgLocList;

            return View(segmentMapList);
        }

        public List<DistrictBoundaryViewModel> GetDistrictBoundary(string districtid)
        {
            List<DistrictMap> districtmaps = db.DistrictMaps.Where(d => d.DistrictId == districtid).ToList();

            return GetDistrictBoundaries(districtmaps);
        }

        public List<DistrictBoundaryViewModel> GetDistrictBoundariesAll()
        {
            List<DistrictMap> districtmaps = db.DistrictMaps.ToList();

            return GetDistrictBoundaries(districtmaps);
        }

        public List<DistrictBoundaryViewModel> GetDistrictBoundaries(List<DistrictMap> districtmaps)
        {
            List<DistrictBoundaryViewModel> districtPointsList = new List<DistrictBoundaryViewModel>();

            foreach (var district in districtmaps)
            {
                if (district == null)
                    continue;

                XDocument doc = XDocument.Parse(district.MapFile);
                XElement root = doc.Root;
                XNamespace ns = root.GetDefaultNamespace();
                List<XElement> document = doc.Descendants(ns + "Document").ToList();

                List<XElement> simpleFields = new List<XElement>();
                foreach (XElement extendedData in document)
                {
                    simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "LineString").Descendants(ns + "coordinates").ToList();
                    break;
                }

                if (simpleFields.Count == 0)
                {
                    // Try another kml format
                    foreach (XElement extendedData in document)
                    {
                        simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "Polygon").Descendants(ns + "outerBoundaryIs").Descendants(ns + "LinearRing").Descendants(ns + "coordinates").ToList();
                        break;
                    }
                }
                // e.g.
                // <coordinates>
                //         37.69148904425893,13.79178827573948,0 37.69126536659586,13.79198856487394,0 ...
                // </coordinates>
                string coordinateStr = simpleFields[0].Value.Trim();

                // "37.69148904425893,13.79178827573948,0", "37.69126536659586,13.79198856487394,0", ...
                List<string> listStrLineElements = coordinateStr.Split(' ').ToList();

                // [lng=37.69148904425893,lat=13.79178827573948], [lng=37.69126536659586, lat=13.79198856487394], ...
                var districtPoints = (from c in listStrLineElements
                                      select new CoordinatePointViewModel
                                      {
                                          lat = Convert.ToDouble(c.Split(',')[1]),
                                          lng = Convert.ToDouble(c.Split(',')[0]),
                                      }).ToList();

                string districtName = db.Districts.Find(district.DistrictId).DistrictName;
                List<Section> sections = db.Sections.Where(s => s.DistrictId == district.DistrictId).ToList();
                double totalLength = 0.0;

                foreach (var section in sections)
                    totalLength += db.Segments.Where(s => s.SectionId == section.SectionId).Select(t => t.Length ?? 0).Sum(); // default of 0 if length is null

                districtPointsList.Add(new DistrictBoundaryViewModel()
                {
                    DistrictBoundaryPoints = districtPoints,
                    DistrictName = districtName,
                    TotalLength = totalLength,
                });
            }

            return districtPointsList;
        }

        public List<SectionBoundaryViewModel> GetBoundaries(List<SectionMap> sectionmaps)
        {
            List<SectionBoundaryViewModel> sectionPointsList = new List<SectionBoundaryViewModel>();

            foreach (var section in sectionmaps)
            {
                if (section == null)
                    continue;

                XDocument doc = XDocument.Parse(section.MapFile);
                XElement root = doc.Root;
                XNamespace ns = root.GetDefaultNamespace();
                List<XElement> document = doc.Descendants(ns + "Document").ToList();

                List<XElement> simpleFields = new List<XElement>();
                foreach (XElement extendedData in document)
                {
                    simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "LineString").Descendants(ns + "coordinates").ToList();
                    break;
                }

                if (simpleFields.Count == 0)
                {
                    // Try another kml format
                    foreach (XElement extendedData in document)
                    {
                        simpleFields = extendedData.Descendants(ns + "Placemark").Descendants(ns + "MultiGeometry").Descendants(ns + "Polygon").Descendants(ns + "outerBoundaryIs").Descendants(ns + "LinearRing").Descendants(ns + "coordinates").ToList();
                        break;
                    }
                }
                // e.g.
                // <coordinates>
                //         37.69148904425893,13.79178827573948,0 37.69126536659586,13.79198856487394,0 ...
                // </coordinates>
                string coordinateStr = simpleFields[0].Value.Trim();

                // "37.69148904425893,13.79178827573948,0", "37.69126536659586,13.79198856487394,0", ...
                List<string> listStrLineElements = coordinateStr.Split(' ').ToList();

                // [lng=37.69148904425893,lat=13.79178827573948], [lng=37.69126536659586, lat=13.79198856487394], ...
                var sectionPoints = (from c in listStrLineElements
                                     select new CoordinatePointViewModel
                                     {
                                         lat = Convert.ToDouble(c.Split(',')[1]),
                                         lng = Convert.ToDouble(c.Split(',')[0]),
                                     }).ToList();

                string sectionName = db.Sections.Find(section.SectionId).SectionName;
                District district = db.Sections.Find(section.SectionId).District;
                double totalLength = db.Segments.Where(s => s.SectionId == section.SectionId).Select(t => t.Length ?? 0).Sum(); // default of 0 if length is null
                sectionPointsList.Add(new SectionBoundaryViewModel()
                {
                    SectionBoundaryPoints = sectionPoints,
                    SectionName = sectionName,
                    DistrictId = district.DistrictId,
                    DistrictName = district.DistrictName,
                    TotalLength = totalLength,
                });
            }

            return sectionPointsList;
        }

        public List<SectionBoundaryViewModel> GetSectionBoundary(string sectionid)
        {
            List<SectionMap> sectionmaps = db.SectionMaps.Where(s => s.SectionId == sectionid).ToList();
            
            return GetBoundaries(sectionmaps);
        }

        public List<SectionBoundaryViewModel> GetSectionBoundariesByDistrict(string districtid)
        {
            List<SectionMap> sectionmaps = (from sec in db.Sections
                                            join map in db.SectionMaps on sec.SectionId equals map.SectionId into table1
                                            from map in table1.ToList()
                                            where sec.DistrictId == districtid
                                            select map).ToList();

            return GetBoundaries(sectionmaps);
        }

        public List<SectionBoundaryViewModel> GetSectionBoundariesAll()
        {
            List<SectionMap> sectionmaps = db.SectionMaps.ToList();

            return GetBoundaries(sectionmaps);
        }

        public List<BridgeLocationViewModel> GetBridgeLocationList(string id) // road segment id
        {
            List<Bridge> bridges = db.Bridges.Where(brg => brg.SegmentId == id).ToList();

            if (bridges == null)
            {
                // if there are no bridges, simply pass empty list
                return new List<BridgeLocationViewModel>();
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
                    SuperStructure supStr = db.SuperStructures.Find(bridgeLoc.BridgeId);

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

                        if (supStr != null)
                        {
                            bridgeLoc.BridgeType = supStr.BridgeType.BridgeTypeName;
                        }
                    }
                }

                return bridgeLocList;
            }
        }
            
        public List<CulvertLocationViewModel> GetCulvertLocationList(string id) // road segment id
        {
            List<Culvert> culverts = db.Culverts.Where(cul => cul.SegmentId == id).ToList();

            if (culverts == null)
            {
                // if there are no culverts, simply pass empty list
                return new List<CulvertLocationViewModel>();
            }
            else
            {
                var culvertLocList = (from cul in culverts
                                      select new CulvertLocationViewModel
                                      {
                                          CulvertId = cul.CulvertId,
                                          OldCulvertId = cul.CulvertNo,
                                          RevisedCulvertId = cul.RevisedCulvertNo,
                                      }).ToList();

                foreach (var culvertLoc in culvertLocList)
                {
                    CulvertGeneralInfo culGenInfo = db.CulvertGeneralInfoes.Find(culvertLoc.CulvertId);
                    CulvertStructure culStr = db.CulvertStructures.Find(culvertLoc.CulvertId);

                    culvertLoc.XCoord = culGenInfo != null ? culGenInfo.XCoord : null;
                    culvertLoc.YCoord = culGenInfo != null ? culGenInfo.YCoord : null;
                    culvertLoc.UtmZone = culGenInfo != null ? (int?)culGenInfo.UtmZoneEthiopia.UtmZone : null;

                    if (culGenInfo != null)
                    {
                        if (culGenInfo.XCoord != null && culGenInfo.YCoord != null && culGenInfo.UtmZoneId != null)
                        {
                            culvertLoc.LatitudeDecimal = culGenInfo.LatitudeDecimal;
                            culvertLoc.LongitudeDecimal = culGenInfo.LongitudeDecimal;
                        }

                        if (culStr != null)
                        {
                            culvertLoc.CulvertLength = culStr.LengthTotal;
                            culvertLoc.CulvertType = culStr.CulvertType.CulvertTypeName;
                        }
                    }
                }

                return culvertLocList;
            }
        }
    }
}