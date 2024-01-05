using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culPierTypeMetadata))]
    public partial class culPierType
    {
    }

    public class culPierTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int PierTypeId { get; set; }

        [DisplayName("Pier Type")]
        public string PierTypeName { get; set; }
        
        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}