using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(DistrictMetadata))]
    public partial class District
    {
    }

    public class DistrictMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string DistrictId { get; set; }

        [Required]
        [DisplayName("District No.")]
        public string DistrictNo { get; set; }
        
        [DisplayName("District")]
        [Required]
        public string DistrictName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        public override string ToString()
        {
            return $"{DistrictNo} - {DistrictName}";
        }
    }
}