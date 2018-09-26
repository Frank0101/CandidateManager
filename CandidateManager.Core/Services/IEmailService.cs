using System.Net.Mail;

namespace CandidateManager.Core.Services
{
    public interface IEmailService
    {
        void SendEmail(MailMessage message);
    }
}
