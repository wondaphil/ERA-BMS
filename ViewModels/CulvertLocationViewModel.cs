using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertLocationViewModel
    {
        public string CulvertId { get; set; }
        public string OldCulvertId { get; set; }
        public string RevisedCulvertId { get; set; }
        public string District { get; set; }
        public string Section { get; set; }
        public string Segment { get; set; }
        public string MainRoute { get; set; }
        public string SubRoute { get; set; }
        public Nullable<double> KmFromAA { get; set; }
        public Nullable<double> CulvertLength { get; set; }
        public string CulvertType { get; set; }

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