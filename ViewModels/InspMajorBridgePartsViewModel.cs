using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class InspMajorBridgePartsViewModel
    {
        public InspMajorSubStructuresViewModel subStructure { get; set; }
        public InspMajorSuperStructuresViewModel superStructure { get; set; }
        public InspMajorAncillariesViewModel ancillaries { get; set; }
    }
}