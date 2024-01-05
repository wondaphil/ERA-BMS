using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BearingTypeMetadata))]
    public partial class BearingType
    {
    }

    public class BearingTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int BearingTypeId { get; set; }

        [DisplayName("Bearing Type")]
        public string BearingTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}