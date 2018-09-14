namespace E_Shop_Engine.Domain.DomainModel
{
    public class OrderedCartLine : DbEntity
    {
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public virtual OrderedCart Cart { get; set; }
    }
}