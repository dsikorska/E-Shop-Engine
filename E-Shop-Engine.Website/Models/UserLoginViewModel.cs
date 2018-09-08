using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    public class UserLoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}