using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace E_Shop_Engine.Website.Models
{
    public class CartViewModel
    {
        public virtual IEnumerable<CartLineViewModel> CartLines { get; set; }

        public decimal TotalValue { get; set; }

        public CartViewModel()
        {
            CartLines = new Collection<CartLineViewModel>();
        }
    }
}