using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertValuePerLmMetadata))]
    public partial class CulvertValuePerLm
    {
    }
    public class CulvertValuePerLmMetadata
    {
        [Key]

        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Culvert Quantity")]
        public Nullable<int> CulvertQuantity { get; set; }

        [DisplayName("Total Culvert Length")]
        public Nullable<double> TotalCulvertLength { get; set; }

        [DisplayName("Culvert Asset Value")]
        public double CulvertAssetValue { get; set; }
        
        [DisplayName("Culvert Value Per Lm")]
        public Nullable<double> CulvertValuePerLM1 { get; set; }
    }
}