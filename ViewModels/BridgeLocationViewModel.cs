using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeLocationViewModel
    {
        public string BridgeId { get; set; }
        public string OldBridgeId { get; set; }
        public string RevisedBridgeId { get; set; }
        public string BridgeName { get; set; }
        public string District { get; set; }
        public string Section { get; set; }
        public string Segment { get; set; }
        public string MainRoute { get; set; }
        public string SubRoute { get; set; }
        public Nullable<double> KmFromAA { get; set; }
        public Nullable<double> BridgeLength { get; set; }
        public string BridgeType { get; set; }

        public string FoundationType { get; set; }
        public string ImageFilePath { get; set; }

        public Nullable<double> XCoord { get; set; }
        public Nullable<double> YCoord { get; set; }
        public Nullable<int> UtmZone { get; set; }
        public Nullable<double> LatitudeDecimal { get; set; }
        public Nullable<double> LongitudeDecimal { get; set; }
        public string LatitudeDMS { get; set; }
        public string LongitudeDMS { get; set; }
    }
}