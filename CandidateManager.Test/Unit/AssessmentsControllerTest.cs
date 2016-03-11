using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Services;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Builders;
using CandidateManager.Web.Controllers;
using CandidateManager.Web.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Mail;
using System.Web.Mvc;

namespace CandidateManager.Test.Unit
{
    public class AssessmentsControllerTest
    {
        [TestFixture]
        public class When_An_AssessmentsController_Is_Present
        {
            private ExerciseModel _exerciseModel;
            private Guid _guid;
            private SessionModel _sessionModel;
            private SessionViewModel _sessionViewModel;
            private MailMessage _startedMailMessage;
            private MailMessage _submittedMailMessage;

            private Mock<IMapper<SessionModel, SessionViewModel>> _mapperMock;
            private Mock<IExercisesRepository> _exercisesRepositoryMock;
            private Mock<ISessionsRepository> _sessionsRepositoryMock;
            private Mock<IAssessmentStatusService> _assessmentStatusServiceMock;
            private Mock<ISessionStartedEmailBuilder> _sessionStartedEmailBuilderMock;
            private Mock<ISessionSubmittedEmailBuilder> _sessionSubmittedEmailBuilderMock;
            private Mock<IEmailService> _emailServiceMock;
            private AssessmentsController _controller;

            [SetUp]
            public void SetUp()
            {
                _exerciseModel = new ExerciseModel
                {
                    Id = 1,
                    Name = "Test Exercise Name",
                    Description = "Test Exercise Description",
                    FileName = "Test Exercise FileName",
                    FileData = new byte[0]
                };
                _guid = Guid.NewGuid();
                _sessionModel = new SessionModel
                {
                    Id = _guid,
                    CandidateId = 1,
                    ExerciseId = 1,
                    FileName = "Test Session FileName",
                    FileData = new byte[0]
                };
                _sessionViewModel = new SessionViewModel
                {
                    Id = _guid,
                    CandidateId = 1,
                    ExerciseId = 1,
                    FileName = "Test Session FileName"
                };
                _startedMailMessage = new MailMessage("sender@test.com", "recipient@test.com")
                {
                    Subject = "started - test message subject",
                    Body = "started - test message body"
                };
                _submittedMailMessage = new MailMessage("sender@test.com", "recipient@test.com")
                {
                    Subject = "submitted - test message subject",
                    Body = "submitted - test message body"
                };

                _mapperMock = new Mock<IMapper<SessionModel, SessionViewModel>>();
                _mapperMock.Setup(o => o.Map(It.IsAny<SessionModel>())).Returns(_sessionViewModel);
                _mapperMock.Setup(o => o.Map(It.IsAny<SessionViewModel>())).Returns(_sessionModel);
                _exercisesRepositoryMock = new Mock<IExercisesRepository>();
                _exercisesRepositoryMock.Setup(o => o.GetById(It.IsAny<int>())).Returns(_exerciseModel);
                _sessionsRepositoryMock = new Mock<ISessionsRepository>();
                _sessionsRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(_sessionModel);
                _assessmentStatusServiceMock = new Mock<IAssessmentStatusService>();
                _sessionStartedEmailBuilderMock = new Mock<ISessionStartedEmailBuilder>();
                _sessionStartedEmailBuilderMock.Setup(o => o.Get(It.IsAny<SessionViewModel>()))
                    .Returns(_startedMailMessage);
                _sessionSubmittedEmailBuilderMock = new Mock<ISessionSubmittedEmailBuilder>();
                _sessionSubmittedEmailBuilderMock.Setup(o => o.Get(It.IsAny<SessionViewModel>()))
                    .Returns(_submittedMailMessage);
                _emailServiceMock = new Mock<IEmailService>();
                _controller = new AssessmentsController(_mapperMock.Object,
                    _exercisesRepositoryMock.Object, _sessionsRepositoryMock.Object,
                    _assessmentStatusServiceMock.Object, _sessionStartedEmailBuilderMock.Object,
                    _sessionSubmittedEmailBuilderMock.Object, _emailServiceMock.Object);
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Serve_An_Index_Request_For_A_Valid_Assessment(AssessmentStatus assessmentStatus)
            {
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                var actionResult = _controller.Index(_guid);

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as SessionViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_sessionViewModel, model);
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Serve_A_Download_Request_For_A_Valid_Assessment(AssessmentStatus assessmentStatus)
            {
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                var actionResult = _controller.Download(_guid);

                var result = actionResult as FileContentResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(result.FileDownloadName, "Test Exercise FileName");
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Change_The_SessionStatus_When_Serving_A_Download_Request_For_A_Published_Session_Related_With_A_Valid_Assessment
                (AssessmentStatus assessmentStatus)
            {
                _sessionModel.Status = SessionStatus.Published;
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                _controller.Download(_guid);

                _sessionsRepositoryMock.Verify(o => o.Update(It.Is<SessionModel>(m =>
                    m == _sessionModel)), Times.Once);
                Assert.AreEqual(SessionStatus.Started, _sessionModel.Status);
                Assert.IsNotNull(_sessionModel.StartedAt);
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Shouldnt_Change_The_SessionStatus_When_Serving_A_Download_Request_For_A_Started_Session_Related_With_A_Valid_Assessment
                (AssessmentStatus assessmentStatus)
            {
                _sessionModel.Status = SessionStatus.Started;
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                _controller.Download(_guid);

                _sessionsRepositoryMock.Verify(o => o.Update(It.Is<SessionModel>(m =>
                    m == _sessionModel)), Times.Never);
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Send_An_Email_When_Serving_A_Download_Request_For_A_Published_Session_Related_With_A_Valid_Assessment
                (AssessmentStatus assessmentStatus)
            {
                _sessionModel.Status = SessionStatus.Published;
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                _controller.Download(_guid);

                _sessionStartedEmailBuilderMock.Verify(o => o.Get(It.Is<SessionViewModel>(m =>
                    m == _sessionViewModel)), Times.Once);
                _emailServiceMock.Verify(o => o.SendEmail(It.Is<MailMessage>(m => m == _startedMailMessage)), Times.Once);
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Shouldnt_Send_An_Email_When_Serving_A_Download_Request_For_A_Started_Session_Related_With_A_Valid_Assessment
                (AssessmentStatus assessmentStatus)
            {
                _sessionModel.Status = SessionStatus.Started;
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                _controller.Download(_guid);

                _sessionStartedEmailBuilderMock.Verify(o => o.Get(It.Is<SessionViewModel>(m =>
                    m == _sessionViewModel)), Times.Never);
                _emailServiceMock.Verify(o => o.SendEmail(It.Is<MailMessage>(m => m == _startedMailMessage)), Times.Never);
            }

            [TestCase(AssessmentStatus.Available)]
            [TestCase(AssessmentStatus.Started)]
            public void It_Shouldnt_Throw_An_Expection_While_Sending_An_Email_For_A_Download_Request
                (AssessmentStatus assessmentStatus)
            {
                _emailServiceMock.Setup(o => o.SendEmail(It.IsAny<MailMessage>()))
                    .Callback(() => { throw new Exception(); });
                _sessionModel.Status = SessionStatus.Published;
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);

                Assert.DoesNotThrow(() => _controller.Download(_guid));
            }

            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Serve_A_Submit_Request_In_Get_For_A_Valid_Assessment(AssessmentStatus assessmentStatus)
            {
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                var actionResult = _controller.Submit(_guid);

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as SessionViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_sessionViewModel, model);
            }

            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Serve_A_Submit_Request_In_Post_For_A_Valid_Assessment(AssessmentStatus assessmentStatus)
            {
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                var actionResult = _controller.Submit(_sessionViewModel);

                _sessionsRepositoryMock.Verify(o => o.Update(It.Is<SessionModel>(m =>
                    m == _sessionModel)), Times.Once());
                Assert.AreEqual(SessionStatus.Submitted, _sessionModel.Status);
                Assert.IsNotNull(_sessionModel.SubmittedAt);
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Index", result.RouteValues["action"]);
            }

            [TestCase(AssessmentStatus.Started)]
            public void It_Should_Send_An_Email_When_Serving_A_Submit_Request_In_Post_For_A_Valid_Assessment
                (AssessmentStatus assessmentStatus)
            {
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                var actionResult = _controller.Submit(_sessionViewModel);

                _sessionSubmittedEmailBuilderMock.Verify(o => o.Get(It.Is<SessionViewModel>(m =>
                    m == _sessionViewModel)), Times.Once);
                _emailServiceMock.Verify(o => o.SendEmail(It.Is<MailMessage>(m => m == _submittedMailMessage)), Times.Once);
            }

            [TestCase(AssessmentStatus.Started)]
            public void It_Shouldnt_Throw_An_Expection_While_Sending_An_Email_For_A_Submit_Request
                (AssessmentStatus assessmentStatus)
            {
                _emailServiceMock.Setup(o => o.SendEmail(It.IsAny<MailMessage>()))
                    .Callback(() => { throw new Exception(); });
                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);

                Assert.DoesNotThrow(() => _controller.Submit(_sessionViewModel));
            }

            [TestCase("Index")]
            [TestCase("Download")]
            [TestCase("Submit")]
            public void It_Should_Serve_A_Request_For_An_Unavailable_Assessment(string actionName)
            {
                var actionMethod = _controller.GetType().GetMethod(actionName, new[] { typeof(Guid) });
                Func<ActionResult> action = () => actionMethod.Invoke(_controller, new object[] { _guid })
                    as ActionResult;

                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(AssessmentStatus.Unavailable);
                var actionResult = action();

                var result = actionResult as HttpNotFoundResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(404, result.StatusCode);
            }

            [TestCase("Index", AssessmentStatus.OutOfRange, "OutOfRange")]
            [TestCase("Index", AssessmentStatus.Expired, "Expired")]
            [TestCase("Index", AssessmentStatus.Submitted, "Submitted")]
            [TestCase("Download", AssessmentStatus.OutOfRange, "OutOfRange")]
            [TestCase("Download", AssessmentStatus.Expired, "Expired")]
            [TestCase("Download", AssessmentStatus.Submitted, "Submitted")]
            [TestCase("Submit", AssessmentStatus.OutOfRange, "OutOfRange")]
            [TestCase("Submit", AssessmentStatus.Expired, "Expired")]
            [TestCase("Submit", AssessmentStatus.Submitted, "Submitted")]
            [TestCase("Submit", AssessmentStatus.Available, "CantSubmit")]
            public void It_Should_Serve_A_Request_For_An_Invalid_Assessment(string actionName,
                AssessmentStatus assessmentStatus, string expectedViewName)
            {
                var actionMethod = _controller.GetType().GetMethod(actionName, new[] { typeof(Guid) });
                Func<ActionResult> action = () => actionMethod.Invoke(_controller, new object[] { _guid })
                    as ActionResult;

                _assessmentStatusServiceMock.Setup(o => o.GetAssessmentStatus(It.IsAny<SessionModel>()))
                    .Returns(assessmentStatus);
                var actionResult = action();

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(expectedViewName, result.ViewName);
                var model = result.Model as SessionViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_sessionViewModel, model);
            }
        }
    }
}
