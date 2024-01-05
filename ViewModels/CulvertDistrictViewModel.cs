using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertDistrictViewModel
    {
        public District Districts { get; set; }
        public int CulvertCount { get; set; }
    }
}