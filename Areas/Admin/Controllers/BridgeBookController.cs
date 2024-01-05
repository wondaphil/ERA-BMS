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

namespace ERA_BMS.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BridgeBookController : Controller
    {
        private BMSEntities db = new BMSEntities();

        // GET: BridgeBook
        public ActionResult Index()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public ActionResult AnnualBridgeBook()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public ActionResult _GetBridgeBookByBridge([Optional] string bridgeid /* drop down value */)
        {
            var bridge = db.Bridges.Find(bridgeid);

            BridgeImageListViewModel brImg = new BridgeImageListViewModel();

            List<BridgeMedia> imageList = (from s in db.BridgeMedias
                                           where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 //MediaTypeId = 1 for image
                                           select s).ToList();
            BridgeMedia firstImage = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // for side view
            BridgeMedia secondImage = imageList.Where(i => i.ImageNo == 2).FirstOrDefault(); // for front view
            brImg.Bridge = bridge;
            brImg.ImagesList = new List<BridgeMedia>();
            brImg.ImagesList.Add(firstImage);
            brImg.ImagesList.Add(secondImage);

            return PartialView(brImg);
        }

        public ActionResult BridgeBookByBridge()
        {
            ViewBag.Districts = LocationModel.GetDistrictList();

            return View();
        }

        public ActionResult _GetBridgeBookBySegment(string segmentid /* drop down value */)
        {
            var bridgeList = (from br in db.Bridges
                              where br.SegmentId == segmentid
                              select br).ToList().OrderBy(s => s.BridgeNo);

            //List<List<BridgeMedia>> bridgeImagesList = new List<List<BridgeMedia>>();
            List<BridgeImageListViewModel> bridgeImagesList = new List<BridgeImageListViewModel>();

            foreach (var bridge in bridgeList)
            {
                // Get list of images for the given bridge
                var imageList = (from s in db.BridgeMedias
                                 where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 //MediaTypeId = 1 for image
                                 select s).ToList();
                BridgeMedia firstImage = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // for side view
                BridgeMedia secondImage = imageList.Where(i => i.ImageNo == 2).FirstOrDefault(); // for front view

                BridgeImageListViewModel brImg = new BridgeImageListViewModel();
                brImg.Bridge = bridge;
                brImg.ImagesList = new List<BridgeMedia>();
                brImg.ImagesList.Add(firstImage);
                brImg.ImagesList.Add(secondImage);
                bridgeImagesList.Add(brImg);
            }

            return PartialView(bridgeImagesList);
        }

        public ActionResult _GetBridgeBookBySection(string sectionid /* drop down value */)
        {
            var bridgeList = (from br in db.Bridges
                              join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              where seg.SectionId == sectionid
                              select br).ToList().OrderBy(s => s.Segment.SegmentId).OrderBy(s => s.BridgeNo);

            List<BridgeImageListViewModel> bridgeImagesList = new List<BridgeImageListViewModel>();

            foreach (var bridge in bridgeList)
            {
                // Get list of images for the given bridge
                var imageList = (from s in db.BridgeMedias
                                 where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 //MediaTypeId = 1 for image
                                 select s).ToList();
                BridgeMedia firstImage = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // for side view
                BridgeMedia secondImage = imageList.Where(i => i.ImageNo == 2).FirstOrDefault(); // for front view
                BridgeImageListViewModel brImg = new BridgeImageListViewModel();
                brImg.Bridge = bridge;
                brImg.ImagesList = new List<BridgeMedia>();
                brImg.ImagesList.Add(firstImage);
                brImg.ImagesList.Add(secondImage);
                bridgeImagesList.Add(brImg);
            }

            return PartialView(bridgeImagesList);
        }

        public ActionResult _GetBridgeBookByDistrict(string districtid /* drop down value */)
        {
            var bridgeList = (from br in db.Bridges
                              join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                              from sec in table2.ToList()
                              where sec.DistrictId == districtid
                              select br).ToList().OrderBy(s => s.Segment.SectionId).OrderBy(s => s.Segment.SegmentId).OrderBy(s => s.BridgeNo);

            //List<List<BridgeMedia>> bridgeImagesList = new List<List<BridgeMedia>>();
            List<BridgeImageListViewModel> bridgeImagesList = new List<BridgeImageListViewModel>();

            foreach (var bridge in bridgeList)
            {
                // Get list of images for the given bridge
                var imageList = (from s in db.BridgeMedias
                                 where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 //MediaTypeId = 1 for image
                                 select s).ToList();
                BridgeMedia firstImage = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // for side view
                BridgeMedia secondImage = imageList.Where(i => i.ImageNo == 2).FirstOrDefault(); // for front view
                BridgeImageListViewModel brImg = new BridgeImageListViewModel();
                brImg.Bridge = bridge;
                brImg.ImagesList = new List<BridgeMedia>();
                brImg.ImagesList.Add(firstImage);
                brImg.ImagesList.Add(secondImage);
                bridgeImagesList.Add(brImg);
            }

            return PartialView(bridgeImagesList);
        }

        public ActionResult _GetAnnualBridgeBookBySection(string sectionid /* drop down value */)
        {
            var bridgeList = (from br in db.Bridges
                              join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              where seg.SectionId == sectionid /*&& seg.SegmentId == "00C0D58D-E4EA-41A7-AFB9-7286B44B63EE"*/
                              select br).ToList().OrderBy(s => s.Segment.SegmentId).OrderBy(s => s.BridgeNo);

            List<BridgeImageListViewModel> bridgeImagesList = new List<BridgeImageListViewModel>();

            foreach (var bridge in bridgeList)
            {
                // Get list of images for the given bridge
                var imageList = (from s in db.BridgeMedias
                                 where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 //MediaTypeId = 1 for image
                                 select s).ToList();
                BridgeMedia firstImage = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // for side view
                BridgeMedia secondImage = imageList.Where(i => i.ImageNo == 2).FirstOrDefault(); // for front view
                BridgeImageListViewModel brImg = new BridgeImageListViewModel();
                brImg.Bridge = bridge;
                brImg.ImagesList = new List<BridgeMedia>();
                brImg.ImagesList.Add(firstImage);
                brImg.ImagesList.Add(secondImage);
                bridgeImagesList.Add(brImg);
            }

            return PartialView(bridgeImagesList);
        }

        public ActionResult _GetAnnualBridgeBookByDistrict(string districtid /* drop down value */)
        {
            var bridgeList = (from br in db.Bridges
                              join seg in db.Segments on br.SegmentId equals seg.SegmentId into table1
                              from seg in table1.ToList()
                              join sec in db.Sections on seg.SectionId equals sec.SectionId into table2
                              from sec in table2.ToList()
                              where sec.DistrictId == districtid
                              select br).ToList().OrderBy(s => s.Segment.SegmentId).OrderBy(s => s.Segment.SegmentId).OrderBy(s => s.BridgeNo);

            //List<List<BridgeMedia>> bridgeImagesList = new List<List<BridgeMedia>>();
            List<BridgeImageListViewModel> bridgeImagesList = new List<BridgeImageListViewModel>();

            foreach (var bridge in bridgeList)
            {
                // Get list of images for the given bridge
                var imageList = (from s in db.BridgeMedias
                                 where s.BridgeId == bridge.BridgeId && s.MediaTypeId == 1 //MediaTypeId = 1 for image
                                 select s).ToList();
                BridgeMedia firstImage = imageList.Where(i => i.ImageNo == 1).FirstOrDefault(); // for side view
                BridgeMedia secondImage = imageList.Where(i => i.ImageNo == 2).FirstOrDefault(); // for front view
                BridgeImageListViewModel brImg = new BridgeImageListViewModel();
                brImg.Bridge = bridge;
                brImg.ImagesList = new List<BridgeMedia>();
                brImg.ImagesList.Add(firstImage);
                brImg.ImagesList.Add(secondImage);
                bridgeImagesList.Add(brImg);
            }

            return PartialView(bridgeImagesList);
        }
    }
}