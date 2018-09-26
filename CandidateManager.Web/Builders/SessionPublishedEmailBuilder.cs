using CandidateManager.Web.ViewModels;
using System.Net.Mail;

namespace CandidateManager.Web.Builders
{
    public class SessionPublishedEmailBuilder : SessionEmailBuilder, ISessionPublishedEmailBuilder
    {
        private readonly string _emailFrom;
        private readonly string _emailSubject;
        private readonly string _emailBody;

        public SessionPublishedEmailBuilder(string emailFrom,
            string emailSubject, string emailBody)
        {
            _emailFrom = emailFrom;
            _emailSubject = emailSubject;
            _emailBody = emailBody;
        }

        public override MailMessage Get(SessionViewModel session)
        {
            return new MailMessage(_emailFrom, session.Candidate.Email)
            {
                Subject = ReplaceProperties(_emailSubject, session),
                Body = ReplaceProperties(_emailBody, session)
            };
        }
    }
}
