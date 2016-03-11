using CandidateManager.Core.Models;
using CandidateManager.Core.Services;
using CandidateManager.Infra.Services;
using NUnit.Framework;
using System;

namespace CandidateManager.Test.Unit
{
    public class AssessmentStatusServiceTest
    {
        [TestFixture]
        public class When_An_AssessmentStatusService_Is_Present
        {
            private IAssessmentStatusService _assessmentStatusService;

            [SetUp]
            public void SetUp()
            {
                _assessmentStatusService = new AssessmentStatusService();
            }

            [Test]
            public void It_Should_Detect_An_Unavailable_Assessment()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Created
                });

                Assert.AreEqual(AssessmentStatus.Unavailable, result);
            }

            [Test]
            public void It_Should_Detect_An_Out_Of_Range_Assessment()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Published,
                    AvailableFrom = DateTime.Now.AddDays(-1),
                    AvailableTo = DateTime.Now.AddHours(-1)
                });

                Assert.AreEqual(AssessmentStatus.OutOfRange, result);
            }

            [Test]
            public void It_Should_Detect_An_Available_Assessment()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Published,
                    AvailableFrom = DateTime.Now.AddDays(-1),
                    AvailableTo = DateTime.Now.AddDays(1),
                });

                Assert.AreEqual(AssessmentStatus.Available, result);
            }

            [Test]
            public void It_Should_Detect_A_Started_Assessment()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Started,
                    AvailableFrom = DateTime.Now.AddDays(-1),
                    AvailableTo = DateTime.Now.AddDays(1),
                    MaxDuration = 2,
                    StartedAt = DateTime.Now.AddHours(-1),
                });

                Assert.AreEqual(AssessmentStatus.Started, result);
            }

            [Test]
            public void It_Should_Detect_A_Started_Assessment_Even_If_Out_Of_Range()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Started,
                    AvailableFrom = DateTime.Now.AddDays(-1),
                    AvailableTo = DateTime.Now.AddHours(-1),
                    MaxDuration = 2,
                    StartedAt = DateTime.Now.AddHours(-1),
                });

                Assert.AreEqual(AssessmentStatus.Started, result);
            }

            [Test]
            public void It_Should_Detect_An_Expired_Assessment()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Started,
                    AvailableFrom = DateTime.Now.AddDays(-1),
                    AvailableTo = DateTime.Now.AddDays(1),
                    MaxDuration = 2,
                    StartedAt = DateTime.Now.AddHours(-3)
                });

                Assert.AreEqual(AssessmentStatus.Expired, result);
            }

            [Test]
            public void It_Should_Detect_An_Expired_Assessment_Even_If_Out_Of_Range()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Started,
                    AvailableFrom = DateTime.Now.AddDays(-1),
                    AvailableTo = DateTime.Now.AddHours(-1),
                    MaxDuration = 2,
                    StartedAt = DateTime.Now.AddHours(-3)
                });

                Assert.AreEqual(AssessmentStatus.Expired, result);
            }

            [Test]
            public void It_Should_Detect_A_Submitted_Assessment()
            {
                var result = _assessmentStatusService.GetAssessmentStatus(new SessionModel
                {
                    Status = SessionStatus.Submitted,
                });

                Assert.AreEqual(AssessmentStatus.Submitted, result);
            }
        }
    }
}
