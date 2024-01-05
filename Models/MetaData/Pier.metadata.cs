using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(PierMetadata))]
    public partial class Pier
    {
    }
    public class PierMetadata
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("Bridge"), Required]
        public string BridgeId { get; set; }

        [DisplayName("Pier No")]
        public int PierNo { get; set; }

        [DisplayName("Pier Type")]
        public Nullable<int> PierTypeId { get; set; }

        [DisplayName("Pier Height")]
        public Nullable<double> HeightOfPier { get; set; }

        [DisplayName("Pier Width")]
        public Nullable<double> WidthOfPier { get; set; }

        [DisplayName("Foundation Type")]
        public Nullable<int> FoundationTypeId { get; set; }

        [DisplayName("Foundation Dimension (L x W)")]
        public string FoundationDimension { get; set; }

        [DisplayName("No. of Pier Piles")]
        public Nullable<int> NoOfPierPiles { get; set; }

        [DisplayName("Pier Pile Depth (m)")]
        public Nullable<double> PierPileDepth { get; set; }
    }
}