using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(RequiredActionMetadata))]
    public partial class RequiredAction
    {
    }
    public class RequiredActionMetadata
    {
        [Key]
        [DisplayName("Ser No")]
        public short RequiredActionId { get; set; }

        [DisplayName("Required Action")]
        public string RequiredActionName { get; set; }

        [DisplayName("From")]
        public Nullable<double> ValueFrom { get; set; }

        [DisplayName("To")]
        public Nullable<double> ValueTo { get; set; }

        [DisplayName("Type A1")]
        public string Remark { get; set; }
    }
}