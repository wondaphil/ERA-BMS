using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeImprovementMetadata))]
    public partial class BridgeImprovement
    {
    }
    public class BridgeImprovementMetadata
    {
        [Key]
        public string Id { get; set; }

        [DisplayName("Bridge")]
        public string BridgeId { get; set; }

        [DisplayName("Improv. Year")]
        public Nullable<int> ImprovementYear { get; set; }

        [DisplayName("Date")]
        Nullable<System.DateTime> ImprovementDate { get; set; }

        [DisplayName("Improv. Action")]
        public Nullable<int> ImprovementTypeId { get; set; }

        [DisplayName("Contractor")]
        string Contractor { get; set; }

        [DisplayName("Supervisor")]
        public string Supervisor { get; set; }

        [DisplayName("Activities")]
        public string ImprovementAction { get; set; }

        [DisplayName("Improv. Cost, ETB")]
        public Nullable<double> ImprovementCost { get; set; }
        public Nullable<int> Status { get; set; }
        public string Remark { get; set; }
    }
}