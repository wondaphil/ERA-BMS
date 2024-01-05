using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(BridgePriorityByInterventionRequirementMetadata))]
    public partial class BridgePriorityByInterventionRequirement
    {
        //private BMSEntities db = new BMSEntities();

        //public string RequiredActionName
        //{
        //    get
        //    {
        //        return db.RequiredActions.Find(RequiredActionId).RequiredActionName;
        //    }
        //}
    }
    public class BridgePriorityByInterventionRequirementMetadata
    {
        [Key, ForeignKey("Bridge")]
        [DisplayName("Ser. No.")]
        public string BridgeId { get; set; }
        
        [DisplayName("Revised Bridge Id")]
        public string RevisedBridgeNo { get; set; }

        [DisplayName("Bridge Name")]
        public string BridgeName { get; set; }

        [DisplayName("District Id")]
        public string DistrictId { get; set; }

        [DisplayName("District")]
        public string DistrictName { get; set; }

        [DisplayName("Section Id")]
        public string SectionId { get; set; }

        [DisplayName("Section")]
        public string SectionName { get; set; }

        [DisplayName("Segment Id")]
        public string SegmentId { get; set; }

        [DisplayName("Segment")]
        public string SegmentName { get; set; }

        [DisplayName("Inspection Year")]
        public int InspectionYear { get; set; }

        [DisplayName("Repair Cost")]
        public Nullable<double> RepairCost { get; set; }
        
        [DisplayName("Dmg %")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> DmgPerc { get; set; }

        [DisplayName("DmgPercWt")]
        public Nullable<int> DmgPercWt { get; set; }

        [DisplayName("Urgency Id")]
        public Nullable<int> UrgencyId { get; set; }

        [DisplayName("Intervention Type")]
        public string UrgencyName { get; set; }
        
        [DisplayName("UrgencyWt")]
        public Nullable<int> UrgencyWt { get; set; }

        [DisplayName("Water Way Adequacy")]
        public Nullable<bool> WaterWayAdequacy { get; set; }

        [DisplayName("WaterWayWt")]
        public Nullable<int> WaterWayWt { get; set; }

        [DisplayName("Road Class Id")]
        public Nullable<int> RoadClassId { get; set; }

        [DisplayName("Road Class")]
        public string RoadClassName { get; set; }
        
        [DisplayName("RoadClassWt")]
        public Nullable<int> RoadClassWt { get; set; }

        [DisplayName("Structure Length (m)")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public Nullable<double> BridgeLength { get; set; }

        [DisplayName("BridgeLengthWt")]
        public int BridgeLengthWt { get; set; }

        [DisplayName("Constr. / Replaced Year")]
        public Nullable<int> ConstructionYear { get; set; }

        [DisplayName("ConstructionYearWt")]
        public int ConstructionYearWt { get; set; }

        [DisplayName("Detour Possible")]
        public Nullable<bool> DetourPossible { get; set; }

        [DisplayName("DetourWt")]
        public int DetourWt { get; set; }

        [DisplayName("ADT")]
        public Nullable<double> ADT { get; set; }

        [DisplayName("TrafficWt")]
        public Nullable<int> TrafficWt { get; set; }

        [DisplayName("Priority %")]
        public Nullable<int> PriorityVal { get; set; }

        [DisplayName("Required Action Id")]
        public Nullable<int> RequiredActionId { get; set; }
    }
}