using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DesignStandardMetadata))]
    public partial class DesignStandard
    {
    }

    public class DesignStandardMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int DesignStandardId { get; set; }

        [DisplayName("Design Std")]
        public string DesignStandardName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}