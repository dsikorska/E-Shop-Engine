using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Mvc;

namespace E_Shop_Engine.Website.Models
{
    public class CategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<SubcategoryViewModel> Subcategories { get; set; }
        public virtual ICollection<ProductViewModel> Products { get; set; }

        public CategoryViewModel()
        {
            Subcategories = new Collection<SubcategoryViewModel>();
            Products = new Collection<ProductViewModel>();
        }
    }
}