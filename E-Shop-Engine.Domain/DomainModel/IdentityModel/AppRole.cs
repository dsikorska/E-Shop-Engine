using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Shop_Engine.Domain.DomainModel.IdentityModel
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name) { }
    }
}
