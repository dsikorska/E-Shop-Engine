using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using NLog;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Settings")]
    [Route("{action}")]
    [Authorize(Roles = "Administrators")]
    public class SettingsAdminController : BaseExtendedController
    {
        private readonly ISettingsRepository _settingsRepository;
        private readonly IMailingRepository _mailingRepository;

        public SettingsAdminController(
            ISettingsRepository settingsRepository,
            IMailingRepository mailingRepository,
            IUnitOfWork unitOfWork,
            IAppUserManager userManager)
            : base(unitOfWork, userManager)
        {
            _settingsRepository = settingsRepository;
            _mailingRepository = mailingRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Settings/Edit
        [ReturnUrl]
        public ActionResult Edit()
        {
            Settings model = _settingsRepository.Get();
            SettingsAdminViewModel viewModel = Mapper.Map<SettingsAdminViewModel>(model);
            return View(viewModel);
        }

        // POST: Admin/Settings/Edit
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(SettingsAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _mailingRepository.TestMail();
            _settingsRepository.Update(Mapper.Map<Settings>(model));
            _unitOfWork.SaveChanges();

            return Redirect("/Admin/Order/Index");
        }
    }
}