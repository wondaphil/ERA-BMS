using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(PierTypeMetadata))]
    public partial class PierType
    {
    }

    public class PierTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int PierTypeId { get; set; }

        [Required]
        [DisplayName("Pier Type")]
        public string PierTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}