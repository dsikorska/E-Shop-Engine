using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public string ReturnUrl { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }

        public static implicit operator CategoryViewModel(Category model)
        {
            return new CategoryViewModel
            {
                Name = model.Name,
                Description = model.Description,
                Id = model.ID,
                Products = model.Products,
                Subcategories = model.Subcategories
            };
        }

        public static implicit operator Category(CategoryViewModel model)
        {
            return new Category
            {
                Name = model.Name,
                Description = model.Description,
                ID = model.Id
            };
        }
    }
}