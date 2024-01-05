using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culDamageRateAndCostMetadata))]
    public partial class culDamageRateAndCost
    {
    }

    public class culDamageRateAndCostMetadata
    {
        [Key]
        [DisplayName("Damage Type Id")]
        public int DamageTypeId { get; set; }
        public int Id { get; set; }
        
        [DisplayName("Damage Type Name")]
        public string DamageTypeName { get; set; }

        [DisplayName("Structure Item Id")]
        public Nullable<int> StructureItemId { get; set; }
        
        [DisplayName("Unit")]
        public string Unit { get; set; }

        [DisplayName("Damage Value")]
        public Nullable<double> DamageValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        [DisplayName("Unit Repair Cost")]
        public Nullable<double> UnitRepairCost { get; set; }
    }
}