using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(StructureItemDmgWtMetaData))]
    public partial class StructureItemDmgWt
    {
    }

    public class StructureItemDmgWtMetaData
    {
        [Key, ForeignKey("StructureItem")]
        [DisplayName("Structure Item Id")]
        public int StructureItemId { get; set; }
        
        [DisplayName("Concrete Dmg Wt (%)")]
        public Nullable<double> ConcreteDmgWt { get; set; }

        [DisplayName("Steel Dmg Wt (%)")]
        public Nullable<double> SteelDmgWt { get; set; }

        [DisplayName("Bridge Part Dmg Wt (%)")]
        public Nullable<double> BridgePartDmgWt { get; set; }

        [DisplayName("Total Structure Dmg Wt (%)")]
        public Nullable<double> TotalStructureDmgWt { get; set; }
        
        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}