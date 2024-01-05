using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class VisualInspectionViewModel
    {
        public InspVisualSubStructuresViewModel SubStructure { get; set; }
        public InspVisualSuperStructuresViewModel SuperStructure { get; set; }
        public InspVisualAncillariesViewModel Ancillaries { get; set; }
    }
}