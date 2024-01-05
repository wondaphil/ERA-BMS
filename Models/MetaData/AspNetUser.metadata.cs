using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ERA_BMS.Models
{
    [MetadataType(typeof(AspNetUserMetadata))]
    public partial class AspNetUser
    {
    }

    public class AspNetUserMetadata
    {
        [Key]
        [DisplayName("User Id")]
        public string Id { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }
    } 
}