using System.Collections.Generic;
using E_Shop_Engine.Domain.DomainModel;

namespace E_Shop_Engine.Website.Models
{
    public class SubcategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string CategoryName { get; set; }
        public int CategoryId { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}