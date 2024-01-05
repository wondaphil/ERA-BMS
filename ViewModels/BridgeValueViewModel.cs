using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeValueViewModel
    {
        public Bridge Bridge { get; set; }
        public BridgeCurrentValue CurrentValue { get; set; }

    }
}