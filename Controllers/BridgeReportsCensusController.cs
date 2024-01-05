using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class BridgeReportsCensusController : Controller
    {
        private BMSEntities db = new BMSEntities();
        
        public ActionResult _GetAllBridges()
        {
            List<BSSDViewModel> BridgeList = (from br in db.Bridges
                                                join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                                                from seg in table1.ToList()
                                                join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                                from sec in table2.ToList()
                                                join dis in db.Districts on sec.DistrictId equals dis.DistrictId into table3
                                                from dis in table3.ToList()
                                                select new BSSDViewModel
                                                {
                                                    bridges = br,
                                                    segments = seg,
                                                    sections = sec,
                                                    districts = dis
                                                }).ToList();

            return PartialView(BridgeList);
        }

        public ActionResult AllBridges()
        {
            return View();
        }

        //public ActionResult AllBridges()
        //{
        //    //var BridgeList = db.Bridges.Include(b => b.Route).Include(b => b.Segment);
        //    var BridgeList = db.Bridges.Include(b => b.Segment);

        //    return View(BridgeList);
        //}

        public ActionResult BridgesWithDistrict()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();
            List<District> districts = db.Districts.ToList();

            var bridgeRecord = from br in bridges
                               join seg in segments on br.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               join sec in sections on seg.SectionId equals sec.SectionId into table2
                               from sec in table2.ToList()
                               join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                               from dis in table3.ToList()
                               select new BSSDViewModel
                               {
                                   bridges = br,
                                   segments = seg,
                                   sections = sec,
                                   districts = dis
                               };

            return View(bridgeRecord);
        }

        public ActionResult _GetBridgesCount()
        {
            return View();
        }

        public ActionResult CountReport()
        {
            List<SelectListItem> countReportTypeList = new List<SelectListItem>();

            //Add items
            countReportTypeList.Insert(0, new SelectListItem { Text = "--Select Count Report Type--", Value = "0" });
            countReportTypeList.Insert(1, new SelectListItem { Text = "Bridges Count", Value = "1" });
            countReportTypeList.Insert(2, new SelectListItem { Text = "Culverts Count By Culvert Type", Value = "2" });
            countReportTypeList.Insert(3, new SelectListItem { Text = "Bridges & Culverts Count", Value = "3" });
            
            ViewBag.CountReportTypes = countReportTypeList;
            return View();
        }

        public ActionResult BridgesCount()
        {
            return PartialView();
        }

        public ActionResult _GetBridgesAndCulvertsCountByDistrict()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();
            List<District> districts = db.Districts.ToList();

            // Get bridge count by district
            var bridgeCount = from br in bridges
                              join seg in segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              join sec in sections on seg.SectionId equals sec.SectionId into table2
                              from sec in table2.ToList()
                              join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                              from dis in table3.ToList()
                              group dis by new { dis } into g
                              select new
                              {
                                  District = g.Key.dis,
                                  BridgeCount = g.Count()
                              };

            // Get culvert count by district
            var culvertCount = from cul in culverts
                               join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               join sec in sections on seg.SectionId equals sec.SectionId into table2
                               from sec in table2.ToList()
                               join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                               from dis in table3.ToList()
                               group dis by new { dis } into g
                               select new
                               {
                                   District = g.Key.dis,
                                   CulvertCount = g.Count()
                               };

            // Join bridge count by district and culvert count by district
            var noofbridgesandculvertsbydistrict = ( from br in bridgeCount
                                                     join cul in culvertCount on br.District equals cul.District into table1
                                                     from cul in table1.ToList()
                                                     select new BridgeCulvertDistrictSectionSegmentViewModel
                                                     {
                                                         Districts = br.District,
                                                         BridgeCount = br.BridgeCount,
                                                         CulvertCount = cul.CulvertCount
                                                     }).ToList();

            return PartialView(noofbridgesandculvertsbydistrict);
        }

        public ActionResult _GetBridgesAndCulvertsCountByMainRoute()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<SubRoute> subroutes = db.SubRoutes.ToList();
            List<MainRoute> mainroutes = db.MainRoutes.ToList();

            // Some Main Routes have both briges and culverts
            // Other may have briges but not culverts
            // Still others may have culverts but not briges
            // So this report needs to handle all three cases

            // Get bridge count by main route
            var bridgeCount = (from br in bridges
                              join sub in subroutes on br.SubRouteId equals sub.SubRouteId into table1
                              from sub in table1.ToList()
                              join main in mainroutes on sub.MainRouteId equals main.MainRouteId into table2
                              from main in table2.ToList()
                              group main by new { main } into g
                              select new
                              {
                                  MainRoutes = g.Key.main,
                                  BridgeCount = g.Count()
                              }).ToList();

            // Get culvert count by main route
            var culvertCount = (from br in culverts
                               join sub in subroutes on br.SubRouteId equals sub.SubRouteId into table1
                               from sub in table1.ToList()
                               join main in mainroutes on sub.MainRouteId equals main.MainRouteId into table2
                               from main in table2.ToList()
                               group main by new { main } into g
                               select new
                               {
                                   MainRoutes = g.Key.main,
                                   CulvertCount = g.Count()
                               }).ToList();

            // Left join bridgeCount with culvertCount to include those mainroutes which have bridges but not culverts 
            var bridgeleftjoinculvert = (from br in bridgeCount
                                         join cul in culvertCount on br.MainRoutes equals cul.MainRoutes into table1
                                         from cul in table1.DefaultIfEmpty().ToList()
                                         select new BridgeCulvertMainRouteSubRouteViewModel
                                         {
                                             MainRoutes = br.MainRoutes,
                                             BridgeCount = br?.BridgeCount, // check for null value
                                             CulvertCount = cul?.CulvertCount // check for null value
                                         }).ToList();

            // Left join culvertCount with bridgeCount to include those mainroutes which have culverts but not bridges
            var culvertleftjoinbridge = (from cul in culvertCount
                                         join br in bridgeCount on cul.MainRoutes equals br.MainRoutes into table1
                                         from br in table1.DefaultIfEmpty().ToList()
                                         select new BridgeCulvertMainRouteSubRouteViewModel
                                         {
                                             MainRoutes = cul.MainRoutes,
                                             BridgeCount = br?.BridgeCount, // check for null value
                                             CulvertCount = cul?.CulvertCount // check for null value
                                         }).ToList();

            // Get those mainroutes which have only bridges but not culverts
            var bridgeminusculvert = bridgeleftjoinculvert.Except(culvertleftjoinbridge).ToList();

            // Get those mainroutes which have only culverts but not bridges
            var culvertminusbridge = culvertleftjoinbridge.Except(bridgeleftjoinculvert).ToList();
            
            // Take the union of the two but remove duplicates (Union -> Group them by all fields -> Take the First occurence)
            var noofbridgesandculvertsbymainroute = bridgeleftjoinculvert.Union(culvertleftjoinbridge).GroupBy(x => new { x.MainRoutes, x.BridgeCount, x.CulvertCount }).Select(g => g.First()).ToList();

            return PartialView(noofbridgesandculvertsbymainroute);
        }

        public ActionResult _GetBridgesAndCulvertsCountBySubRoute(string mainrouteid)
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<SubRoute> subroutes = db.SubRoutes.ToList();

            // Get bridge count by subRoute
            var bridgeCount = from br in bridges
                              join sub in subroutes on br.SubRouteId equals sub.SubRouteId into table2
                              from sub in table2.ToList()
                              where sub.MainRouteId == mainrouteid
                              group sub by new { sub } into g
                              select new
                              {
                                  SubRoutes = g.Key.sub,
                                  BridgeCount = g.Count(),
                              };

            // Get culvert count by subRoute
            var culvertCount = from cul in culverts
                               join sub in subroutes on cul.SubRouteId equals sub.SubRouteId into table2
                               from sub in table2.ToList()
                               where sub.MainRouteId == mainrouteid
                               group sub by new { sub } into g
                               select new
                               {
                                   SubRoutes = g.Key.sub,
                                   CulvertCount = g.Count(),
                               };

            // Left join bridgeCount with culvertCount to include those mainroutes which have bridges but not culverts 
            var bridgeleftjoinculvert = (from br in bridgeCount
                                         join cul in culvertCount on br.SubRoutes equals cul.SubRoutes into table1
                                         from cul in table1.DefaultIfEmpty().ToList()
                                         select new BridgeCulvertMainRouteSubRouteViewModel
                                         {
                                             SubRoutes = br.SubRoutes,
                                             BridgeCount = br?.BridgeCount, // check for null value
                                             CulvertCount = cul?.CulvertCount // check for null value
                                         }).ToList();


            // Left join culvertCount with bridgeCount to include those mainroutes which have culverts but not bridges
            var culvertleftjoinbridge = (from cul in culvertCount
                                         join br in bridgeCount on cul.SubRoutes equals br.SubRoutes into table1
                                         from br in table1.DefaultIfEmpty().ToList()
                                         select new BridgeCulvertMainRouteSubRouteViewModel
                                         {
                                             SubRoutes = cul.SubRoutes,
                                             BridgeCount = br?.BridgeCount, // check for null value
                                             CulvertCount = cul?.CulvertCount // check for null value
                                         }).ToList();
            // Get those mainroutes which have only bridges but not culverts
            var bridgeminusculvert = bridgeleftjoinculvert.Except(culvertleftjoinbridge).ToList();

            // Get those mainroutes which have only culverts but not bridges
            var culvertminusbridge = culvertleftjoinbridge.Except(bridgeleftjoinculvert).ToList();

            // Take the union of the two but remove duplicates (Union -> Group them by all fields -> Take the First occurence)
            var noofbridgesandculvertsbysubRoute = bridgeleftjoinculvert.Union(culvertleftjoinbridge).GroupBy(x => new { x.MainRoutes, x.BridgeCount, x.CulvertCount }).Select(g => g.First()).ToList();

            ViewBag.MainRouteNo = db.MainRoutes.Find(mainrouteid).MainRouteNo.ToString();
            ViewBag.MainRouteName = db.MainRoutes.Find(mainrouteid).MainRouteName.ToString();

            return PartialView(noofbridgesandculvertsbysubRoute);
        }

        public ActionResult _GetBridgesAndCulvertsCountByRegion()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<RegionalGovernment> regions = db.RegionalGovernments.ToList();
            
            // Get bridge count by region
            var bridgeCount = from br in bridges
                              join seg in segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              join reg in regions on seg.RegionalGovernmentId equals reg.RegionalGovernmentId into table2
                              from reg in table2.ToList()
                              group reg by new { reg } into g
                              select new
                              {
                                  Region = g.Key.reg,
                                  BridgeCount = g.Count()
                              };

            // Get culvert count by district
            var culvertCount = from cul in culverts
                               join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               join reg in regions on seg.RegionalGovernmentId equals reg.RegionalGovernmentId into table2
                               from reg in table2.ToList()
                               group reg by new { reg } into g
                               select new
                               {
                                   Region = g.Key.reg,
                                   CulvertCount = g.Count()
                               };

            // Join bridge count by region and culvert count by region
            var noofbridgesandculvertsbyregion = (from br in bridgeCount
                                                    join cul in culvertCount on br.Region equals cul.Region into table1
                                                    from cul in table1.ToList()
                                                    select new BridgeCulvertRegionSegmentViewModel
                                                    {
                                                        Regions = br.Region,
                                                        BridgeCount = br.BridgeCount,
                                                        CulvertCount = cul.CulvertCount
                                                    }).ToList();

            return PartialView(noofbridgesandculvertsbyregion);
        }

        public ActionResult BridgesAndCulvertsCountBySubRoute()
        {
            ViewBag.MainRoutes = LocationModel.GetMainRouteNameList();

            return PartialView();
        }

        public ActionResult BridgesAndCulvertsCount()
        {
            return PartialView();
        }

        public ActionResult _GetBridgesAndCulvertsCountBySection(string districtid)
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();

            // Get bridge count by section
            var bridgeCount = from br in bridges
                              join seg in segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              join sec in sections on seg.SectionId equals sec.SectionId into table2
                              from sec in table2.ToList()
                              where sec.DistrictId == districtid
                              group sec by new { sec } into g
                              select new
                              {
                                  Section = g.Key.sec,
                                  BridgeCount = g.Count(),
                              };

            // Get culvert count by section
            var culvertCount = from cul in culverts
                               join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               join sec in sections on seg.SectionId equals sec.SectionId into table2
                               from sec in table2.ToList()
                               where sec.DistrictId == districtid
                               group sec by new { sec } into g
                               select new
                               {
                                   Section = g.Key.sec,
                                   CulvertCount = g.Count(),
                               };

            // Join bridge count by section and culvert count by section
            var noofbridgesandculvertsbysection = (from br in bridgeCount
                                                   join cul in culvertCount on br.Section equals cul.Section into table1
                                                   //from cul in table1.ToList()
                                                   from cul in table1.ToList()
                                                   select new BridgeCulvertDistrictSectionSegmentViewModel
                                                   {
                                                       Sections = br.Section,
                                                       BridgeCount = br.BridgeCount,
                                                       CulvertCount = cul.CulvertCount
                                                   }).ToList();

            // This one is implementation for full outer join
            //  1. Use "from cul in table1.DefaultIfEmpty()" instead of "from cul in table1.ToList()"
            //  2. Do left join
            //  3. Do right join
            //  4. Take the union of left and right
            //var left = (from br in bridgeCount
            //            join cul in culvertCount on br.Section equals cul.Section into table1
            //            from cul in table1.DefaultIfEmpty()
            //            select new BridgeCulvertDistrictSectionViewModel
            //            {
            //                Sections = br.Section,
            //                BridgeCount = br.BridgeCount,
            //                CulvertCount = cul.CulvertCount
            //            }).ToList();

            //var right = (from cul in culvertCount
            //            join br in bridgeCount on cul.Section equals br.Section into table1
            //            from br in table1.DefaultIfEmpty()
            //            select new BridgeCulvertDistrictSectionViewModel
            //            {
            //                Sections = br.Section,
            //                BridgeCount = br.BridgeCount,
            //                CulvertCount = cul.CulvertCount
            //            }).ToList();

            //List<BridgeCulvertDistrictSectionViewModel> noofbridgesandculvertsbysection = left.Union(right).ToList();
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(noofbridgesandculvertsbysection);
        }

        public ActionResult BridgesAndCulvertsCountBySection()
        {
            ViewBag.DistrictsForSection = LocationModel.GetDistrictList();

            return PartialView();
        }

        public ActionResult _GetBridgesAndCulvertsCountBySegment(string sectionid)
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();

            // Get bridge count by section
            var bridgeCount = from br in bridges
                              join seg in segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              where seg.SectionId == sectionid
                              group seg by new { seg } into g
                              select new
                              {
                                  Segment = g.Key.seg,
                                  BridgeCount = g.Count(),
                              };

            // Get culvert count by section
            var culvertCount = from cul in culverts
                               join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               where seg.SectionId == sectionid
                               group seg by new { seg } into g
                               select new
                               {
                                   Segment = g.Key.seg,
                                   CulvertCount = g.Count(),
                               };

            // Join bridge count by section and culvert count by segment
            var noofbridgesandculvertsbysegment = (from br in bridgeCount
                                                   join cul in culvertCount on br.Segment equals cul.Segment into table1
                                                   //from cul in table1.ToList()
                                                   from cul in table1.ToList()
                                                   select new BridgeCulvertDistrictSectionSegmentViewModel
                                                   {
                                                       Segments = br.Segment,
                                                       BridgeCount = br.BridgeCount,
                                                       CulvertCount = cul.CulvertCount
                                                   }).ToList();

            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(noofbridgesandculvertsbysegment);
        }

        public ActionResult BridgesAndCulvertsCountBySegment()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return PartialView();
        }

        public ActionResult BridgesCountByBridgeType()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<SuperStructure> superStructures = db.SuperStructures.ToList();
            List<BridgeType> bridgeTypes = db.BridgeTypes.ToList();

            var bridgeRecord = from br in bridges
                               join sup in superStructures on br.BridgeId equals sup.BridgeId into table1
                               from sup in table1.ToList()
                               join brtyp in bridgeTypes on sup.BridgeTypeId equals brtyp.BridgeTypeId into table2
                               from brtyp in table2.ToList()
                               select new
                               {
                                   bridges = br,
                                   bridgetypes = brtyp
                               };

            var noofbridgesbybridgetype = (from c in bridgeRecord
                                           group c by new { c.bridgetypes } into g
                                           select new BridgeTypeViewModel
                                           {
                                               BridgeTypes = g.Key.bridgetypes,
                                               BridgeCount = g.Count()
                                           }).ToList();

            return PartialView(noofbridgesbybridgetype);
        }

        public ActionResult BridgesCountByConstructionYear()
        {
            List<BridgeGeneralInfo> genInfo = db.BridgeGeneralInfoes.ToList();
            List<ConstructionYear> constrYear = db.ConstructionYears.ToList();

            var bridgeRecord = from gen in genInfo
                               from con in constrYear
                               where (gen.ConstructionYear >= con.FromYear) && (gen.ConstructionYear <= con.ToYear)
                               select new
                               {
                                   genInfo = gen,
                                   constrYear = con
                               };

            var noofbridgesbyconstructionyear = (from c in bridgeRecord
                                                 group c by new { c.constrYear } into g
                                                 select new BridgeConstructionYearViewModel
                                                 {
                                                     ConstructionYear = g.Key.constrYear,
                                                     BridgeCount = g.Count()
                                                 }).ToList();

            return PartialView(noofbridgesbyconstructionyear);
        }

        public ActionResult BridgesCountByBridgeLength()
        {
            List<BridgeGeneralInfo> genInfo = db.BridgeGeneralInfoes.ToList();
            List<BridgeLength> brgLegth = db.BridgeLengths.ToList();

            var bridgeRecord = from gen in genInfo
                               from len in brgLegth
                               where (gen.BridgeLength >= len.FromLength) && (gen.BridgeLength < len.ToLength)
                               select new
                               {
                                   genInfo = gen,
                                   brgLegth = len
                               };

            var noofbridgesbybridgelength = (from c in bridgeRecord
                                                 group c by new { c.brgLegth } into g
                                                 select new BridgeLengthViewModel
                                                 {
                                                     BridgeLength = g.Key.brgLegth,
                                                     BridgeCount = g.Count()
                                                 }).ToList();

            return PartialView(noofbridgesbybridgelength);
        }

        public ActionResult _GetBridgeTypeByDistrict()
        {
            List<BridgeTypeByDistrict> bridgeTypeByDistrict = db.BridgeTypeByDistricts.OrderBy(b => b.DistrictId).ToList();

            return PartialView(bridgeTypeByDistrict);
        }

        public ActionResult _GetBridgeTypeByRegion()
        {
            List<BridgeTypeByRegion> bridgeTypeByRegion = db.BridgeTypeByRegions.OrderBy(b => b.RegionalGovernmentId).ToList();

            return PartialView(bridgeTypeByRegion);
        }

        public ActionResult _GetBridgeTypeByMainRoute()
        {
            List<BridgeTypeByMainRoute> bridgeTypeByMainRoute = db.BridgeTypeByMainRoutes.OrderBy(b => b.MainRouteId).ToList();

            return PartialView(bridgeTypeByMainRoute);
        }

        public ActionResult _GetBridgeTypeBySection(string districtid)
        {
            List<BridgeTypeBySection> bridgeTypeBySection = db.BridgeTypeBySections.Where(s => s.DistrictId == districtid).ToList();
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(bridgeTypeBySection);
        }

        public ActionResult _GetBridgeTypeBySegmentInSection(string sectionid)
        {
            List<BridgeTypeBySegment> bridgeTypeBySegment = db.BridgeTypeBySegments.Where(s => s.SectionId == sectionid).ToList();
            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(bridgeTypeBySegment);
        }

        public ActionResult _GetBridgeTypeBySegmentInDistrict(string districtid)
        {
            List<BridgeTypeBySegment> bridgeTypeBySegment = db.BridgeTypeBySegments.Where(s => s.DistrictId == districtid).ToList();
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(bridgeTypeBySegment);
        }

        // GET: BridgeTypeCensusByLocation
        public ActionResult BridgeTypeCensusByLocation()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            List<SelectListItem> reportTypeList = new List<SelectListItem>();

            //Add items
            reportTypeList.Insert(0, new SelectListItem { Text = "Bridge Type by Road Segment", Value = "1" });
            reportTypeList.Insert(1, new SelectListItem { Text = "Bridge Type by Section", Value = "2" });
            reportTypeList.Insert(2, new SelectListItem { Text = "Bridge Type by District", Value = "3" });
            reportTypeList.Insert(3, new SelectListItem { Text = "Bridge Type by Main Route", Value = "4" });
            reportTypeList.Insert(4, new SelectListItem { Text = "Bridge Type by Regional Gov't", Value = "5" });
            
            ViewBag.ReportTypes = reportTypeList;

            return View();
        }

        // GET: PileFoundationBridges
        public ActionResult PileFoundationBridges()
        {
            // Since Pier & Foundation (Piers) has a one-to-many relationship with Bridge, select one distinct item
            var pierList = (from pier in db.Piers
                            where pier.FoundationType.FoundationTypeName.ToLower().Contains("pile")
                            select new 
                            { 
                                BridgeId = pier.BridgeId, 
                                Foundation = pier.FoundationType 
                            }).Distinct();
            
            
            var bridgeFoundationList = (from br in db.Bridges.ToList()
                                        join pier in pierList.ToList() on br.BridgeId equals pier.BridgeId into table1
                                        from pier in table1.ToList()
                                        select new BridgeFoundationViewModel
                                        {
                                            Bridges = br,
                                            Foundation = pier.Foundation
                                        }).ToList();
            
            return View(bridgeFoundationList);
        }

        // GET: PileFoundationBridges
        public ActionResult AbandonedBridges()
        {
            // Find newly built bridges with suffix "N" in their id 
            List<string> bridgeNoNew = (from br in db.Bridges
                                        where br.BridgeNo.Contains("N")
                                        select br.BridgeNo).ToList();

            List<string> bridgeNoAbandoned = new List<string>();
            List<string> bridgeNoMissingAbandoned = new List<string>();

            foreach (var brNo in bridgeNoNew)
            {
                int index = brNo.LastIndexOf("N"); // Find the index of letter "N" is found
                string brgNo = brNo.Substring(0, index); // Truncate all characters starting from "N"
                Bridge brg = db.Bridges.Where(b => b.BridgeNo == brgNo).FirstOrDefault(); // Check if the abandoned bridge really exists

                if (brg != null && !bridgeNoAbandoned.Contains(brgNo))
                    bridgeNoAbandoned.Add(brgNo);
                
                if (brg == null && !bridgeNoMissingAbandoned.Contains(brgNo))
                //if (brg == null)
                    bridgeNoMissingAbandoned.Add(brgNo);
            }

            // Get union of old and new bridges
            List<string> bridgeNoList = bridgeNoNew.Union(bridgeNoAbandoned).ToList();

            List<Bridge> bridgeList = new List<Bridge>();
            foreach (var bridgeNo in bridgeNoList)
            {
                Bridge brg = db.Bridges.Where(b => b.BridgeNo == bridgeNo).FirstOrDefault();
                if (brg != null)
                    bridgeList.Add(brg);
            }

            ViewBag.BridgeList = bridgeList;
            ViewBag.NoOfMissingAbandonedBridges = bridgeNoMissingAbandoned.Count;

            return View();
        }
    }
}