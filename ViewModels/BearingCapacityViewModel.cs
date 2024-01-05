using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BearingCapacityViewModel
    {
        public Bridge Bridge { get; set; }
        public BridgeGeneralInfo GeneralInfo { get; set; }
        public BridgeDoc BridgeDoc { get; set; }

    }
}