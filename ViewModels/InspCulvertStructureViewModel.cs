using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspCulvertStructureViewModel
    {
        public List<culDamageInspStructure> Culvert { get; set; }
        public List<culDamageInspStructure> Abutment { get; set; }
        public List<culDamageInspStructure> GuardParapet { get; set; }
        public List<culDamageInspStructure> WingWall { get; set; }
        public List<culDamageInspStructure> HeadWall { get; set; }
        public List<culDamageInspStructure> PavedWaterWay { get; set; }
    }
}