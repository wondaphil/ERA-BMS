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
    
    public partial class Route
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Route()
        {
            this.Bridges = new HashSet<Bridge>();
            this.Culverts = new HashSet<Culvert>();
        }
    
        public string RouteId { get; set; }
        public string RouteName { get; set; }
        public Nullable<int> RouteSerNo { get; set; }
        public string MainRoadId { get; set; }
        public Nullable<double> FromKm { get; set; }
        public Nullable<double> ToKm { get; set; }
        public Nullable<double> Length { get; set; }
        public Nullable<double> ADT { get; set; }
        public string Remark { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bridge> Bridges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Culvert> Culverts { get; set; }
    }
}
