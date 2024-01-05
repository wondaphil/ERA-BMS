using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeValuePerLmMetadata))]
    public partial class BridgeValuePerLm
    {
    }
    public class BridgeValuePerLmMetadata
    {
        [Key]
        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Bridge Quantity")]
        public Nullable<int> BridgeQuantity { get; set; }

        [DisplayName("Total Structure Length")]
        public Nullable<double> TotalBridgeLength { get; set; }
        
        [DisplayName("Bridge Asset Value")]
        public double BridgeAssetValue { get; set; }

        [DisplayName("Bridge Value Per Lm")]
        public Nullable<double> BridgeValuePerLM1 { get; set; }
    }
}