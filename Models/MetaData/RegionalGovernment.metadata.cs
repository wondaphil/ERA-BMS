using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(RegionalGovernmentMetadata))]
    public partial class RegionalGovernment
    {
    }

    public class RegionalGovernmentMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int RegionalGovernmentId { get; set; }

        [DisplayName("Reg. Gov't")]
        public string RegionalGovernmentName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}