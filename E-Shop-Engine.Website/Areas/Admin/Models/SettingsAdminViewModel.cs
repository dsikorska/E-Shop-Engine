using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class SettingsAdminViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }

        public string Currency { get; set; }

        [Display(Name = "Contact Email Address")]
        public string ContactEmailAddress { get; set; }

        [Display(Name = "Notification Reply Email")]
        public string NotificationReplyEmail { get; set; }

        [Display(Name = "SMTP Server")]
        public string SMTP { get; set; }

        [Display(Name = "SMTP Port")]
        public string SMTPPort { get; set; }

        [Display(Name = "Enable SSL?")]
        public bool? SMTPEnableSSL { get; set; }

        [Display(Name = "DotPay Id")]
        public string DotPayId { get; set; }

        [Display(Name = "Enable Test Mode for DotPay?")]
        public bool IsDotPaySandbox { get; set; }
    }
}