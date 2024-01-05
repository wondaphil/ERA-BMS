using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertMediaMetadata))]
    public partial class CulvertMedia
    {
    }

    public class CulvertMediaMetadata
    {
        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Culvert")]
        public string CulvertId { get; set; }

        [DisplayName("Media Type")]
        public Nullable<int> MediaTypeId { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("File Path")]
        public string MediaFilePath { get; set; }

        [DisplayName("Date")]
        public Nullable<System.DateTime> MediaDate { get; set; }
    }
}