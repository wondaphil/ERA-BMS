using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspVisualAncillariesViewModel
    {
        public List<DamageInspVisual> Pavement { get; set; }
        public List<DamageInspVisual> CurbAndRailing { get; set; }
        public List<DamageInspVisual> Drainage { get; set; }
        public List<DamageInspVisual> Bearing { get; set; }
        public List<DamageInspVisual> ExpansionJoint { get; set; }
    }
}