using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using ERA_BMS.Models;
using System.ComponentModel.DataAnnotations;

namespace ERA_BMS.Areas.Admin.ViewModels
{
    public class UserRoleViewModel
    {
        public AspNetUser User { get; set; }
        public AspNetRole Role { get; set; }
    }

    public class RegisterUserViewModel
    {
        //I added this...
        [Required]
        [Display(Name = "User Role")]
        public string UserRoles { get; set; }

        //I added this...
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeUserPasswordViewModel
    {
        public string UserId;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}