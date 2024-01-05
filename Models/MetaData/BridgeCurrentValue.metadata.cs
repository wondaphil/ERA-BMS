using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeCurrentValueMetadata))]
    public partial class BridgeCurrentValue
    {
    }
    public class BridgeCurrentValueMetadata
    {
        [Key, ForeignKey("Bridge")]

        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Construction/ Replaced Year")]
        public Nullable<int> ConstructionYear { get; set; }

        [DisplayName("Construction Cost")]
        public Nullable<double> ConstructionCost { get; set; }

        [DisplayName("Replacement Cost")]
        public double ReplacementCost { get; set; }

        [DisplayName("Service Years")]
        public Nullable<int> ServiceYears { get; set; }

        [DisplayName("Service Year Rate")]
        public Nullable<decimal> ServiceYearRate { get; set; }

        [DisplayName("DOC")]
        public Nullable<double> DOC { get; set; }

        [DisplayName("Total Improvement Cost")]
        public double TotalImprovementCost { get; set; }

        [DisplayName("Latest Priority Value")]
        public Nullable<decimal> LatestPriorityVal { get; set; }

        [DisplayName("Bridge Condition Index (BCI)")]
        public Nullable<decimal> BridgeConditionIndex { get; set; }

        [DisplayName("Depreciated Replacement Cost")]
        public Nullable<double> DepreciatedReplacementCost { get; set; }
        
        [DisplayName("Current Value")]
        public Nullable<double> CurrentValue { get; set; }
    }
}