using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(FoundationTypeMetadata))]
    public partial class FoundationType
    {
    }

    public class FoundationTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int FoundationTypeId { get; set; }

        [Required]
        [DisplayName("Foundation Type")]
        public string FoundationTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}