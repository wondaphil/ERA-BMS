using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertTypeMetadata))]
    public partial class CulvertType
    {
    }

    public class CulvertTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int CulvertTypeId { get; set; }

        [Required]
        [DisplayName("Culvert Type")]
        public string CulvertTypeName { get; set; }

        [DisplayName("Culvert Type Code")]
        public string CulvertTypeShortCode { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}