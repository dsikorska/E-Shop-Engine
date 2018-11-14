using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(IRoleStore<AppRole, string> store) : base(store)
        {

        }
    }
}
