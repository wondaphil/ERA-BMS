using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(MaintenanceUrgencyMetadata))]
    public partial class MaintenanceUrgency
    {
    }
    public class MaintenanceUrgencyMetadata
    {
        [Key]
        public int UrgencyId { get; set; }
        public string UrgencyName { get; set; }
        public string Remark { get; set; }

    }
}