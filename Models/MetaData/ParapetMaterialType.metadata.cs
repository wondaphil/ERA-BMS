using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ParapetMaterialTypeMetadata))]
    public partial class ParapetMaterialType
    {
    }

    public class ParapetMaterialTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int MaterialTypeId { get; set; }

        [DisplayName("Parapet Material Type")]
        public string MaterialTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}