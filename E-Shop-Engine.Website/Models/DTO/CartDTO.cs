using System.Collections.Generic;
using System.Collections.ObjectModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Website.Models.DTO
{
    public class CartDTO
    {
        public virtual ICollection<CartLineViewModel> CartLines { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual OrderViewModel Order { get; set; }
        public bool IsOrdered { get; set; }

        public CartDTO(AppUser user)
        {
            CartLines = new Collection<CartLineViewModel>();
            AppUser = user;
            IsOrdered = false;
        }

        public CartDTO()
        {
            IsOrdered = false;
            CartLines = new Collection<CartLineViewModel>();
        }
    }
}