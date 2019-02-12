using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.CustomFilters;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class PaymentController : BaseExtendedController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private static Settings settings;
        private readonly IMailingRepository _mailingRepository;
        private readonly IPaymentTransactionRepository _transactionRepository;

        public PaymentController(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ISettingsRepository settingsRepository,
            IMailingRepository mailingRepository,
            IPaymentTransactionRepository transactionRepository,
            IAppUserManager userManager,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            settings = settingsRepository.Get();
            _mailingRepository = mailingRepository;
            _transactionRepository = transactionRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [Authorize]
        [ReturnUrl]
        public ActionResult Select()
        {
            return View();
        }

        [Authorize]
        public ActionResult Process(string paymentMethod)
        {
            //TODO
            string path = Path.Combine(HostingEnvironment.MapPath("/"), "Plugins");
            IEnumerable<string> plugins = Directory.EnumerateDirectories(path);
            string pluginPath = plugins.First(x => x.Contains("Payment." + paymentMethod));

            if (pluginPath != null)
            {
                CSharpCodeProvider provider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters()
                {
                    GenerateExecutable = false,
                    GenerateInMemory = false,
                    TempFiles = new TempFileCollection(HostingEnvironment.MapPath("/") + @"Plugins"),
                    OutputAssembly = HostingEnvironment.MapPath("/") + @"Plugins\Compiled"
                };
                parameters.ReferencedAssemblies.AddRange(new string[]
                {
                    "System.dll",
                    "System.Net.dll",
                    "System.Web.dll",
                    @"D:\Program Files\Projekty\E-Shop-Engine\packages\Microsoft.AspNet.Mvc.5.2.6\lib\net45\System.Web.Mvc.dll",
                    HostingEnvironment.MapPath("/") + @"bin\E-Shop-Engine.Domain.dll",
                    HostingEnvironment.MapPath("/") + @"bin\E-Shop-Engine.Services.dll",
                    HostingEnvironment.MapPath("/") + @"bin\E-Shop-Engine.Utilities.dll",
                    HostingEnvironment.MapPath("/") + @"bin\E-Shop-Engine.Website.dll",
                    @"D:\Program Files\Projekty\E-Shop-Engine\packages\AutoMapper.7.0.1\lib\net45\AutoMapper.dll",
                    @"D:\Program Files\Projekty\E-Shop-Engine\packages\Newtonsoft.Json.11.0.2\lib\net45\Newtonsoft.Json.dll",
                    @"D:\Program Files\Projekty\E-Shop-Engine\packages\NLog.4.5.10\lib\net45\NLog.dll",
                    @"D:\Program Files\Projekty\E-Shop-Engine\E-Shop-Engine.Website\bin\Microsoft.AspNet.Identity.EntityFramework.dll"
                });

                string p = Path.Combine(pluginPath, @"Controllers\TransactionController.cs");
                List<string> files = Directory.EnumerateFiles(pluginPath + @"\Models").ToList();
                files.Add(p);
                CompilerResults results = provider.CompileAssemblyFromFile(parameters, files.ToArray());

                if (results.Errors.HasErrors)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (CompilerError error in results.Errors)
                    {
                        sb.AppendLine(string.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                    }

                    throw new InvalidOperationException(sb.ToString());
                }
            }

            //AppUser user = GetCurrentUser();
            //Cart cart = _cartRepository.GetCurrentCart(user);
            //Order order = new Order(user, cart, PaymentMethod.Dotpay);
            //_orderRepository.Create(order);
            //_cartRepository.SetCartOrdered(cart);
            //_cartRepository.NewCart(user);
            //_unitOfWork.SaveChanges();

            return null;
        }
    }
}