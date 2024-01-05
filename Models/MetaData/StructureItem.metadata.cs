using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(StructureItemMetaData))]
    public partial class StructureItem
    {
    }

    public class StructureItemMetaData
    {
        [Key]
        [DisplayName("Structure Item Id")]
        public int StructureItemId { get; set; }

        [DisplayName("Structure Item")]
        public string StructureItemName { get; set; }

        [DisplayName("Bridge Part Id")]
        public Nullable<short> BridgePartId { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}