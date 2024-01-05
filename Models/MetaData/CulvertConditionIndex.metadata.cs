using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertConditionIndexMetadata))]
    public partial class CulvertConditionIndex
    {
    }

    public class CulvertConditionIndexMetadata
    {
        [Key, ForeignKey("MaintenanceUrgency")]
        [DisplayName("Culvert Condition Id")]
        public double CulvertConditionId { get; set; }
        
        [DisplayName("Culvert Condition")]
        public string CulvertConditionName { get; set; }

        [DisplayName("Condition Index (CI)")]
        public double ConditionIndex { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}