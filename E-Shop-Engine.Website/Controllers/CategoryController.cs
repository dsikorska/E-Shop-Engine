using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public ViewResult Details(int id, string sortOrder, bool descending = false)
        {
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SaveSortingState(sortOrder, descending);
            }
            Category category = _categoryRepository.GetById(id);
            CategoryViewModel model = Mapper.Map<CategoryViewModel>(category);
            return View(model);
        }
    }
}