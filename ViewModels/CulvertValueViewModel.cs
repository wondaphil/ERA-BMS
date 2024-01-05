using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertValueViewModel
    {
        public Culvert Culvert { get; set; }
        public CulvertCurrentValue CurrentValue { get; set; }

    }
}