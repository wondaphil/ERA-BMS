//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERA_BCMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeckSlabType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeckSlabType()
        {
            this.SuperStructures = new HashSet<SuperStructure>();
        }
    
        public int DeckSlabTypeId { get; set; }
        public string DeckSlabTypeName { get; set; }
        public string Remark { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SuperStructure> SuperStructures { get; set; }
    }
}