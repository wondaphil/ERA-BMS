using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_ROAD_MAPPING.Controllers
{
    public class KmlName
    {
        public string FileName { get; set; }
        public string SegmentName { get; set; }
    }

    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class KmlController : Controller
    {
        private BMSEntities db = new BMSEntities();
        private double similarityPercentage = 0.65;
        private string SegmentDirectory;
        private string SectionDirectory;
        private List<string> SegmentKmlFilesList = new List<string>();
        private List<string> SectionKmlFilesList = new List<string>();

        public KmlController()
        {
            SegmentDirectory = @"D:\ERA Road Mapping\Roads";
            SegmentKmlFilesList = GetKmlFileNames(SegmentDirectory);

            SectionDirectory = @"D:\ERA Road Mapping\Sections";
            SectionKmlFilesList = GetKmlFileNames(SectionDirectory);
        }

        // GET: Kml
        public ActionResult Index(string id = "")
        {
            Segment segment = db.Segments.Find(id);

            ViewBag.Districts = LocationModel.GetDistrictList();

            if (id == "" || segment == null)
            {
                ViewBag.Sections = new List<SelectListItem>(); // For an empty sections dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist
                ViewBag.Segments = new List<SelectListItem>(); // For an empty segments dropdownlist

                return View();
            }
            else
            {
                // For a dropdownlist that has a list of segments in the current section and the current segment selected
                ViewBag.Segments = LocationModel.GetSegmentList(segment.SectionId);
                ViewBag.SegmentId = segment.SegmentId;

                // For a dropdownlist that has a list of sections in the current district and the current section selected
                ViewBag.Sections = LocationModel.GetSectionList(segment.Section.DistrictId);
                ViewBag.SectionId = segment.SectionId;

                // For a dropdownlist that has a list of all districts and the current district selected
                ViewBag.Districts = LocationModel.GetDistrictList();
                ViewBag.DistrictId = segment.Section.DistrictId;

                // For displaying district, section, and segment names and bridge no on reports
                ViewBag.DistrictName = segment.Section.District.DistrictName;
                ViewBag.SectionName = segment.Section.SectionName;
                ViewBag.SegmentName = segment.SegmentName;
                TempData["SegmentName"] = $"{segment.SegmentNo} - {segment.SegmentName}";

                SegmentMap map = db.SegmentMaps.Find(id);
                TempData["HasKmlFile"] = (map != null);

                return View(segment);
            }
        }

        // GET
        public ActionResult UploadKmlFile()
        {
            return View();
        }

        public ActionResult UploadRoadKmlByName()
        {
            return View();
        }

        public ActionResult _FindSegmentByName(string term)
        {
            Segment segment = db.Segments.Where(seg => seg.SegmentName == term).FirstOrDefault();

            if (segment == null)
            {
                ViewBag.Success = false;
            }
            else
            {
                string segmentid = segment.SegmentId;

                ViewBag.Success = true;
            }

            SegmentMap map = db.SegmentMaps.Find(segment.SegmentId);
            ViewBag.HasKmlFile = (map != null);

            return PartialView(segment);
        }

        public string GetSegmentIdFromName(string segmentname)
        {
            Segment segment = db.Segments.Where(seg => seg.SegmentName == segmentname).FirstOrDefault();

            if (segment == null)
            {
                return "[{\"SegmentId\": \"\"}]";
            }

            // A json string of the form [{ "SegmentId": "A5F47F3C-4F84-486E-85A6-5E1A789474B1" }]
            return "[{\"SegmentId\": \"" + segment.SegmentId + "\"}]";
        }

        // Get a list of segments whose Name starting with term (for AutoComplete)
        public ActionResult GetSegmentListContaining(string term)
        {
            var result = from seg in db.Segments
                         where seg.SegmentName.ToLower().Contains(term)
                         select seg.SegmentName;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //public bool _FileUpload(HttpPostedFileBase file)
        public bool _UploadKmlFile()
        {
            //var excelUtility = new ExcelUtilityService();
            bool success = false;
            //Upload file to server
            //if (file != null)
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                string path = Server.MapPath("~/kml/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                file.SaveAs(path + Path.GetFileName(file.FileName));
                ViewBag.Message = "Kml file uploaded successfully.";
                success = true;
            }
            //return Json(segmentDoc);
            return success;
        }

        //GET
        public ActionResult _GetSegmentKmlUpload([Optional] string segmentid /* drop down value */)
        {
            if (segmentid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Segment segment = db.Segments.Find(segmentid);
            SegmentMap map = db.SegmentMaps.Find(segmentid);

            TempData["HasKmlFile"] = (map != null);

            return PartialView(segment);
        }

        [HttpPost]
        public JsonResult SegmentKmlSave(SegmentMap map)
        {
            if (Request.Files.Count > 0)
            {
                //Upload file to server
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(map.MapFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(map.MapFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);

                // Read map file as text
                map.MapFile = System.IO.File.ReadAllText(fullpath);

                // Save map and other info to database
                SetSegmentMap(map);

                ViewBag.Success = true;

            }

            return Json(map);
        }

        public void SetSegmentMap(SegmentMap segmentMap)
        {
            if (segmentMap != null)
            {
                SegmentMap map = db.SegmentMaps.Find(segmentMap.SegmentId);
                if (map != null && map.MapFile != null)
                {
                    // If an already existing SegmentMap, modify data
                    map.MapFile = segmentMap.MapFile;
                    map.MapFilePath = segmentMap.MapFilePath;
                    map.MapDate = segmentMap.MapDate;
                    map.Description = segmentMap.Description;

                    db.SaveChanges();
                }
                else
                {
                    // Otherwise, create a new segmentMap
                    SegmentMap newMap = new SegmentMap()
                    {
                        SegmentId = segmentMap.SegmentId,
                        MapFile = segmentMap.MapFile,
                        MapFilePath = segmentMap.MapFilePath,
                        MapDate = segmentMap.MapDate,
                        Description = segmentMap.Description,
                    };

                    db.SegmentMaps.Add(newMap);
                    db.SaveChanges();
                }
            }
        }

        //GET
        public ActionResult _GetSectionKmlUpload([Optional] string sectionid /* drop down value */)
        {
            if (sectionid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SectionMap map = db.SectionMaps.Find(sectionid);
            ViewBag.HasKmlFile = (map != null);

            Section section = db.Sections.Find(sectionid);

            ViewBag.SectionId = sectionid;
            ViewBag.SectionName = section.SectionName;
            ViewBag.SectionNo = section.SectionNo;

            return PartialView("_GetSectionKmlUpload", map);
        }

        [HttpPost]
        public JsonResult SectionKmlSave(SectionMap map)
        {
            if (Request.Files.Count > 0)
            {
                //Upload file to server
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(map.MapFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(map.MapFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);

                // Read map file as text
                map.MapFile = System.IO.File.ReadAllText(fullpath);

                // Save map and other info to database
                SetSectionMap(map);

                ViewBag.Success = true;

            }

            return Json(map);
        }

        public void SetSectionMap(SectionMap sectionMap)
        {
            if (sectionMap != null)
            {
                SectionMap map = db.SectionMaps.Find(sectionMap.SectionId);
                if (map != null && map.MapFile != null)
                {
                    // If an already existing SectionMap, modify data
                    map.MapFile = sectionMap.MapFile;
                    map.MapFilePath = sectionMap.MapFilePath;
                    map.MapDate = sectionMap.MapDate;
                    map.Description = sectionMap.Description;

                    db.SaveChanges();
                }
                else
                {
                    // Otherwise, create a new sectionMap
                    SectionMap newMap = new SectionMap()
                    {
                        SectionId = sectionMap.SectionId,
                        MapFile = sectionMap.MapFile,
                        MapFilePath = sectionMap.MapFilePath,
                        MapDate = sectionMap.MapDate,
                        Description = sectionMap.Description,
                    };

                    db.SectionMaps.Add(newMap);
                    db.SaveChanges();
                }
            }
        }

        //GET
        public ActionResult _GetDistrictKmlUpload([Optional] string districtid /* drop down value */)
        {
            if (districtid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DistrictMap map = db.DistrictMaps.Find(districtid);
            ViewBag.HasKmlFile = (map != null);

            District district = db.Districts.Find(districtid);

            ViewBag.DistrictId = districtid;
            ViewBag.DistrictName = district.DistrictName;

            return PartialView("_GetDistrictKmlUpload", map);
        }

        [HttpPost]
        public JsonResult DistrictKmlSave(DistrictMap map)
        {
            if (Request.Files.Count > 0)
            {
                //Upload file to server
                HttpPostedFileBase file = Request.Files[0];
                string fullpath = Server.MapPath(map.MapFilePath); // Path including the file name
                string path = Server.MapPath(Path.GetDirectoryName(map.MapFilePath)); // Path excluding the file name
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(fullpath);

                // Read map file as text
                map.MapFile = System.IO.File.ReadAllText(fullpath);

                // Save map and other info to database
                SetDistrictMap(map);

                ViewBag.Success = true;

            }

            return Json(map);
        }

        public void SetDistrictMap(DistrictMap districtMap)
        {
            if (districtMap != null)
            {
                DistrictMap map = db.DistrictMaps.Find(districtMap.DistrictId);
                if (map != null && map.MapFile != null)
                {
                    // If an already existing DistrictMap, modify data
                    map.MapFile = districtMap.MapFile;
                    map.MapFilePath = districtMap.MapFilePath;
                    map.MapDate = districtMap.MapDate;
                    map.Description = districtMap.Description;

                    db.SaveChanges();
                }
                else
                {
                    // Otherwise, create a new districtMap
                    DistrictMap newMap = new DistrictMap()
                    {
                        DistrictId = districtMap.DistrictId,
                        MapFile = districtMap.MapFile,
                        MapFilePath = districtMap.MapFilePath,
                        MapDate = districtMap.MapDate,
                        Description = districtMap.Description,
                    };

                    db.DistrictMaps.Add(newMap);
                    db.SaveChanges();
                }
            }
        }

        public string UploadSegmentKmlBySimilarName()
        {
            List<Segment> segments = db.Segments.ToList();

            int count = 0;
            foreach (var segment in segments)
            {
                foreach (var file in SegmentKmlFilesList)
                {
                    string s1 = RemoveSpecialChars(file);
                    string s2 = RemoveSpecialChars(segment.SegmentName);
                    var similarity = CalculateSimilarity(s1, s2);

                    if (similarity >= similarityPercentage)
                    {
                        count++;

                        // Upload file to server
                        string fullpath = SegmentDirectory + "\\" + file + ".kml"; // Path including the file name
                        string path = Server.MapPath(Path.GetDirectoryName("/kml/segments/")); // Path excluding the file name
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        SegmentMap map = db.SegmentMaps.Find(segment.SegmentId);

                        // Proceed to the upload only if map file is not already uploaded to the current road segment
                        if (map == null)
                        {
                            map = new SegmentMap();
                            map.SegmentId = segment.SegmentId;
                            map.MapDate = DateTime.Now;
                            map.MapFilePath = "/kml/segments/" + file + ".kml";
                            map.MapFile = System.IO.File.ReadAllText(fullpath);

                            // Save map and other info to database
                            SetSegmentMap(map);
                        }
                        break;
                    }
                }
            }

            return "Uploaded " + count + " segments...";
        }

        public int UploadSectionKmlBySimilarName()
        {
            List<Section> sections = db.Sections.ToList();

            int count = 0;
            foreach (var section in sections)
            {
                foreach (var file in SectionKmlFilesList)
                {
                    string s1 = RemoveSpecialChars(file);
                    string s2 = RemoveSpecialChars(section.SectionName);
                    var similarity = CalculateSimilarity(s1, s2);

                    if (similarity >= similarityPercentage)
                    {
                        count++;

                        // Upload file to server
                        string fullpath = SectionDirectory + "\\" + file + ".kml"; // Path including the file name
                        string path = Server.MapPath(Path.GetDirectoryName("/kml/sections/")); // Path excluding the file name
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        SectionMap map = db.SectionMaps.Find(section.SectionId);

                        // Proceed to the upload only if map file is not already uploaded to the current road section
                        if (map == null)
                        {
                            map = new SectionMap();
                            map.SectionId = section.SectionId;
                            map.MapDate = DateTime.Now;
                            map.MapFilePath = "/kml/sections/" + file + ".kml";
                            map.MapFile = System.IO.File.ReadAllText(fullpath);

                            // Save map and other info to database
                            SetSectionMap(map);
                        }
                        break;
                    }
                }
            }

            return count;
        }

        public List<string> GetKmlFileNames(string directory)
        {
            //e.g. directory = @"D:\ERA Road Mapping\Roads";

            List<string> pathes = Directory.GetFiles(directory).ToList();
            List<string> files = new List<string>();

            foreach (var path in pathes)
            {
                files.Add(Path.GetFileNameWithoutExtension(path));
            }

            return files;
        }

        public string RemoveSpecialChars(string s)
        {
            return Regex.Replace(s, "[^A-Za-z0-9]", "");
        }

        public JsonResult NoOfSimilarSegmentKml()
        {
            List<Segment> segments = db.Segments.ToList();

            int count = 0;
            List<KmlName> segmentPairs = new List<KmlName>();

            foreach (var segment in segments)
            {
                foreach (var file in SegmentKmlFilesList)
                {
                    string s1 = RemoveSpecialChars(file);
                    string s2 = RemoveSpecialChars(segment.SegmentName);
                    var similarity = CalculateSimilarity(s1, s2);

                    if (similarity >= similarityPercentage)
                    {
                        segmentPairs.Add(new KmlName() { FileName = file, SegmentName = segment.SegmentName });
                        count++;
                        break;
                    }
                }
            }

            return Json(segmentPairs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SimilarSegmentKml()
        {
            ViewBag.SimilarityPercentage = similarityPercentage;

            return View();
        }

        public int NoOfSimilarSectionKml()
        {
            List<Section> sections = db.Sections.ToList();

            int count = 0;

            foreach (var section in sections)
            {
                foreach (var file in SectionKmlFilesList)
                {
                    string s1 = RemoveSpecialChars(file);
                    string s2 = RemoveSpecialChars(section.SectionName);
                    var similarity = CalculateSimilarity(s1, s2);

                    if (similarity >= similarityPercentage)
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }


        /// <summary>
        /// Returns the number of steps required to transform the source string
        /// into the target string.
        /// </summary>
        public int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        /// <summary>
        /// Calculate percentage similarity of two strings
        /// <param name="source">Source String to Compare with</param>
        /// <param name="target">Targeted String to Compare</param>
        /// <returns>Return Similarity between two strings from 0 to 1.0</returns>
        /// </summary>
        public double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        public ActionResult NoMapSegments()
        {
            var roadswithnomap = (from seg in db.Segments
                                  join map in db.SegmentMaps on seg.SegmentId equals map.SegmentId into table3
                                  from seg_map in table3.DefaultIfEmpty()
                                  where seg_map.SegmentId == null
                                  select seg).ToList();

            return View(roadswithnomap);
        }
    }
}