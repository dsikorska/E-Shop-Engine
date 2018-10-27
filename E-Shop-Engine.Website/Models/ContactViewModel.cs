using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Models
{
    public class ContactViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(5000)]
        public string Message { get; set; }
    }
}