using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    //TODO use viewmodel instead of appuser
    public class UserViewModel
    {
        //TODO dont use name
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}