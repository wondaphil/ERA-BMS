using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspVisualSubStructuresViewModel
    {
        public List<DamageInspVisual> PierAndFoundation { get; set; }
        public List<DamageInspVisual> AbutmentAndWingWall { get; set; }
        public List<DamageInspVisual> Embankment { get; set; }
        public List<DamageInspVisual> RipRap { get; set; }
    }
}