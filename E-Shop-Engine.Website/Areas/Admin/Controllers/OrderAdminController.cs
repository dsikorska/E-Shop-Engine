using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Services.Data.Identity.Abstraction;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Extensions;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Order")]
    [Route("{action}")]
    [Authorize(Roles = "Administrators, Staff")]
    public class OrderAdminController : BaseExtendedController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMailingRepository _mailingRepository;

        public OrderAdminController(
            IOrderRepository orderRepository,
            IMailingRepository mailingRepository,
            IUnitOfWork unitOfWork,
            IAppUserManager userManager,
            IMapper mapper)
            : base(unitOfWork, userManager, mapper)
        {
            _orderRepository = orderRepository;
            _mailingRepository = mailingRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Order
        [ReturnUrl]
        [ResetDataDictionaries]
        public ActionResult Index(int? page, string sortOrder, string search, bool descending = true, bool reversable = false)
        {
            ManageSearchingTermStatus(ref search);

            IEnumerable<Order> model = GetSearchingResult(search);

            if (model.Count() == 0)
            {
                model = _orderRepository.GetAll();
            }

            if (reversable)
            {
                ReverseSorting(ref descending, sortOrder);
            }

            IEnumerable<OrderAdminViewModel> mappedModel = _mapper.Map<IEnumerable<OrderAdminViewModel>>(model);
            IEnumerable<OrderAdminViewModel> sortedModel = mappedModel.SortBy(x => x.Created, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<OrderAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending, search);

            return View(viewModel);
        }

        // GET: Admin/Order/Details?id
        [ReturnUrl]
        public ActionResult Details(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderAdminViewModel model = _mapper.Map<OrderAdminViewModel>(order);

            return View(model);
        }

        // GET: Admin/Order/Edit?id
        [ReturnUrl]
        public ViewResult Edit(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderAdminViewModel model = _mapper.Map<OrderAdminViewModel>(order);

            return View(model);
        }

        // POST: Admin/Order/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Order order = _orderRepository.GetById(model.Id);
            order.Finished = model.Finished;
            order.OrderStatus = model.OrderStatus;

            _orderRepository.Update(_mapper.Map<Order>(order));
            _mailingRepository.OrderChangedStatusMail(order.AppUser.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order " + order.OrderNumber + " status updated");
            _unitOfWork.SaveChanges();

            return RedirectToAction("Index");
        }

        [NonAction]
        private IEnumerable<Order> GetSearchingResult(string search)
        {
            IEnumerable<Order> resultOrderNumber = _orderRepository.FindByOrderNumber(search);
            IEnumerable<Order> resultTransactionNumber = _orderRepository.FindByTransactionNumber(search);
            IEnumerable<Order> model = resultOrderNumber.Union(resultTransactionNumber).ToList();
            return model;
        }
    }
}