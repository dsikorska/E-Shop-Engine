using System.Collections.Generic;

namespace E_Shop_Engine.Website.Models
{
    public class CartViewModel
    {
        public virtual IEnumerable<CartLineViewModel> CartLines { get; set; }

        public decimal TotalValue { get; set; }
    }
}