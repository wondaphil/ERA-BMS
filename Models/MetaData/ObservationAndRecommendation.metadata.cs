using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ObservationAndRecommendationMetadata))]
    public partial class ObservationAndRecommendation
    {
    }
    public class ObservationAndRecommendationMetadata
    {
        [Key, ForeignKey("Culvert")]
        [DisplayName("Culvert Id")]
        public string CulvertId { get; set; }

        [DisplayName("Intervention Type")]
        public Nullable<int> UrgencyIndex { get; set; }

        [DisplayName("Inspector's Recommendation")]
        public string InspectorRecommendation { get; set; }

        [DisplayName("Inspection Date")]
        public Nullable<System.DateTime> InspectionDate { get; set; }

    }
}