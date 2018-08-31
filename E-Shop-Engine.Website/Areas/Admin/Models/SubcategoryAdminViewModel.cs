using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class SubcategoryAdminViewModel
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int CategoryID { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string ReturnUrl { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public static implicit operator SubcategoryAdminViewModel(Subcategory model)
        {
            return new SubcategoryAdminViewModel
            {
                Name = model.Name,
                Description = model.Description,
                CategoryID = model.CategoryID,
                Id = model.ID,
                CategoryName = model.Category.Name,
                Products = model.Products

            };
        }

        public static implicit operator Subcategory(SubcategoryAdminViewModel model)
        {
            return new Subcategory
            {
                Name = model.Name,
                Description = model.Description,
                CategoryID = model.CategoryID,
                ID = model.Id
            };
        }
    }
}