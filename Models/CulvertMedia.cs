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
    
    public partial class CulvertMedia
    {
        public string Id { get; set; }
        public string CulvertId { get; set; }
        public Nullable<int> MediaTypeId { get; set; }
        public string Description { get; set; }
        public string MediaFilePath { get; set; }
        public Nullable<System.DateTime> MediaDate { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public byte[] Flag { get; set; }
    
        public virtual MediaType MediaType { get; set; }
        public virtual Culvert Culvert { get; set; }
    }
}