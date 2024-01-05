using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssetValueController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: AssetValue
        public ActionResult Index()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public ActionResult _GetBridgeCurrentValueByDistrict(string districtid)
        {
            // Extend the default timeout from the default 15 seconds to 60 seconds
            // to give the time-taking operation more time to complete.
            // This avoids "The wait operation timed out" Win32.exception
            db.Database.CommandTimeout = 120; 
            
            var bridgeList = (from br in db.Bridges
                              join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                              from sec in table2.ToList()
                              where sec.DistrictId == districtid
                              select br).ToList();

            // Abandoned bridges should NOT be included in the asset calculations
            //bridgeList = bridgeList.Except(GetAbandonedBridges()).ToList();

            var result = (from val in db.BridgeCurrentValues.ToList()
                          join br in bridgeList on val.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select new BridgeValueViewModel
                          {
                              Bridge = br,
                              CurrentValue = val
                          }).ToList();

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetCulvertCurrentValueByDistrict(string districtid)
        {
            var culvertList = (from cul in db.Culverts
                               join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                               from sec in table2.ToList()
                               where sec.DistrictId == districtid
                               select cul).ToList(); 
            
            var result = (from val in db.CulvertCurrentValues.ToList()
                          join cul in culvertList on val.CulvertId equals cul.CulvertId into table1
                          from cul in table1.ToList()
                          select new CulvertValueViewModel
                          {
                              Culvert = cul,
                              CurrentValue = val
                          }).ToList();

            ViewBag.District = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetBridgeCurrentValueBySection(string sectionid)
        {
            // Extend the default timeout from the default 15 seconds to 60 seconds
            // to give the time-taking operation more time to complete.
            // This avoids "The wait operation timed out" Win32.exception
            db.Database.CommandTimeout = 120; 
            
            var bridgeList = (from br in db.Bridges
                              join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              where seg.SectionId == sectionid
                              select br).ToList();

            // Abandoned bridges should NOT be included in the asset calculations
            //bridgeList = bridgeList.Except(GetAbandonedBridges()).ToList();

            var result = (from val in db.BridgeCurrentValues.ToList()
                          join br in bridgeList on val.BridgeId equals br.BridgeId into table1
                          from br in table1.ToList()
                          select new BridgeValueViewModel
                          {
                              Bridge = br,
                              CurrentValue = val
                          }).ToList();

            
            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetCulvertCurrentValueBySection(string sectionid)
        {
            var culvertList = (from cul in db.Culverts
                               join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               where seg.SectionId == sectionid
                               select cul).ToList();
            
            var result = (from val in db.CulvertCurrentValues.ToList()
                          join cul in culvertList on val.CulvertId equals cul.CulvertId into table1
                          from cul in table1.ToList()
                          select new CulvertValueViewModel
                          {
                              Culvert = cul,
                              CurrentValue = val
                          }).ToList();

            ViewBag.Section = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.District = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(result);
        }

        /*
        public ActionResult _GetBridgeCurrentValueBySegment(string segmentid)
        {
            var bridgeList = (from br in db.Bridges
                              where br.SegmentId == segmentid
                              select br).ToList();

            //var result = (from val in db.BridgeCurrentValues.ToList()
            //              join brg in bridgeList on val.BridgeId equals brg.BridgeId into table1
            //              from brg in table1.ToList()
            //              select new BridgeValueViewModel
            //              {
            //                  Bridge = cul,
            //                  CurrentValue = val
            //              }).ToList();


            var result = (from br in bridgeList
                          join val in db.BridgeCurrentValues.ToList() on br.BridgeId equals val.BridgeId into table1
                          from val in table1.Distinct()
                          select new BridgeValueViewModel
                          {
                              Bridge = br,
                              CurrentValue = val
                          }).ToList();

            //var result = (from brg in db.Bridges.ToList()
            //              where brg.SegmentId == segmentid
            //              join val in db.BridgeCurrentValues.ToList() on brg.BridgeId equals val.BridgeId
            //              group brg by brg.BridgeId into table1
            //              from val in table1
            //              select new BridgeValueViewModel
            //              {
            //                  Bridge = table1.First(),

            //              }).ToList();

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(result);
        } */

        public ActionResult _GetBridgeCurrentValueBySegment(string segmentid)
        {
            // Extend the default timeout from the default 15 seconds to 60 seconds
            // to give the time-taking operation more time to complete.
            // This avoids "The wait operation timed out" Win32.exception
            db.Database.CommandTimeout = 120; 
            
            var bridgeList = (from brg in db.Bridges
                              where brg.SegmentId == segmentid
                              select brg).ToList();

            // Abandoned bridges should NOT be included in the asset calculations
            //bridgeList = bridgeList.Except(GetAbandonedBridges()).ToList();

            var result = (from val in db.BridgeCurrentValues.ToList()
                          join brg in bridgeList on val.BridgeId equals brg.BridgeId into table1
                          from brg in table1.ToList()
                          select new BridgeValueViewModel
                          {
                              Bridge = brg,
                              CurrentValue = val
                          }).ToList();

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetCulvertCurrentValueBySegment(string segmentid)
        {
            var culvertList = (from cul in db.Culverts
                              where cul.SegmentId == segmentid
                              select cul).ToList(); 
            
            var result = (from val in db.CulvertCurrentValues.ToList()
                          join cul in culvertList on val.CulvertId equals cul.CulvertId into table1
                          from cul in table1.ToList()
                          select new CulvertValueViewModel
                          {
                              Culvert = cul,
                              CurrentValue = val
                          }).ToList();

            ViewBag.Segment = db.Segments.Find(segmentid).SegmentName.ToString();
            ViewBag.Section = db.Segments.Find(segmentid).Section.SectionName.ToString();
            ViewBag.District = db.Segments.Find(segmentid).Section.District.DistrictName.ToString();

            return PartialView(result);
        }

        public ActionResult _GetTotalAssetValue()
        {
            // Extend the default timeout from the default 15 seconds to 60 seconds
            // to give the time-taking operation more time to complete.
            // This avoids "The wait operation timed out" Win32.exception
            db.Database.CommandTimeout = 120;

            List<AssetValueByDistrict> assetList = db.AssetValueByDistricts.ToList();

            return PartialView(assetList);
        }

        public ActionResult _GetBridgeValuePerLm()
        {
            // Extend the default timeout from the default 15 seconds to 60 seconds
            // to give the time-taking operation more time to complete.
            // This avoids "The wait operation timed out" Win32.exception
            db.Database.CommandTimeout = 120;

            List<BridgeValuePerLm> bridgeValuePerLmList = db.BridgeValuePerLms.ToList();

            return PartialView(bridgeValuePerLmList);
        }

        public ActionResult _GetCulvertValuePerLm()
        {
            // Extend the default timeout from the default 15 seconds to 60 seconds
            // to give the time-taking operation more time to complete.
            // This avoids "The wait operation timed out" Win32.exception
            db.Database.CommandTimeout = 120;

            List<CulvertValuePerLm> culvertValuePerLmList = db.CulvertValuePerLms.ToList();

            return PartialView(culvertValuePerLmList);
        }

        public List<Bridge> GetAbandonedBridges()
        {
            // Find newly built bridges with suffix "N" in their id 
            List<string> bridgeNoNew = (from br in db.Bridges
                                        where br.BridgeNo.Contains("N")
                                        select br.BridgeNo).ToList();

            List<string> bridgeNoAbandoned = new List<string>();
            
            foreach (var brNo in bridgeNoNew)
            {
                int index = brNo.LastIndexOf("N"); // Find the index of letter "N" is found
                string brgNo = brNo.Substring(0, index); // Truncate all characters starting from "N"
                Bridge brg = db.Bridges.Where(b => b.BridgeNo == brgNo).FirstOrDefault(); // Check if the abandoned bridge really exists

                if (brg != null && !bridgeNoAbandoned.Contains(brgNo))
                    bridgeNoAbandoned.Add(brgNo);
            }

            List<Bridge> abandonedBridges = new List<Bridge>();
            foreach (var bridgeNo in bridgeNoAbandoned)
            {
                Bridge brg = db.Bridges.Where(b => b.BridgeNo == bridgeNo).FirstOrDefault();
                if (brg != null)
                    abandonedBridges.Add(brg);
            }

            return abandonedBridges;
        }
    }
}