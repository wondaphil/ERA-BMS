using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class DamageByStructureItemByDamgeTypeViewModel
    {
        public int StructureItemId { get; set; }
        public string StructureItemName { get; set; }
        public int DamageTypeId { get; set; }
        public string DamageTypeName { get; set; }
        public int BridgeCount { get; set; }
    }
}