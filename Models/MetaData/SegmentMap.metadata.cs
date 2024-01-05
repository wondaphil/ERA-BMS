﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ERA_BMS.Models
{
    [MetadataType(typeof(SegmentMapMetadata))]
    public partial class SegmentMap
    {
    }

    public class SegmentMapMetadata
    {
        [Key, ForeignKey("Segment"), Required]
        [DisplayName("Ser. No.")]
        public string SegmentId { get; set; }

        [DisplayName("Map File")]
        public string MapFile { get; set; }

        [DisplayName("Map File Path")]
        public string MapFilePath { get; set; }

        [DisplayName("Map Date")]
        public Nullable<System.DateTime> MapDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}