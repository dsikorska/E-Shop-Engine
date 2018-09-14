namespace E_Shop_Engine.Website.Models
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }
}