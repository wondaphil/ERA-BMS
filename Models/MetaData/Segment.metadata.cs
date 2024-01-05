using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(SegmentMetadata))]
    public partial class Segment
    {
    }

    public class SegmentMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string SegmentId { get; set; }

        [Required]
        [DisplayName("Segment No")]
        public string SegmentNo { get; set; }

        [DisplayName("Old Road Id")]
        public string RoadId { get; set; }

        [DisplayName("Revised Road Id")]
        public string RevisedRoadId { get; set; }

        [Required]
        [DisplayName("Road Segment")]
        public string SegmentName { get; set; }

        [DisplayName("Asphalt Length, km")]
        public Nullable<double> AsphaltLength { get; set; }

        [DisplayName("GravelLength, km")]
        public Nullable<double> GravelLength { get; set; }

        [DisplayName("Length, km")]
        public Nullable<double> Length { get; set; }

        [DisplayName("Average Width, m")]
        public Nullable<double> Width { get; set; }

        [DisplayName("Construction Year")]
        public Nullable<int> ConstructionYear { get; set; }

        [DisplayName("Road Class")]
        public Nullable<int> RoadClassId { get; set; }
        
        [Required]
        [DisplayName("Section")]
        public string SectionId { get; set; }

        [DisplayName("Surface Type")]
        public Nullable<int> RoadSurfaceTypeId { get; set; }

        [DisplayName("AADT")]
        public Nullable<int> AverageDailyTraffic { get; set; }

        [DisplayName("Regional Government")]
        public Nullable<int> RegionalGovernmentId { get; set; }

        [DisplayName("Design Standard")]
        public Nullable<int> DesignStandardId { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }

        public override string ToString()
        {
            return $"{SegmentNo} - {SegmentName}";
        }
    }
}