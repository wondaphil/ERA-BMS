using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageRateAndCostMetaData))]
    public partial class DamageRateAndCost
    {
    }

    public class DamageRateAndCostMetaData
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Damage Type Id")]
        public int DamageTypeId { get; set; }

        [DisplayName("Structure Item Id")]
        public int StructureItemId { get; set; }

        [DisplayName("Unit")]
        public string Unit { get; set; }

        [DisplayName("Rank")]
        public Nullable<short> DamageRankId { get; set; }

        [DisplayName("Damage %")]
        public Nullable<double> DamagePercentValue { get; set; }

        [DisplayName("Unit Repair Cost")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public Nullable<double> UnitRepairCost { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("RankNo")]
        public Nullable<short> RankNo { get; set; }

        [DisplayName("DamagePictureFileName")]
        public string DamagePictureFileName { get; set; }

        [DisplayName("IsSelected")]
        public Nullable<short> IsSelected { get; set; }
    }
}