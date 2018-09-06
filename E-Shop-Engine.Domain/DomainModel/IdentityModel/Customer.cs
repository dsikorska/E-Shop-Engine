using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel.IdentityModel
{
    public class Customer : DbEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
