using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(DeckSlabTypeMetadata))]
    public partial class DeckSlabType
    {
    }

    public class DeckSlabTypeMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int DeckSlabTypeId { get; set; }
        
        [Required]
        [DisplayName("Deck Slab Type")]
        public string DeckSlabTypeName { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}