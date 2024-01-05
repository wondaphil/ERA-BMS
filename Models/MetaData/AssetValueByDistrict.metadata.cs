using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(AssetValueByDistrictMetadata))]
    public partial class AssetValueByDistrict
    {
    }
    public class AssetValueByDistrictMetadata
    {
        [Key]
        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Bridge Asset Value")]
        public Nullable<double> BridgeAssetValue { get; set; }

        [DisplayName("Culvert Asset Value")]
        public Nullable<double> CulvertAssetValue { get; set; }

        [DisplayName("Total Asset Value")]
        public Nullable<double> TotalAssetValue { get; set; }
    }
}