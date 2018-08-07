namespace E_Shop_Engine.Domain.DomainModel
{
    public class Product : DbEntity
    {
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int NumberInStock { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public int SubcategoryID { get; set; }
        public virtual Subcategory Subcategory { get; set; }
    }
}
