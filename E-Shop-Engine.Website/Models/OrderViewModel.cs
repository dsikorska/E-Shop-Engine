using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;
using E_Shop_Engine.Website.Models.DTO;

namespace E_Shop_Engine.Website.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }

        [Display(Name = "User")]
        public virtual AppUser AppUser { get; set; }

        [Display(Name = "Cart")]
        public virtual CartDTO Cart { get; set; }
        public decimal Payment { get; set; }

        [Display(Name = "Is Paid")]
        public bool IsPaid { get; set; } = false;

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Transaction Number")]
        public string TransactionNumber { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }

        public decimal? TotalValue
        {
            get
            {
                return Cart?.CartLines.Sum(x => x.Product.Price * x.Quantity);
            }
        }

        public int? TotalProducts
        {
            get
            {
                return Cart?.CartLines.Sum(x => x.Quantity);
            }
        }
    }
}