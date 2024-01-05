using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeLengthMetadata))]
    public partial class BridgeLength
    {
    }

    public class BridgeLengthMetadata
    {
        [Key]
        public int BridgeLengthId { get; set; }

        [DisplayName("Length Range")]
        public string BridgeLengthName { get; set; }

        [DisplayName("From")]
        public Nullable<double> FromLength { get; set; }

        [DisplayName("To")]
        public Nullable<double> ToLength { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}