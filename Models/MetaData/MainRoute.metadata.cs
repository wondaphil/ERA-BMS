using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(MainRouteMetadata))]
    public partial class MainRoute
    {
    }
    public class MainRouteMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string MainRouteId { get; set; }


        [Required]
        [DisplayName("Main Route No.")]
        public string MainRouteNo { get; set; }
        
        [DisplayName("Main Route")]
        public string MainRouteName { get; set; }

        [DisplayName("Length (Km)")]
        public Nullable<double> Length { get; set; }

        [DisplayName("Road Class")]
        public Nullable<int> RoadClassId { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
        public override string ToString()
        {
            return $"{MainRouteNo} - {MainRouteName}";
        }
    }
}