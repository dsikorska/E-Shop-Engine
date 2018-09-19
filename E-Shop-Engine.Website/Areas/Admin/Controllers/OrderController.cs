using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Repositories;
using E_Shop_Engine.Website.Areas.Admin.Models;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly Repository<Order> _orderRepository;

        public OrderController(Repository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: Admin/Order
        public ActionResult Index()
        {
            IQueryable<Order> model = _orderRepository.GetAll().OrderBy(x => x.Created);
            IEnumerable<OrderAdminViewModel> viewModel = Mapper.Map<IQueryable<Order>, IEnumerable<OrderAdminViewModel>>(model);
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