using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageRankMetaData))]
    public partial class DamageRank
    {
    }

    public class DamageRankMetaData
    {
        [Key]
        [DisplayName("Damage Rank Id")]
        public short DamageRankId { get; set; }

        [DisplayName("Rank")]
        public string DamageRankName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}