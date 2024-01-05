using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ERA_BMS.Models;

namespace ERA_BMS.ViewModels
{
    // Bridge, Segment, Section, District ViewModel
    public class BSSDViewModel
    {
        public Bridge bridges { get; set; }
        public Segment segments { get; set; }
        public Section sections { get; set; }
        public District districts { get; set; }
    }
}