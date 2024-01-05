using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ExpansionJointTypeMetadata))]
    public partial class ExpansionJointType
    {
    }

    public class ExpansionJointTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int ExpansionJointTypeId { get; set; }

        [Required]
        [DisplayName("Expansion Joint Type")]
        public string ExpansionJointTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}