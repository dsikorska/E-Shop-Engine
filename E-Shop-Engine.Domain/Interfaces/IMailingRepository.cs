using System.Threading.Tasks;

namespace E_Shop_Engine.Domain.Interfaces
{
    public interface IMailingRepository
    {
        Task WelcomeMail(string mailTo);
        Task ActivationMail(string mailTo, string url);
        Task ResetPasswordMail(string mailTo, string url);
        Task PasswordChangedMail(string mailTo);
    }
}
