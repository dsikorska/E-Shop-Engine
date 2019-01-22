using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class CategoryAdminViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Subcategory> Subcategories { get; set; }

        public CategoryAdminViewModel()
        {
            Products = new Collection<Product>();
            Subcategories = new Collection<Subcategory>();
        }
    }
}