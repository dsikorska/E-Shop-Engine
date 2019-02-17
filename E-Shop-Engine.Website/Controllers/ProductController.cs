using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Website.CustomFilters;
using E_Shop_Engine.Website.Extensions;
using E_Shop_Engine.Website.Models;
using E_Shop_Engine.Website.Models.Custom;
using NLog;
using X.PagedList;

namespace E_Shop_Engine.Website.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository, IMapper mapper) : base(mapper)
        {
            _productRepository = productRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        // GET: /Product/ProductsToPagedList
        [ResetDataDictionaries]
        public PartialViewResult ProductsToPagedList(IEnumerable<ProductViewModel> model, int? page)
        {
            string sortOrder = null;
            bool descending = false;
            if (SortingManager.SortOrder != null)
            {
                sortOrder = SortingManager.SortOrder;
                descending = SortingManager.IsSortDescending;
                SortingManager.SetSorting(sortOrder, descending);
            }

            IEnumerable<ProductViewModel> sortedModel = model.SortBy(x => x.Name, sortOrder, descending);
            int pageNumber = page ?? 1;
            IPagedList<ProductViewModel> viewModel = new PagedList<ProductViewModel>(sortedModel, pageNumber, 9);

            return PartialView("_ProductsDeck", viewModel);
        }

        // GET: /{id}/{name}
        [ReturnUrl]
        public ViewResult Details(int id)
        {
            Product model = _productRepository.GetById(id);
            ProductViewModel viewModel = _mapper.Map<ProductViewModel>(model);

            return View(viewModel);
        }

        // GET: /Product/Search/
        [ResetDataDictionaries]
        public ActionResult Search(int? page, string sortOrder, string search, bool descending = true)
        {
            ManageSearchingTermStatus(ref search);

            IEnumerable<Product> model = GetSearchingResult(search);

            if (model.Count() == 0)
            {
                model = _productRepository.GetAll();
            }

            IEnumerable<ProductViewModel> mappedModel = _mapper.Map<IEnumerable<ProductViewModel>>(model);
            IEnumerable<ProductViewModel> sortedModel = mappedModel.SortBy(x => x.Name, sortOrder, descending);

            int pageNumber = page ?? 1;
            IPagedList<ProductViewModel> viewModel = sortedModel.ToPagedList(pageNumber, 9);

            SaveSortingState(sortOrder, descending, search);

            return View("_ProductsDeck", viewModel);
        }

        [NonAction]
        private IEnumerable<Product> GetSearchingResult(string search)
        {
            IEnumerable<Product> resultName = _productRepository.GetProductsByName(search);
            IEnumerable<Product> resultCatalogNum = _productRepository.GetProductsByCatalogNumber(search);
            IEnumerable<Product> result = resultName.Union(resultCatalogNum).ToList();
            return result;
        }

        // GET: /Product/GetImage?id
        public FileContentResult GetImage(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product?.ImageData != null && product.ImageData.Length != 0)
            {
                return new FileContentResult(product.ImageData, product.ImageMimeType);
            }
            else
            {
                byte[] img = System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/default-img.jpg"));

                return new FileContentResult(img, "image/jpg");
            }
        }
    }
}