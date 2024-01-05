using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertsForUpgradingViewModel
    {
        public Culvert Culvert { get; set; }
        public CulvertStructure CulStructure { get; set; }
        public CulvertGeneralInfo CulGenInfo { get; set; }
        public ObservationAndRecommendation recommendation { get; set; }
    }
}