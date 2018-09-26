using CandidateManager.Core.Services;
using CandidateManager.Infra.Services;
using NUnit.Framework;
using System.Net.Mail;

namespace CandidateManager.Test.Unit
{
    public class EmailServiceTest
    {
        [TestFixture]
        public class When_An_EmailService_Is_Present
        {
            private IEmailService _emailService;

            [SetUp]
            public void SetUp()
            {
                _emailService = new EmailService("localhost", 25);
            }

            [Test]
            public void It_Should_Send_An_Email()
            {
                var message = new MailMessage("sender@test.com", "recipient@test.com")
                {
                    Subject = "test message subject",
                    Body = "test message body"
                };

                Assert.DoesNotThrow(() => _emailService.SendEmail(message));
            }
        }
    }
}
