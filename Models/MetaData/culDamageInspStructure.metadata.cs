using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culDamageInspStructureMetadata))]
    public partial class culDamageInspStructure
    {
    }
    public class culDamageInspStructureMetadata
    {
        [Key]
        public int Id { get; set; }
        
        [DisplayName("Culvert Id")]
        public string CulvertId { get; set; }

        [DisplayName("Inspection Date")]
        public Nullable<System.DateTime> InspectionDate { get; set; }

        [DisplayName("Structure Item Id")]
        public int StructureItemId { get; set; }

        [DisplayName("Damage Type Id")]
        public int DamageTypeId { get; set; }

        [DisplayName("Quantity")]
        public Nullable<double> Quantity { get; set; }
    }
}