using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspVisualSuperStructuresViewModel
    {
        public List<DamageInspVisual> DeckSlab { get; set; }
        public List<DamageInspVisual> ConcreteGirder { get; set; }
        public List<DamageInspVisual> SteelTrussGirder { get; set; }
        }
}