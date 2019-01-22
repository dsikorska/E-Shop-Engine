using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace E_Shop_Engine.Website.Models
{
    public class SubcategoryViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CategoryId { get; set; }

        public virtual ICollection<ProductViewModel> Products { get; set; }

        public SubcategoryViewModel()
        {
            Products = new Collection<ProductViewModel>();
        }
    }
}