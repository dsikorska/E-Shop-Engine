using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using E_Shop_Engine.Domain.Enumerables;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class OrderAdminViewModel
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Finished { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual OrderedCart OrderedCart { get; set; }

        public bool IsPaid { get; set; } = false;

        [Required]
        public OrderStatus OrderStatus { get; set; }

        public decimal? TotalValue
        {
            get
            {
                return OrderedCart?.CartLines.Sum(x => x.Product.Price * x.Quantity);
            }
        }
    }
}