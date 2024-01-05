using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeServiceConditionViewModel
    {
        public Bridge bridges { get; set; }
        public ResultInspMajor resultInspMajor { get; set; }
        public BridgeObservation observation { get; set; }
        //public int urgencyId { get; set; }
        //public bool waterWayAdequacy { get; set; }
        public DamageConditionRange condition { get; set; }
        public double damagePercent { get; set; }
        public double totalRepairCost { get; set; }
        public double priority { get; set; }
    }
}