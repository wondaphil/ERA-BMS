using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeCommentMetadata))]
    public partial class BridgeComment
    {
    }

    public class BridgeCommentMetadata
    {
        [Key]
        public string Id { get; set; }
        public string BridgeId { get; set; }
        public Nullable<int> InspectionYear { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public string Comment { get; set; }
    }
}