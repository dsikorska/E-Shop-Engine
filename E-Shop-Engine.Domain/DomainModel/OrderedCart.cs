using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class OrderedCart : DbEntity
    {
        public virtual ICollection<OrderedCartLine> CartLines { get; set; }
        public virtual Order Order { get; set; }
    }
}
