using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(SuperStructureMetadata))]
    public partial class SuperStructure
    {
    }

    public class SuperStructureMetadata
    {
        [Key, ForeignKey("Bridge")]
        public string BridgeId { get; set; }

        [DisplayName("Bridge Type")]
        public Nullable<int> BridgeTypeId { get; set; }

        [DisplayName("Girder Type")]
        public Nullable<short> GirderTypeId { get; set; }
        
        [DisplayName("No. of Span")]
        public Nullable<int> NoOfSpan { get; set; }

        [DisplayName("Span Length Composition")]
        public string SpanLengthComposition { get; set; }

        [DisplayName("Bridge Length (m)")]
        public Nullable<double> TotalSpanLength { get; set; }

        [DisplayName("Carriage Way Width (m)")]
        public Nullable<double> CarriageWayWidth { get; set; }

        [DisplayName("Side Walk Width (m)")]
        public Nullable<double> SideWalkWidth { get; set; }

        [DisplayName("No. of Lanes")]
        public Nullable<int> NoOfLane { get; set; }

        [DisplayName("Span Support Type")]
        public Nullable<int> SpanSupportTypeId { get; set; }

        [DisplayName("Deck Slab Type")]
        public Nullable<int> DeckSlabTypeId { get; set; }

        [DisplayName("Slab Thickness (cm)")]
        public Nullable<double> SlabThickness { get; set; }

        [DisplayName("No. of Girder (Box)")]
        public Nullable<int> NoOfGirder { get; set; }

        [DisplayName("Girder Depth (m)")]
        public Nullable<double> GirderDepth { get; set; }

        [DisplayName("Girder Spacing (m)")]
        public Nullable<double> SpacingGirder { get; set; }

        [DisplayName("Girder Box Width (m)")]
        public Nullable<double> GirderBoxWidth { get; set; }
    }
}