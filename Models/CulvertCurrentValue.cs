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
    
    public partial class CulvertCurrentValue
    {
        public string CulvertId { get; set; }
        public string CulvertNo { get; set; }
        public string DistrictName { get; set; }
        public int ConstructionYear { get; set; }
        public Nullable<int> ServiceYears { get; set; }
        public double TotalLength { get; set; }
        public double InsideLength { get; set; }
        public double Area { get; set; }
        public Nullable<double> InitialConstructionCost { get; set; }
        public Nullable<double> CurrentReplacementCost { get; set; }
        public string ServiceCondition { get; set; }
        public Nullable<double> ConditionIndex { get; set; }
        public Nullable<double> DepreciatedReplacementCost { get; set; }
        public Nullable<double> CurrentValue { get; set; }
    }
}