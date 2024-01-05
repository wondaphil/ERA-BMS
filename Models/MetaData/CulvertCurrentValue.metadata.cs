using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertCurrentValueMetadata))]
    public partial class CulvertCurrentValue
    {
    }
    public class CulvertCurrentValueMetadata
    {
        [Key, ForeignKey("Culvert")]

        [DisplayName("Culvert Id")]
        public string CulvertId { get; set; }

        [DisplayName("Old Culvert Id")]
        public string CulvertNo { get; set; }

        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Constr. Year")]
        public int ConstructionYear { get; set; }

        [DisplayName("Service Years")]
        public Nullable<int> ServiceYears { get; set; }

        [DisplayName("Tot. Len, m")]
        public double TotalLength { get; set; }

        [DisplayName("Inside Len, m")]
        public double InsideLength { get; set; }

        [DisplayName("Area, Sq. m.")]
        public double Area { get; set; }

        [DisplayName("Initial Constr. Cost")]
        public Nullable<double> InitialConstructionCost { get; set; }

        [DisplayName("Current Replac. Cost")]
        public Nullable<double> CurrentReplacementCost { get; set; }

        [DisplayName("Service Condition")]
        public string ServiceCondition { get; set; }

        [DisplayName("Condition Index (CI)")]
        public Nullable<double> ConditionIndex { get; set; }

        [DisplayName("Deprec. Replac. Cost")]
        public Nullable<double> DepreciatedReplacementCost { get; set; }

        [DisplayName("Current Value")]
        public Nullable<double> CurrentValue { get; set; }
    }
}