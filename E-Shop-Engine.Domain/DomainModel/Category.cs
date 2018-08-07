using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Category : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
