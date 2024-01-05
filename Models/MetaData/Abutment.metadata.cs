using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(AbutmentMetadata))]
    public partial class Abutment
    {
    }

    public class AbutmentMetadata
    {
        [Key, ForeignKey("Bridge")]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Type A1")]
        public Nullable<int> AbutmentTypeIdA1 { get; set; }

        [DisplayName("Type A2")]
        public Nullable<int> AbutmentTypeIdA2 { get; set; }

        [DisplayName("Height A1 (m)")]
        public Nullable<double> AbutmentHeightA1 { get; set; }

        [DisplayName("Height A2 (m)")]
        public Nullable<double> AbutmentHeightA2 { get; set; }

        [DisplayName("Width A1 (m)")]
        public Nullable<double> AbutmentWidthA1 { get; set; }

        [DisplayName("Width A2 (m)")]
        public Nullable<double> AbutmentWidthA2 { get; set; }

        [DisplayName("Wing Wall Length A1")]
        public Nullable<double> WingWallLengthA1 { get; set; }

        [DisplayName("Wing Wall Length A2")]
        public Nullable<double> WingWallLengthA2 { get; set; }

        [DisplayName("Foundation Type A1")]
        public Nullable<int> FoundationTypeIdA1 { get; set; }

        [DisplayName("Foundation Type A2")]
        public Nullable<int> FoundationTypeIdA2 { get; set; }

        [DisplayName("Foundation Size A1 (m)")]
        public string FoundationSizeA1 { get; set; }

        [DisplayName("Foundation Size A2 (m)")]
        public string FoundationSizeA2 { get; set; }

        [DisplayName("No. of Piles A1")]
        public Nullable<int> NoOfAbutmentPilesA1 { get; set; }

        [DisplayName("No. of Piles A2")]
        public Nullable<int> NoOfAbutmentPilesA2 { get; set; }

        [DisplayName("Pile Depth A1 (m)")]
        public Nullable<double> AbutmentPileDepthA1 { get; set; }

        [DisplayName("Pile Depth A2 (m)")]
        public Nullable<double> AbutmentPileDepthA2 { get; set; }

        [DisplayName("Soil Type A1")]
        public Nullable<int> SoilTypeA1 { get; set; }

        [DisplayName("Soil Type A2")]
        public Nullable<int> SoilTypeA2 { get; set; }

        [DisplayName("No. of pier")]
        public Nullable<int> NoOfpier { get; set; }

        //[JsonIgnore]
        //public virtual Bridge Bridge { get; set; }
    } 
}