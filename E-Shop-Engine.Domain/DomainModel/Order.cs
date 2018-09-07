using System;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Order : DbEntity
    {
        public DateTime Created { get; set; }
        public DateTime Finished { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual Cart Cart { get; set; }
        public bool IsPaid { get; set; } = false;
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
