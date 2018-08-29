﻿using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Areas.Admin.Models;

namespace E_Shop_Engine.Website.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // TODO: live refresh validation summary
        // GET: Admin/Category
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Category> model = _categoryRepository.GetAll();
            IEnumerable<CategoryViewModel> viewModel = model.Select(p => (CategoryViewModel)p).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id, string returnUrl)
        {
            CategoryViewModel model = _categoryRepository.GetById(id);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _categoryRepository.Update(model);

            return Redirect(model.ReturnUrl);
        }

        [HttpGet]
        public ViewResult Create()
        {
            CategoryViewModel model = new CategoryViewModel();
            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            _categoryRepository.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id, string returnUrl)
        {
            CategoryViewModel model = _categoryRepository.GetById(id);
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _categoryRepository.Delete(id);
            }
            catch (DbUpdateException e)
            {

            }

            return RedirectToAction("Index");
        }
    }
}