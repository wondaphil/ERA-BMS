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
    
    public partial class MainRoute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MainRoute()
        {
            this.SubRoutes = new HashSet<SubRoute>();
        }
    
        public string MainRouteId { get; set; }
        public string MainRouteNo { get; set; }
        public string MainRouteName { get; set; }
        public Nullable<double> Length { get; set; }
        public Nullable<int> RoadClassId { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public byte[] Flag { get; set; }
    
        public virtual RoadClass RoadClass { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubRoute> SubRoutes { get; set; }
    }
}