using System;
using System.Collections.Generic;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Order : DbEntity
    {
        public DateTime CreatedDate { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
