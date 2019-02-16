using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data;
using E_Shop_Engine.Services.Data.Identity;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Services.Repositories;
using E_Shop_Engine.Services.Services.Payment.DotPay;
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

            // AutoMapper config
            builder.Register(c => AutoMapperConfig.Register()).As<IMapper>().InstancePerLifetimeScope().PropertiesAutowired().PreserveExistingDefaults();

            // DbContext config
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<AppDbContext>().As<IAppDbContext>().InstancePerRequest();
            builder.RegisterType<AppDbContext>().AsSelf().InstancePerRequest();

            // Identity config
            builder.RegisterType<AppSignInManager>().AsSelf().InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register<IDataProtectionProvider>(c => app.GetDataProtectionProvider()).InstancePerRequest();
            builder.RegisterType<AppUserManager>().As<IAppUserManager>().InstancePerRequest();
            builder.RegisterType<AppRoleManager>().AsSelf().InstancePerRequest();
            builder.Register(c => new RoleStore<AppRole>(c.Resolve<AppDbContext>())).As<IRoleStore<AppRole, string>>().InstancePerRequest();
            builder.Register(c => new AppUserStore(c.Resolve<IAppDbContext>())).As<IUserStore<AppUser>>().InstancePerRequest();

            // Types config
            builder.RegisterGeneric(typeof(Repository<>)).As((typeof(IRepository<>))).InstancePerRequest();
            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerRequest();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerRequest();
            builder.RegisterType<SubategoryRepository>().As<ISubcategoryRepository>().InstancePerRequest();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest();
            builder.RegisterType<CartRepository>().As<ICartRepository>().InstancePerRequest();
            builder.RegisterType<SettingsRepository>().As<ISettingsRepository>();
            builder.RegisterType<MailingService>().As<IMailingService>().InstancePerRequest();
            builder.RegisterType<DotPayPaymentService>().As<IDotPayPaymentService>().InstancePerRequest();

            // Register controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();

            // Set Dependency Resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            ConfigureAuth(app);
        }
    }
}