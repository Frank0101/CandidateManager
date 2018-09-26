using CandidateManager.Web.ViewModels;
using System.Net.Mail;

namespace CandidateManager.Web.Builders
{
    public interface ISessionEmailBuilder
    {
        MailMessage Get(SessionViewModel session);
    }
}
