using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Website.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual OrderedCart OrderedCart { get; set; }
        public bool IsPaid { get; set; } = false;

        [Required]
        [Display(Name = "Payment Method")]
        public PaymentMethod? PaymentMethod { get; set; } = null;
        public OrderStatus OrderStatus { get; set; } = OrderStatus.InProgress;

        public decimal? TotalValue
        {
            get
            {
                return OrderedCart?.CartLines.Sum(x => x.Product.Price * x.Quantity);
            }
        }

        public int? TotalProducts
        {
            get
            {
                return OrderedCart?.CartLines.Sum(x => x.Quantity);
            }
        }
    }
}