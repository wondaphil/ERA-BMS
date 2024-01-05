using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgeMetadata))]
    public partial class Bridge
    {
    }

    public class BridgeMetadata
    {
        [Key, Required]
        [DisplayName("Ser. No.")]
        public string BridgeId { get; set; }

        [Required]
        [DisplayName("Old Bridge Id")]
        public string BridgeNo { get; set; }

        [DisplayName("Revised Bridge Id")]
        public string RevisedBridgeNo { get; set; }
        
        [Required]
        [DisplayName("Bridge Name")]
        public string BridgeName { get; set; }

        [Required]
        [DisplayName("Road Segment")]
        public string SegmentId { get; set; }

        [Required]
        [DisplayName("Sub Route")]
        public string SubRouteId { get; set; }

        public override string ToString()
        {
            return $"{BridgeNo} - {BridgeName}";
        }

        //[JsonIgnore]
        //public virtual Abutment Abutment { get; set; }

        //[JsonIgnore]
        //public virtual Ancillary Ancillary { get; set; }

        //[JsonIgnore]
        //public virtual SubRoute SubRoute { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<BridgeComment> BridgeComments { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<BridgeDoc> BridgeDocs { get; set; }

        //[JsonIgnore]
        //public virtual BridgeGeneralInfo BridgeGeneralInfo { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<BridgeImprovement> BridgeImprovements { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<BridgeMedia> BridgeMedias { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<BridgeObservation> BridgeObservations { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<DamageInspVisual> DamageInspVisuals { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<Pier> Piers { get; set; }

        //[JsonIgnore]
        //public virtual SuperStructure SuperStructure { get; set; }

        //[JsonIgnore]
        //public virtual Segment Segment { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<DamageInspMajor> DamageInspMajors { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<ResultInspMajor> ResultInspMajors { get; set; }
    }
}