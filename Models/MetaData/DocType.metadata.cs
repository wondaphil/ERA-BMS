using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DocTypeMetadata))]
    public partial class DocType
    {
    }

    public class DocTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int DocTypeId { get; set; }

        [Required]
        [DisplayName("Doc Type")]
        public string DocTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}