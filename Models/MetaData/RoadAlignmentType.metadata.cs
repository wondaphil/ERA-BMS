using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(RoadAlignmentTypeMetadata))]
    public partial class RoadAlignmentType
    {
    }

    public class RoadAlignmentTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int RoadAlignmentTypeId { get; set; }

        [Required]
        [DisplayName("Road Alignment Type")]
        public string RoadAlignmentTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}