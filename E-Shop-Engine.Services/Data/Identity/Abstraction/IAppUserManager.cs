using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;
using Microsoft.AspNet.Identity;

namespace E_Shop_Engine.Services.Data.Identity.Abstraction
{
    public interface IAppUserManager
    {
        IIdentityValidator<string> PasswordValidator { get; set; }
        IPasswordHasher PasswordHasher { get; set; }
        IIdentityValidator<AppUser> UserValidator { get; set; }
        IQueryable<AppUser> Users { get; }

        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        Task<bool> CheckPasswordAsync(AppUser user, string password);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType);
        Task<IdentityResult> DeleteAsync(AppUser user);
        Task<AppUser> FindAsync(string userName, string password);
        Task<AppUser> FindByEmailAsync(string email);
        Task<AppUser> FindByIdAsync(string userId);
        AppUser FindById(string userid);
        Task<AppUser> FindByNameAsync(string userName);
        IEnumerable<AppUser> FindUsersByEmail(string term);
        IEnumerable<AppUser> FindUsersByName(string term);
        IEnumerable<AppUser> FindUsersBySurname(string term);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<IdentityResult> UpdateAsync(AppUser user);
    }
}
