using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeCulvertMainRouteSubRouteViewModel
    {
        public MainRoute MainRoutes { get; set; }
        public SubRoute SubRoutes { get; set; }
        public int? BridgeCount { get; set; }
        public int? CulvertCount { get; set; }
    }
}