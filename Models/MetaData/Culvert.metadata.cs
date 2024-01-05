using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulverteMetadata))]
    public partial class Culvert
    {
    }

    public class CulverteMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string CulvertId { get; set; }

        [Required]
        [DisplayName("Old Culvert Id")]
        public string CulvertNo { get; set; }

        [DisplayName("Revised Culvert Id")]
        public string RevisedCulvertNo { get; set; }
        
        [Required]
        [DisplayName("Road Segment")]
        public string SegmentId { get; set; }

        [Required]
        [DisplayName("Sub Route")]
        public string SubRouteId { get; set; }
    }
}