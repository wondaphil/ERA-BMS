using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertImprovementMetadata))]
    public partial class CulvertImprovement
    {
    }
    public class CulvertImprovementMetadata
    {
        [Key]
        public string Id { get; set; }

        [DisplayName("Culvert")]
        public string CulvertId { get; set; }

        [DisplayName("Improvement Year")]
        public Nullable<int> ImprovementYear { get; set; }

        [DisplayName("Improvement Date")]
        public Nullable<System.DateTime> ImprovementDate { get; set; }
        
        [DisplayName("Improvement Type")]
        public Nullable<int> ImprovementTypeId { get; set; }

        [DisplayName("Improvement Action")]
        public string ImprovementAction { get; set; }

        [DisplayName("Improvement Cost, ETB")]
        public Nullable<double> ImprovementCost { get; set; }

        [DisplayName("Status")]
        public Nullable<int> Status { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}