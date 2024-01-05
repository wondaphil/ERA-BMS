using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(SectionMetadata))]
    public partial class Section
    {
    }

    public class SectionMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string SectionId { get; set; }

        [Required]
        [DisplayName("Section No.")]
        public string SectionNo { get; set; }
        
        [DisplayName("Section")]
        [Required]
        public string SectionName { get; set; }

        [DisplayName("District")]
        [Required]
        public string DistrictId { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
        public override string ToString()
        {
            return $"{SectionNo} - {SectionName}";
        }
    }
}