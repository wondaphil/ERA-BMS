using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeCulvertDistrictSectionSegmentViewModel
    {
        public District Districts { get; set; }
        public Section Sections { get; set; }
        public Segment Segments { get; set; }
        public int BridgeCount { get; set; }
        public int CulvertCount { get; set; }
        public double BridgeImprovementCost { get; set; }
        public double CulvertImprovementCost { get; set; }
    }
}