using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(DistrictMapMetadata))]
    public partial class DistrictMap
    {
    }

    public class DistrictMapMetadata
    {
        [Key, ForeignKey("District"), Required]
        [DisplayName("Ser. No.")]
        public string DistrictId { get; set; }

        [DisplayName("Map File")]
        public string MapFile { get; set; }

        [DisplayName("Map File Path")]
        public string MapFilePath { get; set; }

        [DisplayName("Map Date")]
        public Nullable<System.DateTime> MapDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}