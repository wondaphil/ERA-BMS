using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertTypeBySectionMetadata))]
    public partial class CulvertTypeBySection
    {
    }
    public class CulvertTypeBySectionMetadata
    {
        [Key]
        [DisplayName("Section Id")]
        public string SectionId { get; set; }

        [DisplayName("Section")]
        public string SectionName { get; set; }

        [DisplayName("District Id")]
        public Nullable<int> DistrictId { get; set; }

        [DisplayName("Total")]
        public Nullable<int> Total { get; set; }

        [DisplayName("Slab Culvert")]
        public Nullable<int> Slab_Culvert { get; set; }

        [DisplayName("Arch Culvert")]
        public Nullable<int> Arch_Culvert { get; set; }

        [DisplayName("Box Culvert")]
        public Nullable<int> Box_Culvert { get; set; }

        [DisplayName("RC Pipe")]
        public Nullable<int> RC_Pipe { get; set; }

        [DisplayName("Metal Pipe")]
        public Nullable<int> Metal_Pipe { get; set; }

        [DisplayName("Ford")]
        public Nullable<int> Ford { get; set; }

        [DisplayName("Mixed")]
        public Nullable<int> Mixed { get; set; }

        [DisplayName("Not Visible")]
        public Nullable<int> Not_Visible { get; set; }
    }
}