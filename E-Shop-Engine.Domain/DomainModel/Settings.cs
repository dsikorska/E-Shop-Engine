namespace E_Shop_Engine.Domain.DomainModel
{
    public class Settings : DbEntity
    {
        public string ShopName { get; set; }
        public string Currency { get; set; }

        public string ContactEmailAddress { get; set; }
        public string NotificationReplyEmail { get; set; }
        public string SMTP { get; set; }

        public int SMTPPort { get; set; }
        public bool SMTPEnableSSL { get; set; }

        public string DotPayId { get; set; }
        public bool IsDotPaySandbox { get; set; } = true;
    }
}
