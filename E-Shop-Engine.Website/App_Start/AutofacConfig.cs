using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Services.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;

[assembly: OwinStartup(typeof(E_Shop_Engine.Website.App_Start.AutofacConfig))]

namespace E_Shop_Engine.Website.App_Start
{
    public partial class AutofacConfig
    {
        public void Configuration(IAppBuilder app)
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<AppDbContext>().As<IAppDbContext>().InstancePerRequest();
            builder.RegisterType<AppDbContext>().AsSelf().InstancePerRequest();
            builder.RegisterType<AppSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
            //builder.RegisterType<AppUserStore>().As<IUserStore<AppUser>>().InstancePerRequest();
            builder.RegisterType<AppUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<AppRoleManager>().AsSelf().InstancePerRequest();
            builder.Register(c => new RoleStore<AppRole>(c.Resolve<AppDbContext>())).As<IRoleStore<AppRole, string>>().InstancePerRequest();
            builder.Register(c => new AppUserStore(c.Resolve<IAppDbContext>())).As<IUserStore<AppUser>>().InstancePerRequest();

            builder.RegisterGeneric(typeof(Repository<>)).As((typeof(IRepository<>))).InstancePerRequest();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerRequest();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerRequest();
            builder.RegisterType<SubategoryRepository>().As<ISubcategoryRepository>().InstancePerRequest();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest();
            builder.RegisterType<CartRepository>().As<ICartRepository>().InstancePerRequest();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>();
            builder.RegisterType<MailingRepository>().As<IMailingRepository>().InstancePerRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            IContainer container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);
        }
    }
}