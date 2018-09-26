using CandidateManager.Web.Builders;
using CandidateManager.Web.ViewModels;
using NUnit.Framework;
using System;
using System.Linq;

namespace CandidateManager.Test.Unit
{
    public class SessionPublishedEmailBuilderTest
    {
        [TestFixture]
        public class When_A_SessionPublishedEmailBuilder_Is_Present
        {
            private Guid _guid;
            private SessionViewModel _sessionViewModel;

            private ISessionPublishedEmailBuilder _sessionPublishedEmailBuilder;

            [SetUp]
            public void SetUp()
            {
                _guid = Guid.NewGuid();
                _sessionViewModel = new SessionViewModel
                {
                    Id = _guid,
                    CandidateId = 1,
                    ExerciseId = 1,
                    Candidate = new CandidateViewModel
                    {
                        Id = 1,
                        Name = "Test Candidate Name",
                        Surname = "Test Candidate Surname",
                        Email = "candidate@test.com"
                    },
                    Exercise = new ExerciseViewModel
                    {
                        Id = 1,
                        Name = "Test Exercise Name",
                        Description = "Test Exercise Description",
                        FileName = "Test Exercise FileName"
                    }
                };

                _sessionPublishedEmailBuilder = new SessionPublishedEmailBuilder(
                    "sender@test.com",
                    "subject - {Id}, {Candidate.Name}, {Candidate.FullName}, {Exercise.Name}",
                    @"body\n{Id}, {Candidate.Name}, {Candidate.FullName}, {Exercise.Name}");
            }

            [Test]
            public void It_Should_Build_An_Email_Message()
            {
                var result = _sessionPublishedEmailBuilder.Get(_sessionViewModel);

                Assert.AreEqual("sender@test.com", result.From.Address);
                Assert.AreEqual(_sessionViewModel.Candidate.Email, result.To.First().Address);
                Assert.AreEqual(string.Format("subject - {0}, {1}, {2}, {3}",
                    _sessionViewModel.Id,
                    _sessionViewModel.Candidate.Name, _sessionViewModel.Candidate.FullName,
                    _sessionViewModel.Exercise.Name), result.Subject);
                Assert.AreEqual(string.Format("body\n{0}, {1}, {2}, {3}",
                    _sessionViewModel.Id,
                    _sessionViewModel.Candidate.Name, _sessionViewModel.Candidate.FullName,
                    _sessionViewModel.Exercise.Name), result.Body);
            }
        }
    }
}
