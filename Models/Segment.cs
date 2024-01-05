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
    
    public partial class Segment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Segment()
        {
            this.Bridges = new HashSet<Bridge>();
            this.Culverts = new HashSet<Culvert>();
        }
    
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Remark { get; set; }
    
        public virtual Section Section { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bridge> Bridges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Culvert> Culverts { get; set; }
    }
}
