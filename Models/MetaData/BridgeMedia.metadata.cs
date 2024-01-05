using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeMediaMetadata))]
    public partial class BridgeMedia
    {
    }

    public class BridgeMediaMetadata
    {
        [Key]
        [DisplayName("Id")]
        public string Id { get; set; }

        [ForeignKey("Bridge"), Required]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Media Type")]
        public Nullable<int> MediaTypeId { get; set; }

        [DisplayName("Image No.")]
        public Nullable<int> ImageNo { get; set; }
        
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("File Path")]
        public string MediaFilePath { get; set; }

        [DisplayName("Date")]
        public Nullable<System.DateTime> MediaDate { get; set; }
    }
}