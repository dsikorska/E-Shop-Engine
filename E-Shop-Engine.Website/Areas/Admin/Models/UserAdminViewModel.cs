using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class UserAdminViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Surname { get; set; }

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual Address Address { get; set; }

        public UserAdminViewModel()
        {
            Orders = new Collection<Order>();
        }
    }
}