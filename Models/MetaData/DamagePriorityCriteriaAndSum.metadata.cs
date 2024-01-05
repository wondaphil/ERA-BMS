using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamagePriorityCriteriaAndSumMetadata))]
    public partial class DamagePriorityCriteriaAndSum
    {
    }
    public class DamagePriorityCriteriaAndSumMetadata
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Priority Criteria")]
        public string PrioritizationCriteria { get; set; }

        [DisplayName("Damage Weight")]
        public Nullable<int> DamageWeight { get; set; }
    }
}