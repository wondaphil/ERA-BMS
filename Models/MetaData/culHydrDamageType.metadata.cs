using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culHydrDamageTypeMetadata))]
    public partial class culHydrDamageType
    {
    }
    public class culHydrDamageTypeMetadata
    {
        [Key]
        public int DamageTypeId { get; set; }
        public string DamageTypeName { get; set; }
        public string Remark { get; set; }
    }
}