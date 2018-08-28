using System.Web.Mvc;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: Cart
        public ActionResult Details(int id)
        {
            return View(_cartRepository.GetById(id));
        }
    }
}