using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertConditionAndRepairCostByDistrictMetadata))]
    public partial class CulvertConditionAndRepairCostByDistrict
    {
    }
    public class CulvertConditionAndRepairCostByDistrictMetadata
    {
        [Key]
        public string District { get; set; }

        [DisplayName("Inspection Year")]
        public Nullable<int> InspectionYear { get; set; }
        public int Good { get; set; }
        public int Fair { get; set; }
        public int Bad { get; set; }
        public int Total { get; set; }
        
        [DisplayName("Repair Cost")]
        public double RepairCost { get; set; }
    }
}