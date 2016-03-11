using CandidateManager.Core.Services;
using CandidateManager.Infra.Services;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Threading;

namespace CandidateManager.Test.Unit
{
    public class EmailServiceTest
    {
        [TestFixture]
        public class When_An_EmailService_Is_Present
        {
            private IEmailService _emailService;
            private Process _smtp4DevProcess;

            [SetUp]
            public void SetUp()
            {
                _emailService = new EmailService("localhost", 25);

                var smtp4DevPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    @"Tools\smtp4dev.exe");
                _smtp4DevProcess = Process.Start(smtp4DevPath);
                while (_smtp4DevProcess.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(1000);
                }
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

            [TearDown]
            public void TearDown()
            {
                _smtp4DevProcess.Kill();
            }
        }
    }
}
