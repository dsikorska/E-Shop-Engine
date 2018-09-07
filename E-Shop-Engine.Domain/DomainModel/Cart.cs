using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Cart : DbEntity
    {
        public ICollection<CartLine> CartLines { get; set; }

        public virtual Order Order { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
