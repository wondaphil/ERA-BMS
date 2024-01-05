using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgePriorityByAverageViewModel
    {
        [Key, ForeignKey("Bridge")]
        [DisplayName("Bridge Id")]
        public string BridgeId { get; set; }

        [DisplayName("Revised Bridge No")]
        public string RevisedBridgeNo { get; set; }

        [DisplayName("Bridge Name")]
        public string BridgeName { get; set; }

        [DisplayName("Inspection Year")]
        public int InspectionYear { get; set; }
        
        [DisplayName("Urgency Id")]
        public int UrgencyId { get; set; }

        [DisplayName("Insp. Recomm.")]
        public string InspRecommendation { get; set; }

        [DisplayName("Repair Cost")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public double? RepairCost  { get; set; }


        [DisplayName("Repair Cost %")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public double? RepairCostPerc  { get; set; }


        [DisplayName("Insp. Damage %")]
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        public double? DmgPerc  { get; set; }

        [DisplayName("Service Condition Id")]
        public double ServiceConditionId  { get; set; }

        [DisplayName("Service Condition")]
        public string ServiceCondition  { get; set; }

        [DisplayName("Priority Val. %")]
        public int? PriorityVal { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:#,0.00}")]
        [DisplayName("Avg. Damage %")]
        public double AverageDamage { get; set; }

        [DisplayName("Required Action Id")]
        public int RequiredActionId { get; set; }

        [DisplayName("Required Action")]
        public string RequiredAction { get; set; }
    }
}