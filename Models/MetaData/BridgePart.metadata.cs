using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgePartMetadata))]
    public partial class BridgePart
    {
    }

    public class BridgePartMetadata
    {
        [Key]
        [DisplayName("Damage Wt (%)")]
        public short BridgePartId { get; set; }

        [DisplayName("Bridge Part Name")]
        public string BridgePartName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}