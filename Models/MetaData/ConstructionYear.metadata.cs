using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ConstructionYearMetadata))]
    public partial class ConstructionYear
    {
    }
    public class ConstructionYearMetadata
    {
        [Key]
        public int CategoryId { get; set; }

        [DisplayName("Construction Year Range")]
        public string ConstructionYears { get; set; }

        [DisplayName("From")]
        public Nullable<int> FromYear { get; set; }

        [DisplayName("To")]
        public Nullable<int> ToYear { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}
