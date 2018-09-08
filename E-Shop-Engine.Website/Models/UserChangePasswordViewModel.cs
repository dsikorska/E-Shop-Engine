using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    public class UserChangePasswordViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "New Password")]
        [StringLength(200, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password does not match.")]
        [DataType(DataType.Password)]
        public string NewPasswordCopy { get; set; }
    }
}