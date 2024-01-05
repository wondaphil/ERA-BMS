using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeConditionByInspectionYearMetadata))]
    public partial class BridgeConditionByInspectionYear
    {
    }
    public class BridgeConditionByInspectionYearMetadata
    {
        [Key]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Bridge Name")]
        public string BridgeName { get; set; }

        [DisplayName("Bridge Type")]
        public string BridgeTypeName { get; set; }

        [DisplayName("Segment")]
        public string SegmentId { get; set; }

        [DisplayName("Section")]
        public string SectionId { get; set; }

        [DisplayName("District")]
        public string DistrictId { get; set; }
        
        [DisplayName("Structure Length")]
        public Nullable<double> BridgeLength { get; set; }

        [DisplayName("Construction Year")]
        public Nullable<int> ConstructionYear { get; set; }
        public double DmgPerc2006 { get; set; }
        public int Bad2006 { get; set; }
        public int Fair2006 { get; set; }
        public int Good2006 { get; set; }
        public double DmgPerc2010 { get; set; }
        public int Bad2010 { get; set; }
        public int Fair2010 { get; set; }
        public int Good2010 { get; set; }
        public double DmgPerc2013 { get; set; }
        public int Bad2013 { get; set; }
        public int Fair2013 { get; set; }
        public int Good2013 { get; set; }
        public double DmgPerc2016 { get; set; }
        public int Bad2016 { get; set; }
        public int Fair2016 { get; set; }
        public int Good2016 { get; set; }
        public double DmgPerc2020 { get; set; }
        public int Bad2020 { get; set; }
        public int Fair2020 { get; set; }
        public int Good2020 { get; set; }

    }
}