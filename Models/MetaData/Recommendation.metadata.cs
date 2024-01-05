using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(RecommendationMetadata))]
    public partial class Recommendation
    {
    }

    public class RecommendationMetadata
    {
        [Key]
        public int RecommendationId { get; set; }
        public string RecommendationName { get; set; }
        public string Remark { get; set; }
    }
}