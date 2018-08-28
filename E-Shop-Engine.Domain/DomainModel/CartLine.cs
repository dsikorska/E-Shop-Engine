namespace E_Shop_Engine.Domain.DomainModel
{
    public class CartLine : DbEntity
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int CartID { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
