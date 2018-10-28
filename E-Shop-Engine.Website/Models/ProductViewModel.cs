using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace E_Shop_Engine.Website.Models
{
    public class ProductViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Catalog Number")]
        public string CatalogNumber { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [Display(Name = "Subcategory")]
        public string SubcategoryName { get; set; }

        [Display(Name = "Availability")]
        public int NumberInStock { get; set; }
    }
}