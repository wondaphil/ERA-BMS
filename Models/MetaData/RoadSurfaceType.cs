using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(RoadSurfaceTypeMetadata))]
    public partial class RoadSurfaceType
    {
    }

    public class RoadSurfaceTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int RoadSurfaceTypeId { get; set; }

        [DisplayName("Surface Type")]
        public string RoadSurfaceTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}