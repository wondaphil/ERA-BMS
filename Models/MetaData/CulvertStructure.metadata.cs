using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(CulvertStructureMetadata))]
    public partial class CulvertStructure
    {
    }

    public class CulvertStructureMetadata
    {
        [Key, ForeignKey("Culvert"), Required]
        [DisplayName("Culvert Id")]
        public int CulvertId { get; set; }
        
        [DisplayName("Culvert Type")]
        public Nullable<int> CulvertTypeId { get; set; }

        [DisplayName("Height (m)")]
        public Nullable<double> Height { get; set; }

        [DisplayName("Length Inside (m)")]
        public Nullable<double> LengthInside { get; set; }

        [DisplayName("No. of Barrels")]
        public Nullable<double> NoOfBarrels { get; set; }

        [DisplayName("Barrels b/n Barrels (m)")]
        public Nullable<double> BarrelsDistance { get; set; }

        [DisplayName("Total Length (m)")]
        public Nullable<double> LengthTotal { get; set; }

        [DisplayName("Abutment Type")]
        public Nullable<int> AbutmentTypeId { get; set; }

        [DisplayName("Abutment Height (m)")]
        public Nullable<double> AbutmentHeight { get; set; }

        [DisplayName("End Wall Type (Outlet)")]
        public Nullable<int> EndWallTypeIdOut { get; set; }

        [DisplayName("End Wall Type (Inlet)")]
        public Nullable<int> EndWallTypeIdIn { get; set; }
    }
}