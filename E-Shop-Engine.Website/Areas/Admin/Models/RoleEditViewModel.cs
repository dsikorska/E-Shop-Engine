using System.Collections.Generic;
using System.Collections.ObjectModel;
using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class RoleEditViewModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }

        public RoleEditViewModel()
        {
            Members = new Collection<AppUser>();
            NonMembers = new Collection<AppUser>();
        }
    }
}