using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    public class UserCreateViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Phone]
        [Display(Name = "Phone Number")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200, MinimumLength = 6)]
        public string Password { get; set; }
    }
}