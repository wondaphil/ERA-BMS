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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Abutment
    {
        [Key, ForeignKey("Bridge")]
        public string BridgeAbutementId { get; set; }
        public Nullable<int> AbutmentTypeIdA1 { get; set; }
        public Nullable<int> AbutmentTypeIdA2 { get; set; }
        public Nullable<double> AbutmentHeightA1 { get; set; }
        public Nullable<double> AbutmentHeightA2 { get; set; }
        public Nullable<double> AbutmentWidthA1 { get; set; }
        public Nullable<double> AbutmentWidthA2 { get; set; }
        public Nullable<double> WingWallLengthA1 { get; set; }
        public Nullable<double> WingWallLengthA2 { get; set; }
        public Nullable<int> FoundationTypeIdA1 { get; set; }
        public Nullable<int> FoundationTypeIdA2 { get; set; }
        public string FoundationSizeA1 { get; set; }
        public string FoundationSizeA2 { get; set; }
        public Nullable<int> NoOfAbutmentPilesA1 { get; set; }
        public Nullable<int> NoOfAbutmentPilesA2 { get; set; }
        public Nullable<double> AbutmentPileDepthA1 { get; set; }
        public Nullable<double> AbutmentPileDepthA2 { get; set; }
        public string SoilType { get; set; }
        public string SoilType2 { get; set; }
        public Nullable<int> NoOfpier { get; set; }
    
        public virtual AbutmentType AbutmentType { get; set; }
        public virtual AbutmentType AbutmentType1 { get; set; }
        public virtual Bridge Bridge { get; set; }
        public virtual FoundationType FoundationType { get; set; }
        public virtual FoundationType FoundationType1 { get; set; }
    }
}
