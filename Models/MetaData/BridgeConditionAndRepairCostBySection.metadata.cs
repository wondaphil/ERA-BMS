using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeConditionAndRepairCostBySectionMetadata))]
    public partial class BridgeConditionAndRepairCostBySection
    {
    }
    public class BridgeConditionAndRepairCostBySectionMetadata
    {
        [Key]
        public string District { get; set; }
        public string Section { get; set; }

        [DisplayName("Inspection Year")]
        public int InspectionYear { get; set; }
        public int Good { get; set; }
        public int Fair { get; set; }
        public int Bad { get; set; }
        public int Total { get; set; }

        [DisplayName("Repair Cost")]
        public double RepairCost { get; set; }
    }
}