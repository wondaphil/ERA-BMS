using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class MajorInspectionViewModel
    {
        public InspMajorSubStructuresViewModel SubStructure { get; set; }
        public InspMajorSuperStructuresViewModel SuperStructure { get; set; }
        public InspMajorAncillariesViewModel Ancillaries { get; set; }
        public BridgeObservation Observations { get; set; }
        public List<BridgeComment> Comments { get; set; }
    }
}