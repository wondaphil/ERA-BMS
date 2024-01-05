using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(SoilTypeeMetadata))]
    public partial class SoilType
    {
    }

    public class SoilTypeeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int SoilTypeId { get; set; }

        [DisplayName("Soil Type")]
        public string SoilTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}