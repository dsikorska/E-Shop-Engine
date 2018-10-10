using System.ComponentModel.DataAnnotations;

namespace E_Shop_Engine.Website.Areas.Admin.Models
{
    public class SettingsAdminViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }

        public string Currency { get; set; }

        [Display(Name = "Admin Email Address")]
        public string AdminEmailAddress { get; set; }

        [Display(Name = "Notification Reply Email")]
        public string NotificationReplyEmail { get; set; }

        [Display(Name = "SMTP Server")]
        public string SMTP { get; set; }

        [Display(Name = "SMTP Username")]
        public string SMTPUsername { get; set; }

        [Display(Name = "SMTP Password")]
        public string SMTPPassword { get; set; }

        [Display(Name = "SMTP Port")]
        public string SMTPPort { get; set; }

        [Display(Name = "Enable SSL?")]
        public bool? SMTPEnableSSL { get; set; }
    }
}