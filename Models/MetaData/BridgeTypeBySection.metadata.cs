using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeTypeBySectionMetadata))]
    public partial class BridgeTypeBySection
    {
    }
    public class BridgeTypeBySectionMetadata
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

        [DisplayName("RC DG")]
        public Nullable<int> RC_Deck_Girder { get; set; }

        [DisplayName("RC BXG")]
        public Nullable<int> RC_Box_Girder { get; set; }

        [DisplayName("RC ARC")]
        public Nullable<int> RC_Arch { get; set; }

        [DisplayName("RC SLB")]
        public Nullable<int> RC_Slab_Bridge { get; set; }

        [DisplayName("BOX CUL")]
        public Nullable<int> RC_Box_Culvert { get; set; }

        [DisplayName("PC DG")]
        public Nullable<int> PC_Deck_Girder { get; set; }

        [DisplayName("PC BXG")]
        public Nullable<int> PC_Box_Girder { get; set; }

        [DisplayName("STL TRS")]
        public Nullable<int> Steel_Truss { get; set; }

        [DisplayName("STL RC COMP")]
        public Nullable<int> Steel_Girder_Composit { get; set; }

        [DisplayName("STL PAN BAIL")]
        public Nullable<int> Steel_Panel_Bailey { get; set; }

        [DisplayName("MAS ARC")]
        public Nullable<int> Masonry_Arch { get; set; }

        [DisplayName("STL TRS RC DG")]
        public Nullable<int> Steel_Truss_and_RCDG { get; set; }

        [DisplayName("RC BXG RC DG")]
        public Nullable<int> RC_Box_Girder_and_RCDG { get; set; }

        [DisplayName("RC ARC RC DG")]
        public Nullable<int> RC_Arch_and_RCDG { get; set; }

        [DisplayName("RC DG RC SLB")]
        public Nullable<int> RC_Deck_Girder_and_RC_Slab { get; set; }

        [DisplayName("RC RGD FRM")]
        public Nullable<int> RC_Rigid_Frame { get; set; }

        [DisplayName("EXT CBL")]
        public Nullable<int> Extradosed_Cable { get; set; }

        [DisplayName("STL TRS BXG")]
        public Nullable<int> Steel_Truss_and_Box_Girder { get; set; }

        [DisplayName("PIP ST RC")]
        public Nullable<int> Pipe_Steel_RC { get; set; }

        [DisplayName("RC DG CNT")]
        public Nullable<int> RC_Deck_Girder_Continuous { get; set; }
    }
}