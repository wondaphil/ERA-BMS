using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;
using Newtonsoft.Json;

namespace ERA_BMS.Controllers
{
    public class ChartGraphController : Controller
    {
        private BMSEntities db = new BMSEntities();
        private List<SelectListItem> graphTypeList = new List<SelectListItem>();
        
        public ChartGraphController()
        {
            //Add Graph types to be shown on the dropdown list
            graphTypeList.Insert(0, new SelectListItem { Text = "--Select Graph Type--", Value = "0" });
            graphTypeList.Insert(1, new SelectListItem { Text = "No. of Bridges vs Districts", Value = "1" });
            graphTypeList.Insert(2, new SelectListItem { Text = "No. of Bridges vs Regional Gov't", Value = "21" });
            graphTypeList.Insert(3, new SelectListItem { Text = "No. of Bridges vs Bridge Types", Value = "2" });
            graphTypeList.Insert(4, new SelectListItem { Text = "No. of Bridges vs Constr./Replaced Years", Value = "3" });
            graphTypeList.Insert(5, new SelectListItem { Text = "No. of Bridges vs Structure Length", Value = "4" });
            graphTypeList.Insert(6, new SelectListItem { Text = "No. of Bridges vs Service Conditions", Value = "5" });
            graphTypeList.Insert(7, new SelectListItem { Text = "No. of Bridges vs Required Actions", Value = "19" });
            graphTypeList.Insert(8, new SelectListItem { Text = "No. of Inspected Bridges vs Inspection Year", Value = "6" });
            graphTypeList.Insert(9, new SelectListItem { Text = "No. of Registered & Inspected Bridges vs Inspection Year", Value = "18" });
            graphTypeList.Insert(10, new SelectListItem { Text = "No. of Inspected Bridges vs Districts", Value = "13" });
            graphTypeList.Insert(11, new SelectListItem { Text = "Bridge Service Conditions vs Districts", Value = "15" });
            graphTypeList.Insert(12, new SelectListItem { Text = "Bridge Required Actions vs Districts", Value = "20" });
            graphTypeList.Insert(13, new SelectListItem { Text = "Bridge Asset Values vs Districts", Value = "7" });
            graphTypeList.Insert(14, new SelectListItem { Text = "No. of Culverts vs Districts", Value = "8" });
            graphTypeList.Insert(15, new SelectListItem { Text = "No. of Culverts vs Regional Gov't", Value = "22" });
            graphTypeList.Insert(16, new SelectListItem { Text = "No. of Culverts vs Culvert Types", Value = "9" });
            graphTypeList.Insert(17, new SelectListItem { Text = "No. of Inspected Culverts vs Inspection Year", Value = "17" });
            graphTypeList.Insert(18, new SelectListItem { Text = "No. of Inspected Culverts vs Districts", Value = "14" });
            graphTypeList.Insert(19, new SelectListItem { Text = "Culvert Service Conditions vs Districts", Value = "16" });
            graphTypeList.Insert(20, new SelectListItem { Text = "Culvert Asset Values vs Districts", Value = "10" });
            graphTypeList.Insert(21, new SelectListItem { Text = "No. of Bridges/Culverts vs Districts", Value = "11" });
            graphTypeList.Insert(22, new SelectListItem { Text = "No. of Bridges/Culverts vs Regional Gov\'ts", Value = "23" });
            graphTypeList.Insert(23, new SelectListItem { Text = "Bridge/Culvert Asset Values vs Districts", Value = "12" });
        }

        // GET: BridgeGraphs
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                int graphIndex = (int)id;
                if (graphIndex >= 0 && graphIndex <= graphTypeList.Count)
                {
                    //graphTypeList[graphIndex].Selected = true;
                    graphTypeList.Where(g => g.Value == id.ToString()).FirstOrDefault().Selected = true;
                }
            }

            ViewBag.GraphTypes = graphTypeList;
            List<SelectListItem> inspectionYearList = (from DmgInspMjr in db.DamageInspMajors
                                                       orderby DmgInspMjr.InspectionYear descending
                                                       select new
                                                       {
                                                           DmgInspMjr.InspectionYear
                                                       }).Distinct().OrderByDescending(s => s.InspectionYear)
                                                       .Select(c => new SelectListItem
                                                       {
                                                           Text = c.InspectionYear.ToString(),
                                                           Value = c.InspectionYear.ToString()
                                                       }).ToList();
            
