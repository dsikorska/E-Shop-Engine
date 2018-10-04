using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Repositories;
using E_Shop_Engine.Website.Areas.Admin.Models;
using E_Shop_Engine.Website.Controllers;
using X.PagedList;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    [RouteArea("Admin", AreaPrefix = "Admin")]
    [RoutePrefix("Order")]
    [Route("{action}")]
    public class OrderAdminController : BaseController
    {
        private readonly Repository<Order> _orderRepository;

        public OrderAdminController(Repository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: Admin/Order
        public ActionResult Index(int? page, string sortOrder, bool descending = true)
        {
            ReverseSorting(ref descending, sortOrder);
            IQueryable<Order> model = _orderRepository.GetAll();
            IEnumerable<OrderAdminViewModel> mappedModel = SortBy<Order, OrderAdminViewModel>(model, "Created", sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<OrderAdminViewModel> viewModel = mappedModel.ToPagedList(pageNumber, 25);

            SaveSortingState(sortOrder, descending);

            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderAdminViewModel model = Mapper.Map<OrderAdminViewModel>(order);

            return View(model);
        }

        public ViewResult Edit(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderAdminViewModel model = Mapper.Map<OrderAdminViewModel>(order);

            return View(model);
        }

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
            order.IsPaid = model.IsPaid;
            order.OrderStatus = model.OrderStatus;

            _orderRepository.Update(Mapper.Map<Order>(order));

            return RedirectToAction("Index");
        }
    }
}