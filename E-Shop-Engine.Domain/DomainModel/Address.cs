using E_Shop_Engine.Domain.DomainModel.IdentityModel;

namespace E_Shop_Engine.Domain.DomainModel
{
    public class Address : DbEntity
    {
        public string Street { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public Customer Customer { get; set; }
    }
}
