using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(culStructureItemMetadata))]
    public partial class culStructureItem
    {
    }
    public class culStructureItemMetadata
    {
        [Key]
        public int StructureItemId { get; set; }
        public string StructureItemName { get; set; }
        public string Remark { get; set; }
    }
}