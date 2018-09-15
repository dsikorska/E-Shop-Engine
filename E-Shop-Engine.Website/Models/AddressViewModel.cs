using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace E_Shop_Engine.Website.Models
{
    public class AddressViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2)]
        public string Street { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "Building Number")]
        public string Line1 { get; set; }

        [StringLength(100)]
        [Display(Name = "Flat Number")]
        public string Line2 { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2)]
        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100, MinimumLength = 2)]
        public string Country { get; set; }
    }
}