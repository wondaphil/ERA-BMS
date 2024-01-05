using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DamageSeverityMetaData))]
    public partial class DamageSeverity
    {
    }

    public class DamageSeverityMetaData
    {
        public short DamageSeverityId { get; set; }
        public string DamageSeverityName { get; set; }
        public string Remark { get; set; }
    }
}