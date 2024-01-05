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
    [MetadataType(typeof(AspNetRoleMetadata))]
    public partial class AspNetRole
    {
    }

    public class AspNetRoleMetadata
    {
        [Key]
        [DisplayName("Role Id")]
        public string Id { get; set; }

        [DisplayName("User Role")]
        public string Name { get; set; }
    } 
}