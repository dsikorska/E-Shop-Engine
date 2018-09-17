using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.Interfaces;
using E_Shop_Engine.Website.Models;

namespace E_Shop_Engine.Website.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Product
        public ActionResult Index()
        {

            IQueryable<Product> model = _productRepository.GetAll();
            IEnumerable<ProductViewModel> viewModel = Mapper.Map<IQueryable<Product>, IEnumerable<ProductViewModel>>(model);
            return View("_ProductsDeck", viewModel);
        }

        [HttpGet]
        public ViewResult Details(int id)
        {

            Product model = _productRepository.GetById(id);
            ProductViewModel viewModel = Mapper.Map<ProductViewModel>(model);

            return View(viewModel);
        }

        [HttpGet]
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