using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Shop_Engine.Domain.DomainModel.IdentityModel
{
    public class AppUser : IdentityUser
    {
        public DateTime Created { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual Address Address { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
