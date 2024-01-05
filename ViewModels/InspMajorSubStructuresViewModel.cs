using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspMajorSubStructuresViewModel
    {
        public List<DamageInspMajor> PierAndFoundation { get; set; }
        public List<DamageInspMajor> AbutmentAndWingWall { get; set; }
        public List<DamageInspMajor> Embankment { get; set; }
        public List<DamageInspMajor> RipRap { get; set; }
    }
}