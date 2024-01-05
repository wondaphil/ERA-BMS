using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(UtmZoneEthiopiaMetadata))]
    public partial class UtmZoneEthiopia
    {
    }

    public class UtmZoneEthiopiaMetadata
    {
        [Key, Required]
        [DisplayName("Id")]
        public int UtmZoneId { get; set; }

        [Required]
        [DisplayName("UTM Zone")]
        public int UtmZone { get; set; }

        [DisplayName("Central Meridian")]
        public string CentralMeridian { get; set; }

        [DisplayName("Longitude")]
        public string Longitude { get; set; }

        [DisplayName("Remark")]
        public string Remark { get; set; }
    }
}