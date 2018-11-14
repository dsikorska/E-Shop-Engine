using System.Security.Claims;
using System.Threading.Tasks;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace E_Shop_Engine.Services.Data.Identity
{
    public class AppSignInManager : SignInManager<AppUser, string>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(AppUser user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }
    }
}
