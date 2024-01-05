using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(MediaTypeMetadata))]
    public partial class MediaType
    {
    }

    public class MediaTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int MediaTypeId { get; set; }

        [Required]
        [DisplayName("Media Type")]
        public string MediaTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}