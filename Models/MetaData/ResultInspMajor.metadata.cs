using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ResultInspMajorMetadata))]
    public partial class ResultInspMajor
    {
    }

    public class ResultInspMajorMetadata
    {
        public string Id { get; set; }

        [Key, Column(Order = 1)]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [Key, Column(Order = 2)]
        [DisplayName("Inspection Year")]
        public Nullable<int> InspectionYear { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }

        [DisplayName("Dmg % Sub Structure")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> DmgPercSubStructure { get; set; }

        [DisplayName("Dmg % Super Structure")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> DmgPercSuperStructure { get; set; }

        [DisplayName("Dmg % Ancillaries")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> DmgPercAncillaries { get; set; }

        [DisplayName("Dmg %")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> DmgPerc { get; set; }

        [DisplayName("Dmg % Avg")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> DmgPercAvg { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> TotalDamageRatio { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> RepairCostRatio { get; set; }

        [DisplayName("Cost Superstr.")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> RepairCostSuperStruct { get; set; }

        [DisplayName("Cost Substr.")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> RepairCostSubStruct { get; set; }

        [DisplayName("Cost Ancill.")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")] 
        public Nullable<double> RepairCostAncillaries { get; set; }
        public Nullable<bool> WaterWayAdequacy_Insp { get; set; }
        public Nullable<int> RecomendationCode_Insp { get; set; }
        public string Recomendation_Insp { get; set; }
        public Nullable<short> RecStatus { get; set; }
    }
}