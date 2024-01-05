using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;
using ERA_BMS.ViewModels;

namespace ERA_BMS.ViewModels
{
    public class InvalidInspectionDateViewModel
    {
        public Culvert Culverts { get; set; }
        public InvalidCulvertInspectionDate InvalidInspDate { get; set; }

    }
}