using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Cart
    {
        public ICollection<CartLine> Products { get; set; }
    }
}
