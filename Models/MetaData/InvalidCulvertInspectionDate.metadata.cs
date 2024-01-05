using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(InvalidCulvertInspectionDateMetadata))]
    public partial class InvalidCulvertInspectionDate
    {
    }
    public class InvalidCulvertInspectionDateMetadata
    {
        [Key, ForeignKey("Culvert")]
        [DisplayName("Culvert Id")]
        public string CulvertId { get; set; }

        [DisplayName("Segment")]
        public string SegmentName { get; set; }

        [DisplayName("Section")]
        public string SectionName { get; set; }

        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Inspection Date")]
        public Nullable<System.DateTime> InspectionDate { get; set; }

        [DisplayName("Damage %")]
        public Nullable<double> DamagePerc { get; set; }

        [DisplayName("Repair Cost")]
        public Nullable<double> RepairCost { get; set; }
    }
}