using System.Collections.Generic;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Subcategory : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}