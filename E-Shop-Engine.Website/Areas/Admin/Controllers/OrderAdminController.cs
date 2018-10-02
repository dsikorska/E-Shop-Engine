using System;
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
    public class OrderAdminController : PagingBaseController
    {
        private readonly Repository<Order> _orderRepository;

        public OrderAdminController(Repository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            IQueryable<Order> model = _orderRepository.GetAll();
            IPagedList<OrderAdminViewModel> viewModel = IQueryableToPagedList<Order, OrderAdminViewModel, DateTime>(model, x => x.Created, pageNumber, 25, true);
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