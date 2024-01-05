using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity; using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ERA_BMS.Areas.Admin.ViewModels;
using ERA_BMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ERA_BMS.Areas.Admin.Controllers
{
    //Only Admins should have access
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private BMSEntities db;
        //private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserManagementController()
        {
            db = new BMSEntities();
        }

        public UserManagementController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        public enum StatusMessageId
        {
            AddUserSuccess,
            ChangePasswordSuccess,
            RemoveUserSuccess,
            Error
        }

        // GET: Admin/UserManagement
        public ActionResult Index(StatusMessageId? messageId)
        {
            ViewBag.StatusMessage =
                messageId == StatusMessageId.AddUserSuccess ? "New user has been added successfully."
                : messageId == StatusMessageId.ChangePasswordSuccess ? "Your password has been changed successfully."
                : messageId == StatusMessageId.RemoveUserSuccess ? "User has been removed successfully."
                : messageId == StatusMessageId.Error ? "An error has occurred and operation is unsuccessful."
                : "";

            var usersList = (from users in db.AspNetUsers
                             select new UserRoleViewModel {
                                  User = users,
                                  Role = users.AspNetRoles.FirstOrDefault()
                             }).ToList();

            return View(usersList);
        }


        //
        // GET: /Admin/UserManagement/New
        [AllowAnonymous]
        public ActionResult New()
        {
            // For user registration, "Admin" role will not be displayed. 
            // Admin can select rest of any role type during registration. 
            ViewBag.Roles = new SelectList(db.AspNetRoles.Where(u => !u.Name.Contains("Admin"))
                                    .ToList(), "Name", "Name");

            return View();
        }

        //
        // POST: /Admin/UserManagement/New
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> New(RegisterUserViewModel model)
        public ActionResult New(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    UserManager.AddToRole(user.Id, model.UserRoles);

                    return RedirectToAction("Index", "UserManagement", new { message = StatusMessageId.AddUserSuccess });
                }
                ViewBag.Roles = new SelectList(db.AspNetRoles.Where(u => !u.Name.Contains("Admin"))
                                  .ToList(), "Name", "Name");

                AddErrors(result);
            }

            // If execution reaches this far, something failed. So redisplay form
            return View(model);
        }

        public string UpdateUserName(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return value;

            // Check if value is a valid user name
            if (value != "" && value.All(Char.IsLetter))
            {
                AspNetUser users = db.AspNetUsers.Find(id);

                users.UserName = value;
                db.SaveChanges();

                return value;
            }

            return origvalue;
        }

        public string UpdateEmail(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return value;

            // Check if value is a valid email address
            if (new EmailAddressAttribute().IsValid(value))
            {
                UserManager.SetEmail(id, value);
                
                return value;
            }

            return origvalue;
        }

        public string UpdateUserRole(string id, string origvalue, string value)
        {
            // No change
            if (origvalue == value)
                return value;

            // First remove the user from his current role
            UserManager.RemoveFromRole(id, origvalue);

            // Then add the user to a new role
            UserManager.AddToRole(id, value);

            return value;
        }

        public JsonResult SelectUserRoles()
        {
            // Get a list of all users except "Admin"
            List<AspNetRole> userroles = db.AspNetRoles.Where(u => !u.Name.Contains("Admin")).ToList();
            var list = userroles.Select(d => new[] { d.Name, d.Name });

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Admin/UserManagement/ChangePassword
        public ActionResult ChangePassword(string id)
        {
            ViewBag.UserId = id;

            return View();
        }

        //
        // POST: /Admin/UserManagement/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangePassword([Bind(Include = "UserId,OldPassword,NewPassword,ConfirmPassword")] ChangeUserPasswordViewModel vmodel)
        public async Task<ActionResult> ChangePassword(FormCollection formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }
            var result = await UserManager.ChangePasswordAsync(formData["UserId"], formData["OldPassword"], formData["NewPassword"]);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(formData["UserId"]);
                if (user != null)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "UserManagement", new { message = StatusMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(formData);
        }

        // GET: Admin/UserManagement/Delete/26c9f10a-7983-45cc-9fe4-35ab07365ba1
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "UserManagement", new { message = StatusMessageId.Error });
            }

            var userToDelete = db.AspNetUsers.Where(u => u.Id == id).Select(user => new UserRoleViewModel
            {
                User = user,
                Role = user.AspNetRoles.FirstOrDefault()
            }).FirstOrDefault();
                        
            // If current user is "Admin, do NOT delete it and return to home
            if (userToDelete.Role.Name == "Admin")
            {
                return RedirectToAction("Index", "UserManagement");
            }

            if (userToDelete == null)
            {
                return RedirectToAction("Index", "UserManagement", new { message = StatusMessageId.Error });
            }

            return View(userToDelete);
        }

        // POST: Admin/UserManagement/Delete/26c9f10a-7983-45cc-9fe4-35ab07365ba1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser user = db.AspNetUsers.Find(id);

            db.AspNetUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index", "UserManagement", new { message = StatusMessageId.RemoveUserSuccess });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
