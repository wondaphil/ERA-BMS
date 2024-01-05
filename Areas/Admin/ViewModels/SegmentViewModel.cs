using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using ERA_BCMS.Models;

namespace ERA_BCMS.Areas.Admin.ViewModel
{
    public class SegmentViewModel
    {
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Remark { get; set; }

        public virtual Section Section { get; set; }
        public List<District> Districts { get; set; }
    }
}