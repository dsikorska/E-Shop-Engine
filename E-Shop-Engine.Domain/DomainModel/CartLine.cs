namespace E_Shop_Engine.Domain.DomainModel
{
    public class CartLine : DbEntity
    {
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; }
        public int Cart_Id { get; set; }
    }
}
