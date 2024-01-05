using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeImprovementIndicatorViewModel
    {
        public Bridge bridges { get; set; }
        public Segment segments { get; set; }
        public Section sections { get; set; }
        public District districts { get; set; }
        public BridgeConditionByInspectionYear bridgeConditionByInspYear { get; set; }
    }
}