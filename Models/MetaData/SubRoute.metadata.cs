using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(SubRouteMetadata))]
    public partial class SubRoute
    {
    }

    public class SubRouteMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string SubRouteId { get; set; }

        [Required]
        [DisplayName("Sub Route No.")]
        public string SubRouteNo { get; set; }
        
        [Required]
        [DisplayName("Sub Route")]
        public string SubRouteName { get; set; }
        
        [Required]
        [DisplayName("Main Route")]
        public string MainRouteId { get; set; }

        [DisplayName("From (Km)")]
        public Nullable<double> FromKm { get; set; }

        [DisplayName("To (Km)")]
        public Nullable<double> ToKm { get; set; }

        [DisplayName("Length (Km)")]
        public Nullable<double> Length { get; set; }

        [DisplayName("ADT")]
        public Nullable<double> AverageDailyTraffic { get; set; }
        
        [DisplayName("Remark")]
        public string Remark { get; set; }
        public override string ToString()
        {
            return $"{SubRouteNo} - {SubRouteName}";
        }
    }
}