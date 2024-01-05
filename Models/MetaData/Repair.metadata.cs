using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(RepairMetadata))]
    public partial class Repair
    {
    }

    public class RepairMetadata
    {
        [Key]
        public int Id { get; set; }
        public string BridgeId { get; set; }
        public Nullable<int> InspectionYYMM { get; set; }
        public Nullable<int> BudgetYear { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public string ActivityDescription { get; set; }
        public Nullable<int> StructureItemNo { get; set; }
        public string StructureItems { get; set; }
        public Nullable<int> RepairPlanDate { get; set; }
        public Nullable<int> RepairProgDate { get; set; }
        public string WorkCategory { get; set; }
        public string RepairMethod1 { get; set; }
        public string RepairMethod2 { get; set; }
        public Nullable<decimal> CostPlan { get; set; }
        public Nullable<decimal> Costprog { get; set; }
        public Nullable<int> FileNumber { get; set; }
    }
}