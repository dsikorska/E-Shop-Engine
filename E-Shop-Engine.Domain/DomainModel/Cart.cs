using System.Collections.Generic;
using System.Collections.ObjectModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Cart : DbEntity
    {
        public virtual ICollection<CartLine> CartLines { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Order Order { get; set; }
        public bool IsOrdered { get; set; }

        public Cart(AppUser user)
        {
            CartLines = new Collection<CartLine>();
            AppUser = user;
            IsOrdered = false;
        }

        public Cart()
        {
            IsOrdered = false;
            CartLines = new Collection<CartLine>();
        }
    }
}
