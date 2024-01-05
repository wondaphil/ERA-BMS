using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeDocMetadata))]
    public partial class BridgeDoc
    {
    }

    public class BridgeDocMetadata
    {
        [Key]
        [DisplayName("Id")]
        public string Id { get; set; }

        [ForeignKey("Bridge")]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Ser. No")]
        public Nullable<int> SerNo { get; set; }

        [DisplayName("Date")]
        public Nullable<System.DateTime> DocDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("File Path")]
        public string DocFilePath { get; set; }

        [DisplayName("Doc type")]
        public int DocTypeId { get; set; }

    }
}