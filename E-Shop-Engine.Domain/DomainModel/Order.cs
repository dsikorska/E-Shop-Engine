using System;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Order : DbEntity
    {
        public string OrderNumber { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual Cart Cart { get; set; }
        public decimal Payment { get; set; }

        public bool IsPaid { get; set; } = false;
        public string PaymentMethod { get; set; }
        public string TransactionNumber { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public Order()
        {

        }

        public Order(AppUser user, Cart cart, string paymentMethod)
        {
            AppUser = user;
            Cart = cart;
            PaymentMethod = paymentMethod;
            Created = DateTime.UtcNow;
            OrderStatus = OrderStatus.WaitingForPayment;
        }
    }
}
