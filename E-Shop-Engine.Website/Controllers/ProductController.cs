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
        IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: Product
        public ActionResult Index()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            IEnumerable<ProductViewModel> model = products.Select(p => Mapper.Map<ProductViewModel>(p)).ToList();
            return View("_ProductsDeck", model);
        }

        [HttpGet]
        public ViewResult Details(int id, string returnUrl)
        {
            Product product = _productRepository.GetById(id);
            ProductViewModel model = Mapper.Map<ProductViewModel>(product);
            model.ReturnUrl = returnUrl;

            return View(model);
        }


        //TODO to Home
        [HttpGet]
        public PartialViewResult GetSpecialOffers()
        {
            IEnumerable<Product> product = _productRepository.GetAllSpecialOffers();
            IList<ProductViewModel> model = product.Select(p => Mapper.Map<ProductViewModel>(p)).ToList();
            return PartialView("SpecialOffers", model);
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffersInDeck()
        {
            IEnumerable<Product> product = _productRepository.GetAllShowingInDeck();
            IEnumerable<ProductViewModel> model = product.Select(p => Mapper.Map<ProductViewModel>(p)).ToList();
            return PartialView("_ProductsDeck", model);
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