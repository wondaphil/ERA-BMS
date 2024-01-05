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
    
    public partial class Rehabilitation
    {
        public int Id { get; set; }
        public string BridgeId { get; set; }
        public Nullable<int> ConstYear { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public string Contractor { get; set; }
        public string Designer { get; set; }
        public string RepairMethod1 { get; set; }
        public string RepairMethod2 { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> Status { get; set; }
        public string PicBefore { get; set; }
        public string PicAfter { get; set; }
    
        public virtual Bridge Bridge { get; set; }
        public virtual ImprovementAction ImprovementAction { get; set; }
    }
}