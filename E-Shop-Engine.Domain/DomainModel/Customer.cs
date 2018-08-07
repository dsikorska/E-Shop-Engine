using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Customer : DbEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }

        public int AddressID { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
