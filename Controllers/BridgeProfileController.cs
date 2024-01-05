using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;
using System.Runtime.InteropServices;

using System.Data.Entity;
using System.Net;

namespace ERA_BMS.Controllers
{
    public class BridgeProfileController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: BridgeProfile
        public ActionResult Index(string id = "")
        {
            Bridge bridge = db.Bridges.Find(id);
            BridgeGeneralInfo genInfo = db.BridgeGeneralInfoes.Find(id);
            SuperStructure superStr = db.SuperStructures.Find(id);
            Abutment abutment = db.Abutments.Find(id);
            Ancillary ancillaries = db.Ancillaries.Find(id);
            List<Pier> piers = db.Piers.Where(p => p.BridgeId == id).ToList();
            List<BridgeDoc> bridgeDoc = db.BridgeDocs.Where(d => d.BridgeId == id).ToList(); 
            List<BridgeMedia> bridgeImage = db.BridgeMedias.Where(m => m.BridgeId == id && m.MediaTypeId == 1).OrderBy(d => d.Description).ToList(); // Sort by media description
            List<BridgeMedia> bridgeVideo = db.BridgeMedias.Where(m => m.BridgeId == id && (m.MediaTypeId == 2 || m.MediaTypeId == 7)).ToList(); // bridge video and damage video
            List<BridgeMedia> bridgeDrawing = db.BridgeMedias.Where(m => m.BridgeId == id && m.MediaTypeId == 3).ToList();
            List<BridgeMedia> bridgeDmgImage = db.BridgeMedias.Where(m => m.BridgeId == id && m.MediaTypeId == 6).OrderBy(d => d.Description).ToList(); // Sort by media description

            BridgeViewModel profile = new BridgeViewModel()
            {
                Bridge = bridge,
                GenInfo = genInfo,
                SuperStr = superStr,
                Abutment = abutment,
                Piers = piers,
                Ancillaries = ancillaries,
                BridgeDoc = bridgeDoc,
                BridgeImage = bridgeImage,
                BridgeVideo = bridgeVideo,
                BridgeDrawing = bridgeDrawing,
                BridgeDamageImage = bridgeDmgImage
            };

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == "" || bridge == null)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Bridges = new List<SelectListItem>(); // For an empty bridges dropdownlist

                return View();
            }
            else
            {
                // For a dropdownlist that has a list of bridges in the current segment and the current bridge selected
                ViewBag.Bridges = LocationModel.GetBridgeNameList(bridge.SegmentId);
                ViewBag.BridgeId = id;

                // For a dropdownlist that has a list of segments in the current section and the current segment selected
                ViewBag.Segments = LocationModel.GetSegmentList(bridge.Segment.SectionId);
                ViewBag.SegmentId = bridge.Segment.SegmentId;

                // For a dropdownlist that has a list of sections in the current district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(bridge.Segment.Section.DistrictId);
                ViewBag.SectionId = bridge.Segment.SectionId;

                // For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.Districts = LocationModel.GetDistrictList();
                ViewBag.DistrictId = bridge.Segment.Section.DistrictId;

                // For displaying district, section, and segment names and bridge no on reports
                ViewBag.DistrictName = bridge.Segment.Section.District.DistrictName;
                ViewBag.SectionName = bridge.Segment.Section.SectionName;
                ViewBag.SegmentName = bridge.Segment.SegmentName;
                TempData["BridgeName"] = $"{bridge.BridgeNo} - {bridge.BridgeName}";

                return View(profile);
            }
        }

        public ActionResult PrintProfile([Optional] string bridgeid /* drop down value */)
        {
            return PartialView(db.Bridges.Find(bridgeid));
        }

        //GET
        public ActionResult Location([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Bridge bridge = db.Bridges.Find(bridgeid);
            
            if (bridge == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            Segment segment = db.Segments.Find(bridge.SegmentId);

            //Get district name that encompasses the current bridge
            ViewBag.DistrictName = db.Sections.Find(segment.SectionId).District.DistrictName;

            ViewBag.SectionName = segment.Section.SectionName;

            return PartialView(bridge);
        }

        //GET
        public ActionResult GeneralInfo([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            BridgeGeneralInfo bridgeGenInfo = db.BridgeGeneralInfoes.Find(bridgeid);
            
            if (bridgeGenInfo == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            return PartialView(bridgeGenInfo);
        }

        //GET
        public ActionResult SuperStructure([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SuperStructure superStructure = db.SuperStructures.Find(bridgeid);
            
            if (superStructure == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            return PartialView(superStructure);
        }

        //GET
        public ActionResult Abutment([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Abutment abutment = db.Abutments.Find(bridgeid);
            
            if (abutment == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            return PartialView(abutment);
        }


        //GET
        public ActionResult Pier([Optional] string bridgeid /* drop down value */)
        {
            List<Pier> pierList = (from s in db.Piers
                                    where s.BridgeId == bridgeid
                                    select s).ToList();
            
            return PartialView(pierList);
        }

        //GET
        public ActionResult Ancillary([Optional] string bridgeid /* drop down value */)
        {
            if (bridgeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ancillary ancillary = db.Ancillaries.Find(bridgeid);
            
            if (ancillary == null)
            {
                //return HttpNotFound();
                return PartialView("NoData");
            }

            return PartialView(ancillary);
        }


        //GET
        public ActionResult BridgeDoc([Optional] string bridgeid /* drop down value */)
        {
            List<BridgeDoc> bridgeDoc = db.BridgeDocs.Where(s => s.BridgeId == bridgeid).ToList();

            return PartialView(bridgeDoc);
        }

        //GET
        public ActionResult BridgeImage([Optional] string bridgeid)
        {
            int mediatypeid = 1; //MediaTypeId is 1 for image

            List<BridgeMedia> bridgeImageList = (from s in db.BridgeMedias
                                                 where s.BridgeId == bridgeid && s.MediaTypeId == mediatypeid
                                                 select s).OrderBy(d => d.Description).ToList();
                                                // Sort by media description

            return PartialView(bridgeImageList);
        }

        //GET
        public ActionResult BridgeVideo([Optional] string bridgeid /* drop down value */)
        {
            int mediatypeid1 = 2; //MediaTypeId is 2 for video
            int mediatypeid2 = 7; //MediaTypeId is 7 for damage video
            
            List<BridgeMedia> bridgeVideoList = (from s in db.BridgeMedias
                                                 where s.BridgeId == bridgeid && (s.MediaTypeId == mediatypeid1 || s.MediaTypeId == mediatypeid2)
                                                 select s).ToList();

            return PartialView(bridgeVideoList);
        }

        //GET
        public ActionResult BridgeDrawing([Optional] string bridgeid /* drop down value */)
        {
            int mediatypeid = 3; //MediaTypeId is 3 for drawing
            
            List<BridgeMedia> bridgeDrawingList = (from s in db.BridgeMedias
                                                   where s.BridgeId == bridgeid && s.MediaTypeId == mediatypeid
                                                   select s).ToList();

            return PartialView(bridgeDrawingList);
        }

        //GET
        [Authorize(Roles = "Admin")]
        public ActionResult BridgeDamageImage([Optional] string bridgeid /* drop down value */)
        {
            int mediatypeid = 6; //MediaTypeId is 6 for damage picture

            List<BridgeMedia> bridgeDrawingList = (from s in db.BridgeMedias
                                                   where s.BridgeId == bridgeid && s.MediaTypeId == mediatypeid
                                                   select s).OrderBy(d => d.Description).ToList();
                                                    // Sort by media description

            return PartialView(bridgeDrawingList);
        }
    }
}