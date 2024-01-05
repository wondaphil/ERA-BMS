using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(ImprovementTypeMetadata))]
    public partial class ImprovementType
    {
    }
    public class ImprovementTypeMetadata
    {
        [Key]
        public int ImprovementTypeId { get; set; }

        [DisplayName("Improvement Action")]
        public string ImprovementTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}