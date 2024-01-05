using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgePartDmgWtMetadata))]
    public partial class BridgePartDmgWt
    {
    }

    public class BridgePartDmgWtMetadata
    {
        [Key, ForeignKey("BridgePart")]
        [DisplayName("Bridge Part Id")]
        public short BridgePartId { get; set; }
        
        [DisplayName("Damage Wt (%)")]
        public double DmgWt { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        public virtual BridgePart BridgePart { get; set; }
    }
}