using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public SubcategoryAdminViewModel()
        {
            Categories = new Collection<Category>();
            Products = new Collection<Product>();
        }
    }
}