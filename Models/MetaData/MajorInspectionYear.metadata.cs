using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(MajorInspectionYearMetadata))]
    public partial class MajorInspectionYear
    {
    }

    public class MajorInspectionYearMetadata
    {
        [Key]
        [DisplayName("Id")]
        public int Id { get; set; }

        [DisplayName("Inspection Year")]
        [Required(ErrorMessage = "* Required")]
        public int InspectionYear { get; set; }

        [DisplayName("Start Year")]
        [Required(ErrorMessage = "* Required")]
        public Nullable<int> StartYear { get; set; }

        [DisplayName("End Year")]
        [Required(ErrorMessage = "* Required")]
        public Nullable<int> EndYear { get; set; }
        
        [DisplayName("No. of Registered Bridges")]
        [Required(ErrorMessage = "* Required")]
        public Nullable<int> NoOfRegisteredBridges { get; set; }

        [DisplayName("No. of Registered Culverts")]
        public Nullable<int> NoOfRegisteredCulverts { get; set; }

        [DisplayName("No. of Inspected Bridges")]
        public Nullable<int> NoOfInspectedBridges { get; set; }

        [DisplayName("No. of Inspected Culverts")]
        public Nullable<int> NofInspectedCulverts { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}