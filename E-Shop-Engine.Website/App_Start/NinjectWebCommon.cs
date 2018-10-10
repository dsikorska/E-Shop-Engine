[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(E_Shop_Engine.Website.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(E_Shop_Engine.Website.App_Start.NinjectWebCommon), "Stop")]

namespace E_Shop_Engine.Website.App_Start
{
    using System;
    using System.Web;
    using E_Shop_Engine.Domain.DomainModel.IdentityModel;
    using E_Shop_Engine.Domain.Interfaces;
    using E_Shop_Engine.Services.Data;
    using E_Shop_Engine.Services.Data.Identity;
    using E_Shop_Engine.Services.Repositories;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            StandardKernel kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAppDbContext>().To<AppDbContext>().InRequestScope();
            kernel.Bind<IUserStore<AppUser>>().To<UserStore<AppUser>>().InRequestScope();
            kernel.Bind<AppUserManager>().ToSelf().InRequestScope();
            //kernel.Bind<AppRoleManager>().ToMethod(x => HttpContext.Current.GetOwinContext().GetUserManager<AppRoleManager>());
            kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication);
            kernel.Bind(typeof(IRepository<>)).To((typeof(Repository<>))).InRequestScope();
            kernel.Bind<IProductRepository>().To<ProductRepository>().InRequestScope();
            kernel.Bind<ICartRepository>().To<CartRepository>().InRequestScope();
            kernel.Bind<ISettingsRepository>().To<SettingsRepository>();
        }
    }
}
