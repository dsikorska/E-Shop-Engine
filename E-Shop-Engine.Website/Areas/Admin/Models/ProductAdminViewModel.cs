using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class ProductAdminViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(3900)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be higher than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Number In Stock")]
        [Range(0, int.MaxValue)]
        public int NumberInStock { get; set; }

        [Display(Name = "Catalog Number")]
        [StringLength(100)]
        public string CatalogNumber { get; set; }

        [Display(Name = "Image")]
        public HttpPostedFileBase ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] ImageBytes { get; set; }

        public string ImageMimeType { get; set; }

        [Display(Name = "Display product at main page as special offer?")]
        public bool ShowAsSpecialOffer { get; set; } = false;

        [Display(Name = "Display product at main page?")]
        public bool ShowAtMainPage { get; set; } = false;

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        [Display(Name = "Subcategory")]
        public int? SubcategoryId { get; set; }

        public string SubcategoryName { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Edited { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public ProductAdminViewModel()
        {
            Categories = new Collection<Category>();
        }
    }
}