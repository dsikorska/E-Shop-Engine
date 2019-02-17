using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using Microsoft.AspNet.Identity;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class BaseExtendedController : BaseController
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAppUserManager _userManager;

        public BaseExtendedController(
            IUnitOfWork unitOfWork,
            IAppUserManager userManager,
            IMapper mapper)
            : base(mapper)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [NonAction]
        protected AppUser GetCurrentUser()
        {
            string userId = ControllerContext.HttpContext.User.Identity.GetUserId();
            AppUser user = _userManager.FindById(userId);
            return user;
        }
    }
}