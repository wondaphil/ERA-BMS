using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class BridgeImageListViewModel
    {
        public Bridge Bridge { get; set; }
        public List<BridgeMedia> ImagesList { get; set; }
    }
}