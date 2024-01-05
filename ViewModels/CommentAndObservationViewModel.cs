using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    public class CommentAndObservationViewModel
    {
        public List<string> Comment { get; set; }
        public BridgeObservation Observation { get; set; }
    }
}