using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Shop_Engine.Domain.DomainModel.IdentityModel
{
    public class AppUser : IdentityUser
    {
        public DateTime Created { get; set; }
    }
}
