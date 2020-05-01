using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;
using ShoppingPortal.App_Start;
using ShoppingPortal.Models;

[assembly: OwinStartupAttribute(typeof(ShoppingPortal.Startup))]
namespace ShoppingPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultUser();
           
        }
        private void CreateDefaultUser() {
            try
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var Role = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var User = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (!Role.RoleExists("Admin"))
                {
                    //Create new role 
                    var createRole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    createRole.Name = "Admin";
                    Role.Create(createRole);

                    //Create User
                    var user = new ApplicationUser();
                    user.UserName = "admin@shoppingportal.com";
                    user.Email = "admin@shoppingportal.com";

                    var userPwd = "Admin@2020";
                    var chkUser = User.Create(user, userPwd);

                    if (chkUser.Succeeded)
                    {
                        //Assign Admin role to created user
                        User.AddToRole(user.Id, "Admin");
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
          
        }
    }
}
