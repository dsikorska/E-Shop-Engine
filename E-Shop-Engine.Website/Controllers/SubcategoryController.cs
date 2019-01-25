using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;
using NLog;

namespace E_Shop_Engine.Website.Controllers
{
    public class SubcategoryController : BaseController
    {
        private readonly IRepository<Subcategory> _subcategoryRepository;

        public SubcategoryController(IRepository<Subcategory> subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository;
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Category/{mainName}/{subcategoryName}/{subcategoryId}
        public ViewResult Details(int id, string sortOrder, bool descending = false)
        {
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SaveSortingState(sortOrder, descending);
            }
            Subcategory subcategory = _subcategoryRepository.GetById(id);
            SubcategoryViewModel model = Mapper.Map<SubcategoryViewModel>(subcategory);

            return View(model);
        }
    }
}