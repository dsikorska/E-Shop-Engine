using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Services.Extensions
{
    public static class IdentityHelpers
    {
        public static IEnumerable<AppUser> FindUsersByEmail(this UserManager<AppUser> manager, string searchTerm)
        {
            return manager.Users.Where(x => x.Email.Contains(searchTerm)).Select(x => x);
        }

        public static IEnumerable<AppUser> FindUsersByName(this UserManager<AppUser> manager, string searchTerm)
        {
            return manager.Users.Where(x => x.Name.Contains(searchTerm)).Select(x => x);
        }

        public static IEnumerable<AppUser> FindUsersBySurname(this UserManager<AppUser> manager, string searchTerm)
        {
            return manager.Users.Where(x => x.Surname.Contains(searchTerm)).Select(x => x);
        }
    }
}
