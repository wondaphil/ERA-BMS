using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamagePriorityMetadata))]
    public partial class DamagePriority
    {
    }
    public class DamagePriorityMetadata
    {
        [Key]
        public int Id { get; set; }
        public int PrioritizationCriteriaId { get; set; }
        
        [DisplayName("Ser. No")]
        public int CriteriaSerNo { get; set; }
        public int DmgValFrom { get; set; }
        public int DmgValTo { get; set; }
        
        [DisplayName("Weight")]
        public int DmgWtVal { get; set; }
        public Nullable<int> DmgItemGroupId { get; set; }
        public Nullable<int> Status { get; set; }
    }
}