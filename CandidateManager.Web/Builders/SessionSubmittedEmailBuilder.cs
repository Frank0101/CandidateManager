using CandidateManager.Web.ViewModels;
using System.Net.Mail;

namespace CandidateManager.Web.Builders
{
    public class SessionSubmittedEmailBuilder : SessionEmailBuilder, ISessionSubmittedEmailBuilder
    {
        private readonly string _emailFrom;
        private readonly string _emailTo;
        private readonly string _emailSubject;
        private readonly string _emailBody;

        public SessionSubmittedEmailBuilder(string emailFrom, string emailTo,
            string emailSubject, string emailBody)
        {
            _emailFrom = emailFrom;
            _emailTo = emailTo;
            _emailSubject = emailSubject;
            _emailBody = emailBody;
        }

        public override MailMessage Get(SessionViewModel session)
        {
            return new MailMessage(_emailFrom, _emailTo)
            {
                Subject = ReplaceProperties(_emailSubject, session),
                Body = ReplaceProperties(_emailBody, session)
            };
        }
    }
}
