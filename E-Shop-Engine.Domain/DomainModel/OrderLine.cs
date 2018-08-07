namespace E_Shop_Engine.Domain.DomainModel
{
    public class OrderLine : DbEntity
    {
        public int Quantity { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }

        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
    }
}
