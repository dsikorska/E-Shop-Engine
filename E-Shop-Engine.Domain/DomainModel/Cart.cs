using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Cart : DbEntity
    {
        public ICollection<CartLine> CartLines { get; set; }
        public int? OrderID { get; set; }
        public virtual Order Order { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
