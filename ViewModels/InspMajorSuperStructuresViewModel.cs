using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspMajorSuperStructuresViewModel
    {
        public List<DamageInspMajor> DeckSlab { get; set; }
        public List<DamageInspMajor> ConcreteGirder { get; set; }
        public List<DamageInspMajor> SteelTrussGirder { get; set; }
    }
}