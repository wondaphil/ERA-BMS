using Microsoft.Owin;
using Owin;

//I added these namespaces
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ERA_BCMS.Models;

[assembly: OwinStartupAttribute(typeof(ERA_BCMS.Startup))]
namespace ERA_BCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //createRolesandUsers(); // I added this 
        }

        // In this method we will create default User roles and Admin user for login    
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup create the first Admin Role and create a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Create a Admin super user who will maintain the website                   
                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin.bms@era.gov.et";

                string userPWD = "Admin#1";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Data Clerk role     
            if (!roleManager.RoleExists("Data Clerk"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Data Clerk";
                roleManager.Create(role);

            }

            // creating Creating Guest role     
            if (!roleManager.RoleExists("Guest"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Guest";
                roleManager.Create(role);

            }

            //// creating Creating Temp role     
            //if (!roleManager.RoleExists("Temp"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "Temp";
            //    roleManager.Create(role);

            //}
        }
    }
}
