using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class RoleModificationViewModel
    {
        [Required]
        public string RoleName { get; set; }

        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}