using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Services.Services;
using E_Shop_Engine.Website.Models;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository, IMapper mapper) : base(mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Category/{name}/{id}
        public ViewResult Details(int id, string sortOrder, bool descending = false)
        {
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SaveSortingState(sortOrder, descending);
            }
            Category category = _categoryRepository.GetById(id);
            CategoryViewModel model = _mapper.Map<CategoryViewModel>(category);

            if (model == null)
            {
                return View("_Error", new string[] { GetErrorMessage.ItemNotFound });
            }

            return View(model);
        }
    }
}