using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(AncillaryMetadata))]
    public partial class Ancillary
    {
    }

    public class AncillaryMetadata
    {
        [Key, ForeignKey("Bridge")]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Expansion Joint Type")]
        public Nullable<int> ExpansionJointTypeId { get; set; }

        [DisplayName("Guard Railing Type")]
        public Nullable<int> GuardRailingTypeId { get; set; }

        [DisplayName("Abutment Bearing Type")]
        public Nullable<int> AbutmentBearingTypeId { get; set; }

        [DisplayName("Piers Bearing Type")]
        public Nullable<int> PiersBearingTypeId { get; set; }

        [DisplayName("Surface Type")]
        public Nullable<int> SurfaceTypeId { get; set; }
    }
}