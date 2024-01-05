using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeCulvertRegionSegmentViewModel
    {
        public RegionalGovernment Regions { get; set; }
        public Segment Segments { get; set; }
        public int BridgeCount { get; set; }
        public int CulvertCount { get; set; }
    }
}