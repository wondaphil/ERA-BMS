using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.Controllers
{
    public class ImageGalleryController : Controller
    {
        BMSEntities db = new BMSEntities();
        
       // Lists all images (bridge image and damage images)
        public ActionResult BridgeAllImages(string id, int? val) // id = BridgeId, val = SelectedImageId
        {
            if (id == "" || id == null)
            {
                return View();
            }
            else
            {
                // if selected imaege id is not provided, just start from the first item (0)
                if (val == null)
                    val = 0;

                ImageGalleryModel images = new ImageGalleryModel();

                TempData["SelectedId"] = val;
                TempData["BridgeId"] = id;
                
                var bridge = db.Bridges.Find(id);
                TempData["BridgeNo"] = bridge.BridgeNo;
                TempData["BridgeName"] = bridge.BridgeName;
                var imgList = images.GetBridgeAllImageList(id);
                if (imgList.Count() > 0)
                    images.SelectedImage = images.GetBridgeAllImageList(id)[(int)val];

                return View(images);
            }
        }

        // Lists bridge images only (without damage images)
        public ActionResult BridgeImages(string id, int? val) // id = bridge id, val = selected image id
        {
            if (id == "" || id == null)
            {
                return View();
            }
            else
            {
                // if selected imaege id is not provided, just start from the first item (0)
                if (val == null)
                    val = 0;
                
                ImageGalleryModel images = new ImageGalleryModel();

                TempData["SelectedId"] = val;
                TempData["BridgeId"] = id;
                
                var bridge = db.Bridges.Find(id);
                TempData["BridgeNo"] = bridge.BridgeNo;
                TempData["BridgeName"] = bridge.BridgeName;
                var imgList = images.GetBridgeImageList(id);
                if (imgList.Count() > 0)
                    images.SelectedImage = images.GetBridgeImageList(id)[(int)val];

                return View(images);
            }
        }

        // Lists damage images only (without bridge images)
        public ActionResult BridgeDamageImages(string id, int? val) //bridgeid
        {
            if (id == "" || id == null)
            {
                return View();
            }
            else
            {
                // if selected imaege id is not provided, just start from the first item (0)
                if (val == null)
                    val = 0;

                ImageGalleryModel images = new ImageGalleryModel();

                TempData["SelectedId"] = val;
                TempData["BridgeId"] = id;
                
                var bridge = db.Bridges.Find(id);
                TempData["BridgeNo"] = bridge.BridgeNo;
                TempData["BridgeName"] = bridge.BridgeName;
                
                var imgList = images.GetBridgeDamageImageList(id);
                if (imgList.Count() > 0)
                    images.SelectedImage = imgList[(int)val];

                return View(images);
            }
        }

        public ActionResult CulvertAllImages(string id, int? val) //culvertid
        {
            if (id == "" || id == null)
            {
                return View();
            }
            else
            {
                // if selected imaege id is not provided, just start from the first item (0)
                if (val == null)
                    val = 0;

                ImageGalleryModel images = new ImageGalleryModel();

                TempData["SelectedId"] = val;
                TempData["CulvertId"] = id;

                var culvert = db.Culverts.Find(id);
                TempData["CulvertNo"] = culvert.CulvertNo;
                
                var imgList = images.GetCulvertAllImageList(id);
                if (imgList.Count() > 0)
                    images.SelectedImage = imgList[(int)val];

                return View(images);
            }
        }

        public ActionResult CulvertImages(string id, int? val) //culvertid
        {
            if (id == "" || id == null)
            {
                return View();
            }
            else
            {
                // if selected imaege id is not provided, just start from the first item (0)
                if (val == null)
                    val = 0;

                ImageGalleryModel images = new ImageGalleryModel();

                TempData["SelectedId"] = val;
                TempData["CulvertId"] = id;

                var culvert = db.Culverts.Find(id);
                TempData["CulvertNo"] = culvert.CulvertNo;
                
                var imgList = images.GetCulvertAllImageList(id);
                if (imgList.Count() > 0)
                    images.SelectedImage = imgList[(int)val];

                return View(images);
            }
        }

        [HttpPost]
        public ActionResult GetNextOrPrevBridgeAllImage(string buttontype, string bridgeid)
        {
            ImageGalleryModel images = new ImageGalleryModel();
            List<ImageGalleryViewModel> brgImgList = images.GetBridgeAllImageList(bridgeid);

            int id = System.Convert.ToInt32(TempData["SelectedId"]);
            
            if (buttontype.Trim().ToLower() == "next")
                images.SelectedImage = brgImgList[++id < brgImgList.Count ? id : --id];
            else if (buttontype.Trim().ToLower() == "prev")
                images.SelectedImage = brgImgList[--id > -1 ? id : ++id];

            TempData["SelectedId"] = id;

            return PartialView("_ImagePartial", images);
        }

        [HttpPost]
        public ActionResult GetNextOrPrevBridgeImage(string buttontype, string bridgeid)
        {
            ImageGalleryModel images = new ImageGalleryModel();
            List<ImageGalleryViewModel> brgImgList = images.GetBridgeImageList(bridgeid);

            int id = System.Convert.ToInt32(TempData["SelectedId"]);
            
            if (buttontype.Trim().ToLower() == "next")
                images.SelectedImage = brgImgList[++id < brgImgList.Count ? id : --id];
            else if (buttontype.Trim().ToLower() == "prev")
                images.SelectedImage = brgImgList[--id > -1 ? id : ++id];

            TempData["SelectedId"] = id;

            return PartialView("_ImagePartial", images);
        }

        [HttpPost]
        public ActionResult GetNextOrPrevBridgeDamageImage(string buttontype, string bridgeid)
        {
            ImageGalleryModel images = new ImageGalleryModel();
            List<ImageGalleryViewModel> brgDmgImgList = images.GetBridgeDamageImageList(bridgeid);

            int id = System.Convert.ToInt32(TempData["SelectedId"]);

            if (buttontype.Trim().ToLower() == "next")
                images.SelectedImage = brgDmgImgList[++id < brgDmgImgList.Count ? id : --id];
            else if (buttontype.Trim().ToLower() == "prev")
                images.SelectedImage = brgDmgImgList[--id > -1 ? id : ++id];

            TempData["SelectedId"] = id;

            return PartialView("_ImagePartial", images);
        }

        [HttpPost]
        public ActionResult GetNextOrPrevCulvertAllImage(string buttontype, string culvertid)
        {
            ImageGalleryModel images = new ImageGalleryModel();
            List<ImageGalleryViewModel> culImgList = images.GetCulvertAllImageList(culvertid);

            int id = System.Convert.ToInt32(TempData["SelectedId"]);

            if (buttontype.Trim().ToLower() == "next")
                images.SelectedImage = culImgList[++id < culImgList.Count ? id : --id];
            else if (buttontype.Trim().ToLower() == "prev")
                images.SelectedImage = culImgList[--id > -1 ? id : ++id];

            TempData["SelectedId"] = id;

            return PartialView("_ImagePartial", images);
        }

        [HttpPost]
        public ActionResult GetNextOrPrevCulvertImage(string buttontype, string culvertid)
        {
            ImageGalleryModel images = new ImageGalleryModel();
            List<ImageGalleryViewModel> culImgList = images.GetCulvertImageList(culvertid);

            int id = System.Convert.ToInt32(TempData["SelectedId"]);

            if (buttontype.Trim().ToLower() == "next")
                images.SelectedImage = culImgList[++id < culImgList.Count ? id : --id];
            else if (buttontype.Trim().ToLower() == "prev")
                images.SelectedImage = culImgList[--id > -1 ? id : ++id];

            TempData["SelectedId"] = id;

            return PartialView("_ImagePartial", images);
        }
    }
}