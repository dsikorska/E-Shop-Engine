using System.Collections.Generic;
using System.Web;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Utilities;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class CreateProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int NumberInStock { get; set; }
        public string CatalogNumber { get; set; }
        public HttpPostedFileBase ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public bool ShowAsSpecialOffer { get; set; } = false;
        public bool ShowAtMainPage { get; set; } = false;
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }

        public static implicit operator CreateProductViewModel(Product product)
        {
            return new CreateProductViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                NumberInStock = product.NumberInStock,
                CatalogNumber = product.CatalogNumber,
                ImageData = ConvertPostedFile.ToHttpPostedFileBase(product.ImageData),
                ImageMimeType = product.ImageMimeType,
                ShowAsSpecialOffer = product.ShowAsSpecialOffer,
                ShowAtMainPage = product.ShowAtMainPage,
                CategoryId = product.CategoryID,
                SubcategoryId = product.SubcategoryID
            };
        }

        public static implicit operator Product(CreateProductViewModel viewModel)
        {
            return new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CatalogNumber = viewModel.CatalogNumber,
                CategoryID = viewModel.CategoryId,
                SubcategoryID = viewModel.SubcategoryId,
                ImageData = ConvertPostedFile.ToByteArray(viewModel.ImageData),
                ImageMimeType = viewModel.ImageMimeType,
                NumberInStock = viewModel.NumberInStock,
                Price = viewModel.Price,
                ShowAsSpecialOffer = viewModel.ShowAsSpecialOffer,
                ShowAtMainPage = viewModel.ShowAtMainPage
            };
        }
    }
}