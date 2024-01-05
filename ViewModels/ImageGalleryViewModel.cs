using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.ViewModels
{
    public class ImageGalleryViewModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string ImageDescription { get; set; }
        public int ImageTypeId { get; set; }
    }

    public class ImageGalleryModel
    {
        public ImageGalleryViewModel SelectedImage;
        public ImageGalleryModel()
        {

        }

        public List<ImageGalleryViewModel> GetBridgeAllImageList(string bridgeid)
        {
            BMSEntities db = new BMSEntities();
            List<ImageGalleryViewModel> bridgeImagesList = new List<ImageGalleryViewModel>();

            // MediaTypeId = 1 for bridge image, MediaTypeId = 6 for bridge damage image
            List<BridgeMedia> brgImg = (from br in db.BridgeMedias
                                        where br.BridgeId == bridgeid && (br.MediaTypeId == 1 || br.MediaTypeId == 6) 
                                        select br).OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList();
                                        // Sort by media type and then by description

            foreach (var img in brgImg)
            {
                bridgeImagesList.Add(new ImageGalleryViewModel
                {
                    Id = brgImg.IndexOf(img),
                    ImagePath = img.MediaFilePath,
                    ImageDescription = img.Description,
                    ImageTypeId = (int) img.MediaTypeId
                });
            }

            return bridgeImagesList;
        }

        public List<ImageGalleryViewModel> GetBridgeImageList(string bridgeid)
        {
            BMSEntities db = new BMSEntities();
            List<ImageGalleryViewModel> bridgeImagesList = new List<ImageGalleryViewModel>();
            List<BridgeMedia> brgImg = (from br in db.BridgeMedias
                                        where br.BridgeId == bridgeid && br.MediaTypeId == 1 //MediaTypeId is 1 for bridge image
                                        select br).OrderBy(d => d.Description).ToList();
                                        // Sort by media description

            foreach (var img in brgImg)
            {
                bridgeImagesList.Add(new ImageGalleryViewModel
                {
                    Id = brgImg.IndexOf(img),
                    ImagePath = img.MediaFilePath,
                    ImageDescription = img.Description,
                    ImageTypeId = (int)img.MediaTypeId
                });
            }

            return bridgeImagesList;
        }

        public List<ImageGalleryViewModel> GetBridgeDamageImageList(string bridgeid)
        {
            BMSEntities db = new BMSEntities();
            List<ImageGalleryViewModel> bridgeDamageImagesList = new List<ImageGalleryViewModel>();
            List<BridgeMedia> brgImg = (from br in db.BridgeMedias
                                        where br.BridgeId == bridgeid && br.MediaTypeId == 6 //MediaTypeId is 6 for bridge damage image
                                        select br).OrderBy(d => d.Description).ToList();
                                        // Sort by media description

            foreach (var img in brgImg)
            {
                bridgeDamageImagesList.Add(new ImageGalleryViewModel
                {
                    Id = brgImg.IndexOf(img),
                    ImagePath = img.MediaFilePath,
                    ImageDescription = img.Description,
                    ImageTypeId = (int)img.MediaTypeId
                });
            }

            return bridgeDamageImagesList;
        }

        public List<ImageGalleryViewModel> GetCulvertAllImageList(string culvertid)
        {
            BMSEntities db = new BMSEntities();
            List<ImageGalleryViewModel> culvertImagesList = new List<ImageGalleryViewModel>();

            // MediaTypeId = 1 for culvert image, MediaTypeId = 6 for culvert damage image
            List<CulvertMedia> culImg = (from cul in db.CulvertMedias
                                         where cul.CulvertId == culvertid && (cul.MediaTypeId == 1 || cul.MediaTypeId == 6) 
                                         select cul).OrderBy(d => d.MediaTypeId).ThenBy(d => d.Description).ToList();
                                         // Sort by media type and then by description

            foreach (var img in culImg)
            {
                culvertImagesList.Add(new ImageGalleryViewModel
                {
                    Id = culImg.IndexOf(img),
                    ImagePath = img.MediaFilePath,
                    ImageDescription = img.Description,
                    ImageTypeId = (int)img.MediaTypeId
                });
            }

            return culvertImagesList;
        }

        public List<ImageGalleryViewModel> GetCulvertImageList(string culvertid)
        {
            BMSEntities db = new BMSEntities();
            List<ImageGalleryViewModel> culvertImagesList = new List<ImageGalleryViewModel>();

            // MediaTypeId = 1 for culvert image
            List<CulvertMedia> culImg = (from cul in db.CulvertMedias
                                         where cul.CulvertId == culvertid && (cul.MediaTypeId == 1) 
                                         select cul).OrderBy(d => d.Description).ToList();
                                        // Sort by media description

            foreach (var img in culImg)
            {
                culvertImagesList.Add(new ImageGalleryViewModel
                {
                    Id = culImg.IndexOf(img),
                    ImagePath = img.MediaFilePath,
                    ImageDescription = img.Description,
                    ImageTypeId = (int)img.MediaTypeId
                });
            }

            return culvertImagesList;
        }
    }
}