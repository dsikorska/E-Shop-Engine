using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
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
    [ReturnUrl]
    [Authorize(Roles = "Administrators, Staff")]
    public class OrderAdminController : BaseController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMailingRepository _mailingRepository;

        public OrderAdminController(IOrderRepository orderRepository, IMailingRepository mailingRepository)
        {
            _orderRepository = orderRepository;
            _mailingRepository = mailingRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: Admin/Order
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

            IEnumerable<OrderAdminViewModel> mappedModel = Mapper.Map<IEnumerable<OrderAdminViewModel>>(model);
            IEnumerable<OrderAdminViewModel> sortedModel = PagedListHelper.SortBy(mappedModel, x => x.Created, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<OrderAdminViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending, search);

            return View(viewModel);
        }

        // GET: Admin/Order/Details?id
        public ActionResult Details(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderAdminViewModel model = Mapper.Map<OrderAdminViewModel>(order);

            return View(model);
        }

        // GET: Admin/Order/Edit?id
        [ChildActionOnly]
        public ViewResult Edit(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderAdminViewModel model = Mapper.Map<OrderAdminViewModel>(order);

            return View(model);
        }

        // POST: Admin/Order/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ChildActionOnly]
        public ActionResult Edit(OrderAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Order order = _orderRepository.GetById(model.Id);
            order.Finished = model.Finished;
            order.OrderStatus = model.OrderStatus;

            _orderRepository.Update(Mapper.Map<Order>(order));
            _mailingRepository.OrderChangedStatusMail(order.AppUser.Email, order.OrderNumber, order.OrderStatus.ToString(), "Order " + order.OrderNumber + " status updated");

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