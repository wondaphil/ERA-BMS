using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageTypeMetaData))]
    public partial class DamageType
    {
    }

    public class DamageTypeMetaData
    {
        public int Id { get; set; }
        public int DamageTypeId { get; set; }
        public string DamageTypeName { get; set; }
        public int StructureItemId { get; set; }
    }
}