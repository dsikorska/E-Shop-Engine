using System;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Order : DbEntity
    {
        public DateTime Created { get; set; }
        public DateTime Finished { get; set; }
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
        public int CartID { get; set; }
        public virtual Cart Cart { get; set; }
        public bool IsPaid { get; set; } = false;
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
