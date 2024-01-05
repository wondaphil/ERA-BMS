using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgePriorityByHighestDamageRankMetadata))]
    public partial class BridgePriorityByHighestDamageRank
    {
    }

    public class BridgePriorityByHighestDamageRankMetadata
    {
        [Key]
        [DisplayName("Ser. No.")]
        public string BridgeId { get; set; }

        [DisplayName("Revised Bridge Id")]
        public string RevisedBridgeNo { get; set; }

        [DisplayName("Bridge Name")]
        public string BridgeName { get; set; }

        [DisplayName("Segment Id")]
        public string SegmentId { get; set; }

        [DisplayName("Segment")]
        public string SegmentName { get; set; }

        [DisplayName("Section Id")]
        public string SectionId { get; set; }

        [DisplayName("Section")]
        public string SectionName { get; set; }

        [DisplayName("District Id")]
        public string DistrictId { get; set; }

        [DisplayName("District")]
        public string DistrictName { get; set; }
        [DisplayName("Inspection Year")]
        public int InspectionYear { get; set; }

        [DisplayName("Length")]
        public Nullable<double> BridgeLength { get; set; }

        [DisplayName("Bridge Type")]
        public string BridgeType { get; set; }

        [DisplayName("Recommendation")]
        public string Recommendation { get; set; }
        
        [DisplayName("Deck Slab")]
        public string DeckSlab { get; set; }

        [DisplayName("Concrete Girder")]
        public string ConcreteGirder { get; set; }

        [DisplayName("Pier & Foundation")]
        public string PierAndFoundation { get; set; }

        [DisplayName("Abutment & Wingwall")]
        public string AbutmentAndWingWall { get; set; }

        [DisplayName("Bearing")]
        public string Bearing { get; set; }

        [DisplayName("Repair Cost")]
        public Nullable<double> RepairCost { get; set; }
    } 
}