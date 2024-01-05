using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class AbandonedBridgeViewModel
    {
        public string AbandonedBridgeNo { get; set; }
        public List<string> NewBridgeNos { get; set; }
    }
}