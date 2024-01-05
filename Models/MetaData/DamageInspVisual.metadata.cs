using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageInspVisualMetaData))]
    public partial class DamageInspVisual
    {
    }

    public class DamageInspVisualMetaData
    {
        [Key]
        public string Id { get; set; }
        public string BridgeId { get; set; }
        public int StructureItemId { get; set; }
        public Nullable<int> DamageTypeId { get; set; }
        public Nullable<short> DamageSeverityId { get; set; }
        public Nullable<int> InspectionTypeId { get; set; }
        public Nullable<int> InspectionYear { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> InspectionDate { get; set; }
    }
}