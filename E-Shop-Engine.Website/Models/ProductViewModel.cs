namespace E_Shop_Engine.Website.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string CatalogNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
        public string CategoryName { get; set; }
        public string SubcategoryName { get; set; }
        public string ReturnUrl { get; set; }
    }
}