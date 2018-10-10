namespace E_Shop_Engine.Domain.DomainModel
{
    public class Settings : DbEntity
    {
        public string ShopName { get; set; }
        public string Currency { get; set; }

        public string AdminEmailAddress { get; set; }
        public string NotificationReplyEmail { get; set; }
        public string SMTP { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPPort { get; set; }
        public bool? SMTPEnableSSL { get; set; }
    }
}
