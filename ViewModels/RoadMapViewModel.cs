using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERA_BMS.ViewModels
{
    public class RoadMapViewModel
    {
        public string SegmentId { get; set; }
        public string SegmentNo { get; set; }
        public string RoadId { get; set; }
        public string RevisedRoadId { get; set; }
        public string SegmentName { get; set; }
        public string DistrictName { get; set; }
        public string SectionName { get; set; }
        public Nullable<double> RoadLength { get; set; }
        public Nullable<int> AADT { get; set; }
        public List<CoordinatePointViewModel> RoadPointList { get; set; }
    }

    public class CoordinatePointViewModel
    {
        public Nullable<double> lat { get; set; }
        public Nullable<double> lng { get; set; }
    }

    public class RoadPointViewModel
    {
        public Nullable<double> lat { get; set; }
        public Nullable<double> lng { get; set; }
    }

    public class RoadPointBySegmentViewModel
    {
        public string SegmentId { get; set; }
        public List<RoadPointViewModel> RoadPoints { get; set; }
    }
    public class DistrictBoundaryViewModel
    {
        public List<CoordinatePointViewModel> DistrictBoundaryPoints { get; set; }
        public string DistrictName { get; set; }
        public double TotalLength { get; set; }
    }

    public class SectionBoundaryViewModel
    {
        public List<CoordinatePointViewModel> SectionBoundaryPoints { get; set; }
        public string SectionName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public double TotalLength { get; set; }
    }
}