            ViewBag.InspectionYearList = inspectionYearList;

            return View();
        }

        // GET: CulvertGraph
        public ActionResult CulvertGraphs()
        {
            List<SelectListItem> graphTypeList = new List<SelectListItem>();

            //Add Graph types to be shown on the dropdown list
            graphTypeList.Insert(0, new SelectListItem { Text = "--Select Graph Type--", Value = "0" });
            graphTypeList.Insert(1, new SelectListItem { Text = "No. of Culverts vs Districts", Value = "1" });
            graphTypeList.Insert(2, new SelectListItem { Text = "No. of Culverts vs Culvert Types", Value = "2" });
            graphTypeList.Insert(3, new SelectListItem { Text = "Culvert Asset Value vs Districts", Value = "3" });

            ViewBag.GraphTypes = graphTypeList;

            return View();
        }

        public ActionResult BridgeAndCulvertGraphs()
        {
            List<SelectListItem> graphTypeList = new List<SelectListItem>();

            //Add Graph types to be shown on the dropdown list
            graphTypeList.Insert(0, new SelectListItem { Text = "--Select Graph Type--", Value = "0" });
            graphTypeList.Insert(1, new SelectListItem { Text = "No. of Bridges/Culvert vs Districts", Value = "1" });
            graphTypeList.Insert(2, new SelectListItem { Text = "Bridge/Culvert Asset Value vs Districts", Value = "2" });

            ViewBag.GraphTypes = graphTypeList;

            return View();
        }

        public DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public JsonResult _NoOfBridgesVsDistrictChart()
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
                               select new
                               {
                                   bridges = br,
                                   districts = dis
                               };

            var noofbridgesbydistrict = (from c in bridgeRecord.OrderBy(d => d.districts.DistrictNo)
                                         group c by new { c.districts } into g
                                         select new
                                         {
                                             DistrictName = g.Key.districts.DistrictName,
                                             BridgeCount = g.Count()
                                         }).ToList();

            DataTable dt = ListToDataTable(noofbridgesbydistrict);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesAndCulvertsVsDistrictChart()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList(); 
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();
            List<District> districts = db.Districts.ToList();

            var noofbridgesbydistrict = from br in bridges
                                       join seg in segments on br.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join sec in sections on seg.SectionId equals sec.SectionId into table2
                                       from sec in table2.ToList()
                                       join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                                       from dis in table3.ToList()
                                       group dis by new { dis.DistrictName } into g
                                       select new
                                       {
                                           DistrictName = g.Key.DistrictName,
                                           BridgeCount = g.Count()
                                       };

            var noofculvertsbydistrict = from cul in culverts
                                         join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                                         from seg in table1.ToList()
                                         join sec in sections on seg.SectionId equals sec.SectionId into table2
                                         from sec in table2.ToList()
                                         join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                                         from dis in table3.ToList()
                                         group dis by new { dis.DistrictName } into g
                                         select new
                                         {
                                             DistrictName = g.Key.DistrictName,
                                             CulvertCount = g.Count()
                                         };

            var noofbridgesandculvertsbydistricts = (from br in noofbridgesbydistrict
                                                     join cul in noofculvertsbydistrict on br.DistrictName equals cul.DistrictName into table1
                                                     from cul in table1.ToList()
                                                     join dis in districts on br.DistrictName equals dis.DistrictName into table2
                                                     from dis in table2.ToList()
                                                     orderby dis.DistrictNo
                                                     select new
                                                     {
                                                         DistrictName = br.DistrictName,
                                                         BridgeCount = br.BridgeCount,
                                                         CulvertCount = cul.CulvertCount
                                                     }).ToList();

            
            DataTable dt = ListToDataTable(noofbridgesandculvertsbydistricts);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesVsRegionChart()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<RegionalGovernment> regions = db.RegionalGovernments.ToList();

            var bridgeRecord = from br in bridges
                               join seg in segments on br.SegmentId equals seg.SegmentId into table1
                               from seg in table1.ToList()
                               join reg in regions on seg.RegionalGovernmentId equals reg.RegionalGovernmentId into table2
                               from reg in table2.ToList()
                               select new
                               {
                                   bridges = br,
                                   regions = reg
                               };

            var noofbridgesbyregion = (from c in bridgeRecord.OrderBy(d => d.regions.RegionalGovernmentId)
                                         group c by new { c.regions } into g
                                         select new
                                         {
                                             RegionalGovernmentName = g.Key.regions.RegionalGovernmentName,
                                             BridgeCount = g.Count()
                                         }).ToList();

            DataTable dt = ListToDataTable(noofbridgesbyregion);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesAndCulvertsVsRegionChart()
        {
            List<Bridge> bridges = db.Bridges.ToList();
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<RegionalGovernment> regions = db.RegionalGovernments.ToList();

            var noofbridgesbyregion = from br in bridges
                                        join seg in segments on br.SegmentId equals seg.SegmentId into table1
                                        from seg in table1.ToList()
                                        join reg in regions on seg.RegionalGovernmentId equals reg.RegionalGovernmentId into table2
                                        from reg in table2.ToList()
                                        group reg by new { reg.RegionalGovernmentName } into g
                                        select new
                                        {
                                            RegionalGovernmentName = g.Key.RegionalGovernmentName,
                                            BridgeCount = g.Count()
                                        };

            var noofculvertsbyregion = from cul in culverts
                                       join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                                       from seg in table1.ToList()
                                       join reg in regions on seg.RegionalGovernmentId equals reg.RegionalGovernmentId into table2
                                       from reg in table2.ToList()
                                       group reg by new { reg.RegionalGovernmentName } into g
                                       select new
                                       {
                                           RegionalGovernmentName = g.Key.RegionalGovernmentName,
                                           CulvertCount = g.Count()
                                       };

            var noofbridgesandculvertsbyregions = (from br in noofbridgesbyregion
                                                   join cul in noofculvertsbyregion on br.RegionalGovernmentName equals cul.RegionalGovernmentName into table1
                                                   from cul in table1.ToList()
                                                   join reg in regions on br.RegionalGovernmentName equals reg.RegionalGovernmentName into table2
                                                   from reg in table2.ToList()
                                                   orderby reg.RegionalGovernmentId
                                                   select new
                                                   {
                                                       RegionalGovernmentName = br.RegionalGovernmentName,
                                                       BridgeCount = br.BridgeCount,
                                                       CulvertCount = cul.CulvertCount
                                                   }).ToList();


            DataTable dt = ListToDataTable(noofbridgesandculvertsbyregions);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesVsBridgeTypeChart()
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
                                           select new
                                           {
                                               BridgeType = g.Key.bridgetypes.BridgeTypeName,
                                               BridgeCount = g.Count()
                                           }).ToList();

            DataTable dt = ListToDataTable(noofbridgesbybridgetype);

            List<object> iData = new List<object>();
            
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            
            return Json(iData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _NoOfBridgesVsConstructionYearChart()
        {
            List<BridgeGeneralInfo> genInfo = db.BridgeGeneralInfoes.ToList();
            List<ConstructionYear> constrYear = db.ConstructionYears.ToList();
            
            var bridgeRecord = from gen in genInfo
                               from con in constrYear
                               where (gen.ReplacedYear >= con.FromYear) && (gen.ReplacedYear <= con.ToYear)
                               select new
                               {
                                   genInfo = gen,
                                   constrYear = con
                               };

            var noofbridgesbyconstructionyear = (from c in bridgeRecord
                                                 group c by new { c.constrYear } into g
                                                 select new
                                                 {
                                                     ConstructionYear = g.Key.constrYear.ConstructionYears,
                                                     BridgeCount = g.Count()
                                                 }).OrderBy(s => s.ConstructionYear).ToList();

            DataTable dt = ListToDataTable(noofbridgesbyconstructionyear);

            List<object> iData = new List<object>();
            
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesVsBridgeLengthChart()
        {
            List<BridgeGeneralInfo> genInfo = db.BridgeGeneralInfoes.ToList();
            List<BridgeLength> brgLegth = db.BridgeLengths.ToList();

            var bridgeRecord = (from gen in genInfo
                               from len in brgLegth
                               where (gen.BridgeLength >= len.FromLength) && (gen.BridgeLength < len.ToLength)
                               select new
                               {
                                   genInfo = gen,
                                   brgLegth = len
                               }).OrderBy(s => s.brgLegth.BridgeLengthId);

            var noofbridgesbybridgelength = (from c in bridgeRecord
                                             group c by new { c.brgLegth } into g
                                             select new 
                                             {
                                                 BridgeLength = g.Key.brgLegth.BridgeLengthName,
                                                 BridgeCount = g.Count()
                                             }).ToList();

            DataTable dt = ListToDataTable(noofbridgesbybridgelength);

            List<object> iData = new List<object>();
            
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesVsServiceConditionChart(int inspectionyear)
        {
            List<ResultInspMajor> result = db.ResultInspMajors.ToList();
            
            // Get all bridge inspection results in the given inspection year
            var bridgeRecord = from res in result
                               where res.InspectionYear == inspectionyear
                               select new
                               {
                                   bridgeid = res.BridgeId,
                                   condition = res.DmgPerc
                               };

            // Get all bridge inspction results with bridge conditions ("Good", "Fair" and "Bad")
            // SQL Statement:-
            //      SELECT BridgeCondition.Condition, COUNT(BridgeCondition.BridgeId)
            //      FROM (  SELECT BridgeId,
            //                  CASE
            //                      WHEN DmgPerc >= 0 AND DmgPerc< 10 THEN 'Good'
            //                      WHEN DmgPerc >= 10 AND DmgPerc< 15 THEN 'Fair'
            //                      WHEN DmgPerc >= 15 AND DmgPerc< 100 THEN 'Bad'
            //                      ELSE ''
            //                  END AS Condition
            //              FROM ResultInspMajor
            //              WHERE InspectionYear = 2013) AS BridgeCondition
            //      GROUP BY Condition 

            List<DamageConditionRange> dmgRange = db.DamageConditionRanges.ToList();

            var bridgesbydmgperc = (from c in bridgeRecord
                                    select new
                                    {
                                        bridgeid = c.bridgeid,
                                        Condition =
                                            (
                                                c.condition >= dmgRange[0].ValueFrom && c.condition < dmgRange[0].ValueTo ? "Good" :
                                                c.condition >= dmgRange[1].ValueFrom && c.condition < dmgRange[1].ValueTo ? "Fair" :
                                                c.condition >= dmgRange[2].ValueFrom && c.condition <= dmgRange[2].ValueTo ? "Bad" : ""
                                            ),
                                    }).ToList();


            // Get No. of bridges vs bridge condition ("Good", "Fair" and "Bad")
            var noofbridgesbycondition = (from c in bridgesbydmgperc
                                          group c by new { c.Condition } into d
                                          select new
                                          {
                                              Condition = d.Key.Condition,
                                              BridgeCount = d.Count(),
                                          }).OrderByDescending(s => s.Condition).ToList();

            ViewBag.TotalBridges = bridgeRecord.Count();

            DataTable dt = ListToDataTable(noofbridgesbycondition);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfBridgesVsRequiredActionChart(int inspectionyear)
        {
            List<BridgePriorityByInterventionRequirement> result = db.BridgePriorityByInterventionRequirements.ToList();
            
            // Get all bridge inspection results in the given inspection year
            var bridgeReqAction = from res in result
                                  where res.InspectionYear == inspectionyear
                                  select new
                                  {
                                      bridgeid = res.BridgeId,
                                      priority = res.PriorityVal
                                  };

            // Get all bridge inspction results with bridge required action ("'Routine Maintenance", "Major Rehabilitation" and "Emergency Repair")
            // SQL Statement:-
            //      SELECT BridgePriority.RequiredAction, COUNT(BridgePriority.BridgeId)
            //      FROM (  SELECT BridgeId,
            //                  CASE
            //                      WHEN PriorityVal >= 0 AND PriorityVal < 15 THEN 'Routine Maintenance'
            //                      WHEN PriorityVal >= 15 AND PriorityVal < 40 THEN 'Major Rehabilitation'
            //                      WHEN PriorityVal >= 40 AND PriorityVal <= 100 THEN 'Emergency Repair'
            //                      ELSE ''
            //                  END AS RequiredAction
            //              FROM BridgePriorityByInterventionRequirement
            //              WHERE InspectionYear = 2013) AS BridgePriority
            //      GROUP BY RequiredAction 

            List<RequiredAction> requiredAction = db.RequiredActions.ToList();
            
            var bridgesbypriority = (from c in bridgeReqAction
                                    select new
                                    {
                                        bridgeid = c.bridgeid,
                                        ReqAction =
                                            (
                                                c.priority >= requiredAction[0].ValueFrom && c.priority < requiredAction[0].ValueTo ? "Regular Inspection" :
                                                c.priority >= requiredAction[1].ValueFrom && c.priority < requiredAction[1].ValueTo ? "Rehabilitation" :
                                                c.priority >= requiredAction[2].ValueFrom && c.priority <= requiredAction[2].ValueTo ? "Urgent Action / Replacement" : ""
                                            ),
                                    }).ToList();


            // Get No. of bridges vs required action ("Routine Maintenance", "Major Rehabilitation" and "Emergency Repair")
            var noofbridgesbyrequiredaction = (from c in bridgesbypriority
                                               group c by new { c.ReqAction } into d
                                               select new
                                               {
                                                   ReqAction = d.Key.ReqAction,
                                                   BridgeCount = d.Count(),
                                               }).ToList();

            ViewBag.TotalBridges = bridgeReqAction.Count();

            DataTable dt = ListToDataTable(noofbridgesbyrequiredaction);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _NoOfInspectedBridgesVsInspectionYearChart()
        {
            var noofbridgesbyinspctionyear = (from dmg in db.ResultInspMajors
                                              group dmg by new { dmg.InspectionYear } into g
                                              select new
                                              {
                                                  InspectionYear = g.Key.InspectionYear,
                                                  BridgeCount = g.Count()
                                              }).ToList();

            DataTable dt = ListToDataTable(noofbridgesbyinspctionyear.OrderBy(s => s.InspectionYear).ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _NoOfInspectedAndRegisteredBridgesVsInspectionYearChart()
        {
            var inspectedbridges = (from dmg in db.ResultInspMajors
                                        group dmg by new { dmg.InspectionYear } into g
                                        select new
                                        {
                                            InspectionYear = g.Key.InspectionYear,
                                            BridgeCount = g.Count()
                                        }).ToList();
            var registerededbridges = db.MajorInspectionYears; //.Select(s => s.NoOfRegisteredBridges);

            var noofbridges = (from insp in inspectedbridges
                                join reg in registerededbridges on insp.InspectionYear equals reg.InspectionYear into table1
                                from reg in table1.ToList()
                                select new
                                {
                                    InspectionYear = insp.InspectionYear,
                                    RegisteredBridges = reg.NoOfRegisteredBridges,
                                    InspectedBridges = insp.BridgeCount
                                }).ToList();

            DataTable dt = ListToDataTable(noofbridges.OrderBy(s => s.InspectionYear).ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _NoOfInspectedBridgesVsDistrictChart(int inspectionyear)
        {
            var brgCondByDistrict = (from cond in db.BridgeConditionAndRepairCostByDistricts
                                     join dist in db.Districts on cond.District equals dist.DistrictName
                                     where cond.InspectionYear == inspectionyear
                                     orderby dist.DistrictNo
                                     select new 
                                     { 
                                         DistrictName = cond.District, 
                                         Condition = cond.Total 
                                     }).ToList();

            ViewBag.InspectionYear = inspectionyear;

            DataTable dt = ListToDataTable(brgCondByDistrict.ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _InspectedBridgeConditionsVsDistrictChart(int inspectionyear)
        {
            var brgCondByDistrict = (from cond in db.BridgeConditionAndRepairCostByDistricts
                                     join dist in db.Districts on cond.District equals dist.DistrictName
                                     where cond.InspectionYear == inspectionyear
                                     orderby dist.DistrictNo
                                     select new 
                                     { 
                                         DistrictName = cond.District, 
                                         Good = cond.Good, 
                                         Fair = cond.Fair, 
                                         Bad = cond.Bad 
                                     }).ToList();
            
            ViewBag.InspectionYear = inspectionyear;

            DataTable dt = ListToDataTable(brgCondByDistrict.ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _InspectedBridgeRequiredActionsVsDistrictChart(int inspectionyear)
        {
            var brgReqActionByDistrict = (from action in db.BridgeRequiredActionByDistricts
                                         join dist in db.Districts on action.District equals dist.DistrictName
                                         where action.InspectionYear == inspectionyear
                                         orderby dist.DistrictNo
                                         select new 
                                         { 
                                             DistrictName = action.District,
                                             RegularInspection = action.RegularInspection,
                                             Rehabilitation = action.Rehabilitation,
                                             Replacement = action.Replacement 
                                         }).ToList();
            
            ViewBag.InspectionYear = inspectionyear;

            DataTable dt = ListToDataTable(brgReqActionByDistrict.ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _NoOfInspectedCulvertsVsDistrictChart(int inspectionyear)
        {
            var culCondByDistrict = (from cond in db.CulvertConditionAndRepairCostByDistricts
                                     join dist in db.Districts on cond.District equals dist.DistrictName
                                     where cond.InspectionYear == inspectionyear
                                     orderby dist.DistrictNo
                                     select new 
                                     { 
                                         DistrictName = cond.District, 
                                         CulvertCount = cond.Total 
                                     }).ToList();

            ViewBag.InspectionYear = inspectionyear;
            
            DataTable dt = ListToDataTable(culCondByDistrict.ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _NoOfInspectedCulvertsVsInspectionYearChart()
        {
            // To get inspection year, simply take the year part from the date
            var noofculvertsbyinspctionyear = (from dmg in db.CulvertConditionAndRepairCostByDistricts
                                              group dmg by new { InspectionYear = dmg.InspectionYear } into g
                                              select new
                                              {
                                                  InspectionYears = g.Key.InspectionYear,
                                                  CulvertCount = g.Sum(s => s.Total)
                                              }).ToList();

            //var noofculvertsbyinspctionyear = (from dmg in db.ResultInspCulverts
            //                                  group dmg by new { InspectionYear = dmg.InspectionDate.ToString().Substring(0, 4) } into g
            //                                  select new
            //                                  {
            //                                      InspectionYears = g.Key.InspectionYear,
            //                                      CulvertCount = g.Count()
            //                                  }).ToList();

            //InspYear = culDmgInsp.InspectionDate.ToString().Substring(0, 4) // take the year part from the date
            DataTable dt = ListToDataTable(noofculvertsbyinspctionyear.OrderBy(s => s.InspectionYears).ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _InspectedCulvertConditionsVsDistrictChart(int inspectionyear)
        {
            var culCondByDistrict = (from cond in db.CulvertConditionAndRepairCostByDistricts
                                     join dist in db.Districts on cond.District equals dist.DistrictName
                                     where cond.InspectionYear == inspectionyear
                                     orderby dist.DistrictNo
                                     select new
                                     {
                                         DistrictName = cond.District,
                                         Good = cond.Good,
                                         Fair = cond.Fair,
                                         Bad = cond.Bad
                                     }).ToList();

            DataTable dt = ListToDataTable(culCondByDistrict.ToList());

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _BridgeAssetValueVsDistrictsChart()
        {
            var bridgeassetvaluebydistrict = (from c in db.AssetValueByDistricts
                                              join dist in db.Districts on c.DistrictName equals dist.DistrictName
                                              orderby dist.DistrictNo
                                              select new
                                              {
                                                  DistrictName = c.DistrictName,
                                                  // Divided by billion and round to one decimal place
                                                  AssetValue = Math.Round((double)c.BridgeAssetValue / 1000000000.00, 2)
                                              }).ToList();

            DataTable dt = ListToDataTable(bridgeassetvaluebydistrict);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _BridgeAndCulvertAssetValuesVsDistrictsChart()
        {
            var bridgeandculvertassetvaluebydistrict = (from c in db.AssetValueByDistricts
                                                        join dist in db.Districts on c.DistrictName equals dist.DistrictName
                                                        orderby dist.DistrictNo
                                                        select new
                                                        {
                                                            DistrictName = c.DistrictName,
                                                            // Divided by billion and round to one decimal place
                                                            BridgeAssetValue = Math.Round((double)c.BridgeAssetValue / 1000000000.00, 1),
                                                            CulvertAssetValue = Math.Round((double)c.CulvertAssetValue / 1000000000.00, 1),
                                                            TotalAssetValue = Math.Round((double)c.TotalAssetValue / 1000000000.00, 1)
                                                        }).ToList();

            
            DataTable dt = ListToDataTable(bridgeandculvertassetvaluebydistrict);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfCulvertsVsDistrictChart()
        {
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();
            List<District> districts = db.Districts.ToList();

            var culvertRecord = from br in culverts
                                join seg in segments on br.SegmentId equals seg.SegmentId into table1
                                from seg in table1.ToList()
                                join sec in sections on seg.SectionId equals sec.SectionId into table2
                                from sec in table2.ToList()
                                join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                                from dis in table3.ToList()
                                orderby dis.DistrictNo
                                select new
                                {
                                    culverts = br,
                                    districts = dis
                                };

            var noofculvertsbydistrict = (from c in culvertRecord
                                          group c by new { c.districts } into g
                                          select new
                                          {
                                              DistrictName = g.Key.districts.DistrictName,
                                              CulvertCount = g.Count()
                                          }).ToList();

            DataTable dt = ListToDataTable(noofculvertsbydistrict);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfCulvertsVsRegionChart()
        {
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<RegionalGovernment> regions = db.RegionalGovernments.ToList();

            var culvertRecord = from br in culverts
                                join seg in segments on br.SegmentId equals seg.SegmentId into table1
                                from seg in table1.ToList()
                                join reg in regions on seg.RegionalGovernmentId equals reg.RegionalGovernmentId into table3
                                from reg in table3.ToList()
                                orderby reg.RegionalGovernmentId
                                select new
                                {
                                    culverts = br,
                                    regions = reg
                                };

            var noofculvertsbydistrict = (from c in culvertRecord
                                          group c by new { c.regions } into g
                                          select new
                                          {
                                              RegionalGovernmentName = g.Key.regions.RegionalGovernmentName,
                                              CulvertCount = g.Count()
                                          }).ToList();

            DataTable dt = ListToDataTable(noofculvertsbydistrict);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _NoOfCulvertsVsCulvertTypeChart()
        {
            List<Culvert> culverts = db.Culverts.ToList();
            List<CulvertStructure> culStructures = db.CulvertStructures.ToList();
            List<CulvertType> culvertTypes = db.CulvertTypes.ToList();

            var culvertRecord = from cul in culverts
                                join str in culStructures on cul.CulvertId equals str.CulvertId into table1
                                from sup in table1.ToList()
                                join brtyp in culvertTypes on sup.CulvertTypeId equals brtyp.CulvertTypeId into table2
                                from brtyp in table2.ToList()
                                select new
                                {
                                    culverts = cul,
                                    culverttypes = brtyp
                                };

            var noofculvertsbyculverttype = (from c in culvertRecord
                                             group c by new { c.culverttypes } into g
                                             select new
                                             {
                                                 CulvertType = g.Key.culverttypes.CulvertTypeName,
                                                 CulvertCount = g.Count()
                                             }).ToList();

            DataTable dt = ListToDataTable(noofculvertsbyculverttype);

            List<object> iData = new List<object>();

            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult _CulvertAssetValueVsDistrictsChart()
        {
            var culvertassetvaluebydistrict = (from c in db.AssetValueByDistricts
                                               join dist in db.Districts on c.DistrictName equals dist.DistrictName
                                               orderby dist.DistrictNo
                                               select new
                                               {
                                                   DistrictName = c.DistrictName,
                                                   // Divided by billion and round to one decimal place
                                                   AssetValue = Math.Round((double)c.CulvertAssetValue / 1000000000.00, 2)
                                               }).ToList();

            DataTable dt = ListToDataTable(culvertassetvaluebydistrict);

            List<object> iData = new List<object>();
            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);
        }
    }
}