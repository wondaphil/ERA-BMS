using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CulvertViewModel
    {
        public Culvert Culvert { get; set; }
        public CulvertGeneralInfo GenInfo { get; set; }
        public CulvertStructure CulvertStr { get; set; }
        public List<CulvertMedia> CulvertMedia { get; set; }
        public List<CulvertMedia> CulvertImage { get; set; }
    }
}