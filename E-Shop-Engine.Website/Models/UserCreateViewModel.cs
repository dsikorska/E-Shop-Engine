using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    //TODO add address
    public class UserCreateViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Surname { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 6)]
        public string Password { get; set; }
    }
}