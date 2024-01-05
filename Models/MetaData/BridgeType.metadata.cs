using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeTypeMetadata))]
    public partial class BridgeType
    {
    }

    public class BridgeTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int BridgeTypeId { get; set; }
        
        [Required]
        [DisplayName("Bridge Type")]
        public string BridgeTypeName { get; set; }

        [DisplayName("Bridge Type Code")]
        public string BridgeTypeShortCode { get; set; }
        
        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}