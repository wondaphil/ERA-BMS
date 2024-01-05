using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertDamageInspectionViewModel
    {
        public Culvert culverts { get; set; }
        public culDamageInspStructure damageInspCulvert { get; set; }
        public ResultInspCulvert resultInspCulvert { get; set; }
        public string condition { get; set; }
        public ObservationAndRecommendation recommendation { get; set; }
    }
}