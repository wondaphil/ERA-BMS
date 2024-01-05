using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(GuardRailingTypeMetadata))]
    public partial class GuardRailingType
    {
    }

    public class GuardRailingTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int GuardRailingTypeId { get; set; }

        [DisplayName("Guard Railing Type")]
        public string GuardRailingTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}