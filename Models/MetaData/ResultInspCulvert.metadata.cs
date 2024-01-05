using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ResultInspCulvertMetadata))]
    public partial class ResultInspCulvert
    {
    }
    public class ResultInspCulvertMetadata
    {
        [Key, ForeignKey("Culvert")]
        [DisplayName("Culvert Id")]
        public string CulvertId { get; set; }

        [DisplayName("Inspection Date")]
        public Nullable<System.DateTime> InspectionDate { get; set; }

        [DisplayName("Dmg %")]
        public Nullable<double> DamagePerc { get; set; }

        [DisplayName("Repair Cost")]
        public Nullable<double> RepairCost { get; set; }
    }
}