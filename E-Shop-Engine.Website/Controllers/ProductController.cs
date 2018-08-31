using System;
using System.IO;
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
            return View("_ProductsDeck", _productRepository.GetAll());
        }

        //TODO
        [HttpGet]
        public ViewResult Details(int id, string returnUrl)
        {
            Product product = _productRepository.GetById(id);
            ProductViewModel model = Mapper.Map<ProductViewModel>(product);
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        //TODO switch domain model to viewmodel
        [HttpGet]
        public PartialViewResult GetSpecialOffers()
        {
            return PartialView("SpecialOffers", _productRepository.GetAllSpecialOffers());
        }

        [HttpGet]
        public PartialViewResult GetSpecialOffersInDeck()
        {
            return PartialView("_ProductsDeck", _productRepository.GetAllShowingInDeck());
        }

        [HttpGet]
        public FileContentResult GetImage(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product?.ImageData != null)
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