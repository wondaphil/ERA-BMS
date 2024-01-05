using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(AbutmentTypeMetadata))]
    public partial class AbutmentType
    {
    }
    public class AbutmentTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int AbutmentTypeId { get; set; }
        
        [Required]
        [DisplayName("Abutment Type")]
        public string AbutmentTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}