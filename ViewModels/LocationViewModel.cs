using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Models;


namespace ERA_BMS.ViewModels
{
    public class DropDownViewModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public static class LocationModel
    {
        private static BMSEntities db = new BMSEntities();
        private static List<DropDownViewModel> DistrictList;
        private static List<DropDownViewModel> SectionList;
        private static List<DropDownViewModel> SegmentList;
        private static List<DropDownViewModel> BridgeNameList;
        private static List<DropDownViewModel> CulvertNameList;
        private static List<DropDownViewModel> MainRouteNameList;
        private static List<DropDownViewModel> SubRouteNameList;

        public static List<DropDownViewModel> GetDistrictList()
        {
            DistrictList = (from dis in db.Districts
                            orderby dis.DistrictNo
                            select new DropDownViewModel { Value = dis.DistrictId.ToString(), Text = dis.DistrictNo + " - " + dis.DistrictName }).ToList();
            
            //Add the first unselected item
            DistrictList.Insert(0, new DropDownViewModel { Text = "--Select District--", Value = "0" });

            return DistrictList;
        }

        public static List<DropDownViewModel> GetSectionList([Optional] string districtid)
        {
            SectionList = (from sec in db.Sections
                           where sec.DistrictId == districtid
                           orderby sec.SectionNo
                           select new DropDownViewModel { Value = sec.SectionId.ToString(), Text = sec.SectionNo + " - " + sec.SectionName }).ToList();

            return SectionList;
        }

        public static List<DropDownViewModel> GetSegmentList([Optional] string sectionid)
        {
            SegmentList = (from seg in db.Segments
                           where seg.SectionId == sectionid
                           orderby seg.RevisedRoadId
                           select new DropDownViewModel { Value = seg.SegmentId.ToString(), Text = "[" + seg.RevisedRoadId + "] " + seg.SegmentName }).ToList();
            
            return SegmentList;
        }

        public static List<DropDownViewModel> GetBridgeNameList([Optional] string segmentid)
        {
            BridgeNameList = (from brg in db.Bridges
                              where brg.SegmentId == segmentid
                              orderby brg.RevisedBridgeNo
                              select new DropDownViewModel { Value = brg.BridgeId, Text = "[" + brg.RevisedBridgeNo + "] " + brg.BridgeName }).ToList();
            
            return BridgeNameList;
        }

        public static List<DropDownViewModel> GetCulvertNameList([Optional] string segmentid)
        {
            CulvertNameList = (from cul in db.Culverts
                               where cul.SegmentId == segmentid
                               orderby cul.RevisedCulvertNo
                               select new DropDownViewModel { Value = cul.CulvertId, Text = cul.CulvertNo }).ToList();
            
            return CulvertNameList;
        }

        public static List<DropDownViewModel> GetMainRouteNameList()
        {
            MainRouteNameList = (from rt in db.MainRoutes
                                 orderby rt.MainRouteNo
                                 select new DropDownViewModel { Value = rt.MainRouteId, Text = rt.MainRouteNo + " - " + rt.MainRouteName }).ToList();

            //Add the first unselected item
            MainRouteNameList.Insert(0, new DropDownViewModel { Text = "--Select Main Route--", Value = "0" });

            return MainRouteNameList;
        }

        public static List<DropDownViewModel> GetSubRouteNameList([Optional] string mainrouteid)
        {
            SubRouteNameList = (from sub in db.SubRoutes
                                where sub.MainRouteId == mainrouteid
                                orderby sub.SubRouteNo
                                select new DropDownViewModel { Value = sub.SubRouteId, Text = sub.SubRouteNo + " - " + sub.SubRouteName }).ToList();


            return SubRouteNameList;
        }
    }
}