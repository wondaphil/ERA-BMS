using System;
using System.ComponentModel.DataAnnotations;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageInspMajorMetaData))]
    public partial class DamageInspMajor
    {
    }

    public class DamageInspMajorMetaData
    {
        [Key]
        public string Id { get; set; }
        public string BridgeId { get; set; }
        public int StructureItemId { get; set; }
        public Nullable<int> DamageTypeId { get; set; }
        public Nullable<int> InspectionYear { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public Nullable<short> DamageRankId { get; set; }
        public Nullable<double> DamageArea { get; set; }
    }
}