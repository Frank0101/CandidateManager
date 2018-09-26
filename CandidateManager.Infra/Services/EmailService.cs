using CandidateManager.Core.Services;
using System.Net.Mail;

namespace CandidateManager.Infra.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _host;
        private readonly int _port;

        public EmailService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void SendEmail(MailMessage message)
        {
            new SmtpClient
            {
                Host = _host,
                Port = _port
            }.Send(message);
        }
    }
}
