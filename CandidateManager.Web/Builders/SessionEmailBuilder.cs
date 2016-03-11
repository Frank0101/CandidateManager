using CandidateManager.Web.ViewModels;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CandidateManager.Web.Builders
{
    public abstract class SessionEmailBuilder : ISessionEmailBuilder
    {
        public abstract MailMessage Get(SessionViewModel session);

        protected string ReplaceProperties(string target, SessionViewModel session)
        {
            target = target.Replace(@"\n", "\n");
            var parameterRegex = new Regex(@"{[a-zA-Z_]\w*(\.[a-zA-Z_]\w*)*}");
            foreach (Match match in parameterRegex.Matches(target))
            {
                var matchValue = match.Value.Substring(1, match.Value.Length - 2);
                object propertyValue = null;
                object propertyOwner = session;
                foreach (var propertyName in matchValue.Split('.'))
                {
                    var propertyInfo = propertyOwner.GetType().GetProperty(propertyName);
                    propertyValue = propertyInfo.GetValue(propertyOwner);
                    propertyOwner = propertyValue;
                }
                target = target.Replace(match.Value, propertyValue.ToString());
            }
            return target;
        }
    }
}
