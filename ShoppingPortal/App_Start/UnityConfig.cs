using Core.Interfaces;
using Core.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ShoppingPortal.Models;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace ShoppingPortal
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            // var accountInjectionConstructor = new InjectionConstructor(new IdentitySampleDbModelContext(configurationStore));

            #region asp.net idenity

            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<IAuthenticationManager>(
                             new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                        new InjectionConstructor(typeof(ApplicationDbContext)));


            #endregion
            //container.RegisterType<IMapper,Mapper>();

            container.RegisterType<IUserRepository, UserService>();
            container.RegisterType<ICategoriesRepository, CategoriesService>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
           
        }
    }
}