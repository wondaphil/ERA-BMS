using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class CulvertProfileController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: CulvertProfile
        public ActionResult Index(string id = "")
        {
            Culvert culvert = db.Culverts.Find(id);
            CulvertGeneralInfo genInfo = db.CulvertGeneralInfoes.Find(id);
            CulvertStructure culStr = db.CulvertStructures.Find(id);
            List<CulvertMedia> culvertImage = db.CulvertMedias.Where(m => m.CulvertId == id && (m.MediaTypeId == 1 || m.MediaTypeId == 6 ))
                                                    .OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList(); // Sort by media type and then by description

            CulvertViewModel profile = new CulvertViewModel()
            {
                Culvert = culvert,
                GenInfo = genInfo,
                CulvertStr = culStr,
                CulvertImage = culvertImage
            };

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == "" || culvert == null)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Culverts = new List<SelectListItem>(); // For an empty culverts dropdownlist

                return View();
            }
            else
            {
                // For a dropdownlist that has a list of culverts in the current segment and the current culvert selected
                ViewBag.Culverts = LocationModel.GetCulvertNameList(culvert.SegmentId);
                ViewBag.CulvertId = id;

                // For a dropdownlist that has a list of segments in the current section and the current segment selected
                ViewBag.Segments = LocationModel.GetSegmentList(culvert.Segment.SectionId);
                ViewBag.SegmentId = culvert.Segment.SegmentId;

                // For a dropdownlist that has a list of sections in the current district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(culvert.Segment.Section.DistrictId);
                ViewBag.SectionId = culvert.Segment.SectionId;

                // For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.Districts = LocationModel.GetDistrictList();
                ViewBag.DistrictId = culvert.Segment.Section.DistrictId;

                // For displaying district, section, and segment names and culvert no on reports
                ViewBag.DistrictName = culvert.Segment.Section.District.DistrictName;
                ViewBag.SectionName = culvert.Segment.Section.SectionName;
                ViewBag.SegmentName = culvert.Segment.SegmentName;
                TempData["CulvertNo"] = culvert.CulvertNo;

                return View(profile);
            }
        }

        public ActionResult PrintProfile([Optional] string culvertid /* drop down value */)
        {
            return PartialView(db.Culverts.Find(culvertid));
        }

        public ActionResult Location([Optional] string culvertid /* drop down value */)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Culvert culvert = db.Culverts.Find(culvertid);
            
            if (culvert == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            Segment segment = db.Segments.Find(culvert.SegmentId);

            //Get district name that encompasses the current culvert
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;

            ViewBag.SectionName = segment.Section.SectionName;

            return PartialView(culvert);
        }

        //GET
        public ActionResult GeneralInfo([Optional] string culvertid /* drop down value */)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            CulvertGeneralInfo culvertGenInfo = db.CulvertGeneralInfoes.Find(culvertid);
            
            if (culvertGenInfo == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            return PartialView(culvertGenInfo);
        }

        //GET
        public ActionResult StructuralInfo([Optional] string culvertid /* drop down value */)
        {
            if (culvertid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CulvertStructure culvertStructure = db.CulvertStructures.Find(culvertid);
            
            if (culvertStructure == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            return PartialView(culvertStructure);
        }

        //GET
        public ActionResult CulvertImage([Optional] string culvertid)
        {
            // MediaTypeId = 1 for culvert image, MediaTypeId = 6 for culvert damage image

            List<CulvertMedia> culvertImageList = (from s in db.CulvertMedias
                                                   where s.CulvertId == culvertid && (s.MediaTypeId == 1 || s.MediaTypeId == 6)
                                                   select s).OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList();
                                                    // Sort by media description

            return PartialView(culvertImageList);
        }

        //GET
        public ActionResult CulvertVideo([Optional] string culvertid)
        {
            int mediatypeid = 2; //MediaTypeId is 2 for video
            
            List<CulvertMedia> culvertImageList = (from s in db.CulvertMedias
                                                   where s.CulvertId == culvertid && s.MediaTypeId == mediatypeid
                                                   select s).OrderBy(d => d.Description).ToList();
                                                    // Sort by media description

            return PartialView(culvertImageList);
        }
    }
}