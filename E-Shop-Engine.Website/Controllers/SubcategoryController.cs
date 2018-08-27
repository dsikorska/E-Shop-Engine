using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;

namespace E_Shop_Engine.Website.Controllers
{
    public class SubcategoryController : Controller
    {
        IRepository<Subcategory> _subcategoryRepository;

        public SubcategoryController(IRepository<Subcategory> subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
        }

        [HttpGet]
        public ViewResult Details(int id)
        {
            return View(_subcategoryRepository.GetById(id));
        }
    }
}