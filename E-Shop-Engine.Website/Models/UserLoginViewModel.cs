using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    public class UserLoginViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}