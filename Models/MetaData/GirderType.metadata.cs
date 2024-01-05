using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(GirderTypeMetadata))]
    public partial class GirderType
    {
    }

    public class GirderTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int GirderTypeId { get; set; }
        [Required]

        [DisplayName("Girder Type")]
        public string GirderTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}