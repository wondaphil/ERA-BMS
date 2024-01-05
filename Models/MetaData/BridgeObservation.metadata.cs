using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeObservationMetadata))]
    public partial class BridgeObservation
    {
    }

    public class BridgeObservationMetadata
    {
        [Key]
        public string Id { get; set; }
        public string BridgeId { get; set; }
        public Nullable<int> InspectionYear { get; set; }
        public Nullable<System.DateTime> InspectionDate { get; set; }
        public Nullable<int> UrgencyId { get; set; }
        public Nullable<bool> WaterWayAdequacy { get; set; }

        public virtual Bridge Bridge { get; set; }
        public virtual MaintenanceUrgency MaintenanceUrgency { get; set; }
    }
}