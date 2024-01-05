using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamagePriorityItemMetadata))]
    public partial class DamagePriorityItem
    {
    }
    public class DamagePriorityItemMetadata
    {
        [Key]
        public int PrioritizationCriteriaId { get; set; }
        public string PrioritizationCriteriaName { get; set; }
        public string ShortName { get; set; }
        public string Remark { get; set; }
    }
}