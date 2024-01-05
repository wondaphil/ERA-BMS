using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culDamageInspHydraulicMetadata))]
    public partial class culDamageInspHydraulic
    {
    }
    public class culDamageInspHydraulicMetadata
    {
        [Key, ForeignKey("Culvert")]
        [DisplayName("Culvert Id")]
        public string CulvertId { get; set; }
        public Nullable<int> OverTopping { get; set; }
        public Nullable<int> Constriction { get; set; }
        public Nullable<int> EmbankmentScour { get; set; }
        public Nullable<int> ChannelScour { get; set; }
        public Nullable<int> ChannelObstruction { get; set; }
        public Nullable<int> Siltation { get; set; }
        public Nullable<int> Vegitation { get; set; }

        [DisplayName("Inspection Date")]
        public Nullable<System.DateTime> InspectionDate { get; set; }

    }
}