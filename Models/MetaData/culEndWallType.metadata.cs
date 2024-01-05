using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culEndWallTypeMetadata))]
    public partial class culEndWallType
    {
    }

    public class culEndWallTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int EndWallTypeId { get; set; }

        [DisplayName("End Wall Type")]
        public string EndWallTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}