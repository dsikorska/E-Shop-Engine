using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Category : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Subcategories = new Collection<Subcategory>();
            Products = new Collection<Product>();
        }
    }
}
