using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertInspectionViewModel
    {
        public InspCulvertStructureViewModel CulvertStructure { get; set; }
        public culDamageInspHydraulic HydraulicDamage { get; set; }
        public ObservationAndRecommendation Observations { get; set; }
    }
}