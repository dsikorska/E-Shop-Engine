using E_Shop_Engine.Domain.Abstract;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity.EntityFramework;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppUserStore : UserStore<AppUser>
    {
        public AppUserStore(IAppDbContext context) : base(context as AppDbContext)
        {
        }
    }
}
