using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeConstructionYearViewModel
    {
        public ConstructionYear ConstructionYear { get; set; }
        public int BridgeCount { get; set; }
    }
}