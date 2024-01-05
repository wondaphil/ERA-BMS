using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageConditionRangeMetadata))]
    public partial class DamageConditionRange
    {
    }

    public class DamageConditionRangeMetadata
    {
        [Key]
        [DisplayName("Damage Condition Id")]
        public double DamageConditionId { get; set; }
        
        [DisplayName("Damage Condition")]
        public string DamageConditionName { get; set; }

        [DisplayName("From")]
        public double ValueFrom { get; set; }

        [DisplayName("To")]
        public double ValueTo { get; set; }

        [DisplayName("Action")]
        public string Action { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}