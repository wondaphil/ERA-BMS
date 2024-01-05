using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class CulvertReportsCensusController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: CulvertReportsCensus
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _GetAllCulvertsByDistrict(string districtid)
        {
            //var CulvertList = db.Culverts.Include(b => b.Segment).Include(b => b.SubRoute);
            List<CSSDViewModel> CulvertList = (from cul in db.Culverts
                                              join seg in db.Segments on cul.SegmentId equals seg.SegmentId into table1
                                              from seg in table1.ToList()
                                              join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                                              from sec in table2.ToList()
                                              join dis in db.Districts on sec.DistrictId equals dis.DistrictId into table3
                                              from dis in table3.ToList()
                                              where dis.DistrictId == districtid
                                               select new CSSDViewModel
                                              {
                                                  culverts = cul,
                                                  segments = seg,
                                                  sections = sec,
                                                  districts = dis
                                              }).ToList();

            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName;

            return PartialView(CulvertList);
        }

        public ActionResult AllCulvertsByDistrict()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public ActionResult CulvertsByDistrict()
        {
            List<Culvert> culverts = db.Culverts.ToList();
            List<Segment> segments = db.Segments.ToList();
            List<Section> sections = db.Sections.ToList();
            List<District> districts = db.Districts.ToList();

            var culvertRecord = from cul in culverts
                                join seg in segments on cul.SegmentId equals seg.SegmentId into table1
                                from seg in table1.ToList()
                                join sec in sections on seg.SectionId equals sec.SectionId into table2
                                from sec in table2.ToList()
                                join dis in districts on sec.DistrictId equals dis.DistrictId into table3
                                from dis in table3.ToList()
                                select new CSSDViewModel
                                {
                                    culverts = cul,
                                    segments = seg,
                                    sections = sec,
                                    districts = dis
                                };

            return View(culvertRecord);
        }

        public ActionResult CulvertsCountByCulvertType()
        {
            List<Culvert> culverts = db.Culverts.ToList();
            List<CulvertStructure> culStructures = db.CulvertStructures.ToList();
            List<CulvertType> culvertTypes = db.CulvertTypes.ToList();

            var culvertRecord = from cul in culverts
                               join str in culStructures on cul.CulvertId equals str.CulvertId into table1
                               from str in table1.ToList()
                               join brtyp in culvertTypes on str.CulvertTypeId equals brtyp.CulvertTypeId into table2
                               from brtyp in table2.ToList()
                               select new
                               {
                                   culverts = cul,
                                   culverttypes = brtyp
                               };

            var noofculvertsbyculverttype = (from c in culvertRecord
                                           group c by new { c.culverttypes } into g
                                           select new CulvertTypeViewModel
                                           {
                                               CulvertTypes = g.Key.culverttypes,
                                               CulvertCount = g.Count()
                                           }).ToList();

            return PartialView(noofculvertsbyculverttype);
        }

        public ActionResult _GetCulvertTypeByDistrict()
        {
            List<CulvertTypeByDistrict> culvertTypeByDistrict = db.CulvertTypeByDistricts.OrderBy(c => c.DistrictId).ToList();

            return PartialView(culvertTypeByDistrict);
        }
        
        public ActionResult _GetCulvertTypeByRegion()
        {
            List<CulvertTypeByRegion> culvertTypeByRegion = db.CulvertTypeByRegions.OrderBy(c => c.RegionalGovernmentId).ToList();

            return PartialView(culvertTypeByRegion);
        }
        
        public ActionResult _GetCulvertTypeBySection(string districtid)
        {
            List<CulvertTypeBySection> culvertTypeBySection = db.CulvertTypeBySections.Where(s => s.DistrictId == districtid).ToList();
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(culvertTypeBySection);
        }

        public ActionResult _GetCulvertTypeBySegmentInSection(string sectionid)
        {
            List<CulvertTypeBySegment> culvertTypeBySegment = db.CulvertTypeBySegments.Where(s => s.SectionId == sectionid).ToList();
            ViewBag.SectionName = db.Sections.Find(sectionid).SectionName.ToString();
            ViewBag.DistrictName = db.Sections.Find(sectionid).District.DistrictName.ToString();

            return PartialView(culvertTypeBySegment);
        }

        public ActionResult _GetCulvertTypeBySegmentInDistrict(string districtid)
        {
            List<CulvertTypeBySegment> culvertTypeBySegment = db.CulvertTypeBySegments.Where(s => s.DistrictId == districtid).ToList();
            ViewBag.DistrictName = db.Districts.Find(districtid).DistrictName.ToString();

            return PartialView(culvertTypeBySegment);
        }

        public ActionResult CulvertTypeCensusByLocation()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            List<SelectListItem> reportTypeList = new List<SelectListItem>();

            //Add items
            reportTypeList.Insert(0, new SelectListItem { Text = "Culvert Type by Road Segment", Value = "1" });
            reportTypeList.Insert(1, new SelectListItem { Text = "Culvert Type by Section", Value = "2" });
            reportTypeList.Insert(2, new SelectListItem { Text = "Culvert Type by District", Value = "3" });
            reportTypeList.Insert(3, new SelectListItem { Text = "Culvert Type by Regional Gov't", Value = "4" });
            
            ViewBag.ReportTypes = reportTypeList;

            return View();
        }
    }
}