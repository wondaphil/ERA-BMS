//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERA_BMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StructureItemDmgWt
    {
        public int StructureItemId { get; set; }
        public Nullable<double> ConcreteDmgWt { get; set; }
        public Nullable<double> SteelDmgWt { get; set; }
        public Nullable<double> BridgePartDmgWt { get; set; }
        public Nullable<double> TotalStructureDmgWt { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public byte[] Flag { get; set; }
    
        public virtual StructureItem StructureItem { get; set; }
    }
}