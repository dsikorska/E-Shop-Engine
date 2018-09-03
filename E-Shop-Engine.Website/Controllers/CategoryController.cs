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

        [HttpGet]
        public ViewResult Details(int id)
        {
            Category category = _categoryRepository.GetById(id);
            CategoryViewModel model = Mapper.Map<CategoryViewModel>(category);
            return View(model);
        }
    }
}