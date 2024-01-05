using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(RoadTypeMetadata))]
    public partial class RoadType
    {
    }

    public class RoadTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int RoadTypeId { get; set; }

        [Required]
        [DisplayName("Road Type")]
        public string RoadTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}