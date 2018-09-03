using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;

namespace E_Shop_Engine.Website.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Category
        [HttpGet]
        public ViewResult Index()
        {
            return View(_categoryRepository.GetAll());
        }

        // GET: Category - for navbar
        [HttpGet]
        public PartialViewResult NavList()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();
            IEnumerable<CategoryViewModel> model = categories.Select(p => Mapper.Map<CategoryViewModel>(p)).ToList();
            return PartialView("_Categories", model);
        }

        //TODO put product to partialview
        [HttpGet]
        public ViewResult Details(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryViewModel model = Mapper.Map<CategoryViewModel>(category);
            return View(model);
        }
    }
}