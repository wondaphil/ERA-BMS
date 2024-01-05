using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertsForUpgradingMetadata))]
    public partial class CulvertsForUpgrading
    {
    }
    public partial class CulvertsForUpgradingMetadata
    {
        [Key, ForeignKey("Culvert")]
        [DisplayName("Ser. No.")]
        public string CulvertId { get; set; }

        [DisplayName("Revised Culvert No")]
        public string RevisedCulvertNo { get; set; }
        
        [DisplayName("Culvert Type")]
        public string CulvertTypeName { get; set; }

        public string SegmentId { get; set; }
        
        [DisplayName("Segment")]
        public string SegmentName { get; set; }

        public string SectionId { get; set; }
        
        [DisplayName("Section")]
        public string SectionName { get; set; }

        public string DistrictId { get; set; }
        
        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Total Length (m)")]
        public Nullable<double> LengthTotal { get; set; }

        [DisplayName("Dist. From AA (Km)")]
        public Nullable<double> KMFromAddis { get; set; }

        [DisplayName("Construction Year")]
        public Nullable<int> ConstructionYear { get; set; }

        [DisplayName("Urgency Index")]
        public Nullable<int> UrgencyIndex { get; set; }

        [DisplayName("Intervention Type")]
        public string InterventionType { get; set; }
    }
}