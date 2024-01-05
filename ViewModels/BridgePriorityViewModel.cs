using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgePriorityViewModel
    {
        public Bridge bridges { get; set; }
        public BridgePriorityByInterventionRequirement bridgePriority { get; set; }
    }
}