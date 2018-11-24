using System.Collections.Generic;
using System.Linq;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Services.Extensions
{
    public static class IdentityHelpers
    {
        /// <summary>
        /// Get all AppUsers that email address contains search term.
        /// </summary>
        /// <param name="manager">User Manager.</param>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>AppUsers that email address contains search term.</returns>
        public static IEnumerable<AppUser> FindUsersByEmail(this UserManager<AppUser> manager, string searchTerm)
        {
            return manager.Users.Where(x => x.Email.Contains(searchTerm)).Select(x => x);
        }

        /// <summary>
        /// Get all AppUsers that name contains search term.
        /// </summary>
        /// <param name="manager">User Manager.</param>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>AppUsers that name contains search term.</returns>
        public static IEnumerable<AppUser> FindUsersByName(this UserManager<AppUser> manager, string searchTerm)
        {
            return manager.Users.Where(x => x.Name.Contains(searchTerm)).Select(x => x);
        }

        /// <summary>
        /// Get all AppUsers that surname contains search term.
        /// </summary>
        /// <param name="manager">User Manager.</param>
        /// <param name="searchTerm">Search term.</param>
        /// <returns>AppUsers that surname contains search term.</returns>
        public static IEnumerable<AppUser> FindUsersBySurname(this UserManager<AppUser> manager, string searchTerm)
        {
            return manager.Users.Where(x => x.Surname.Contains(searchTerm)).Select(x => x);
        }
    }
}
