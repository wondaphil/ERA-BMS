using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(SpanSupportTypeMetadata))]
    public partial class SpanSupportType
    {
    }

    public class SpanSupportTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int SpanSupportTypeId { get; set; }
        
        [Required]
        [DisplayName("Span Support Type")]
        public string SpanSupportTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}