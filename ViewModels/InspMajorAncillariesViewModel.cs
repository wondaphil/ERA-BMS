using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspMajorAncillariesViewModel
    {
        public List<DamageInspMajor> Pavement { get; set; }
        public List<DamageInspMajor> CurbAndRailing { get; set; }
        public List<DamageInspMajor> Drainage { get; set; }
        public List<DamageInspMajor> Bearing { get; set; }
        public List<DamageInspMajor> ExpansionJoint { get; set; }
    }
}