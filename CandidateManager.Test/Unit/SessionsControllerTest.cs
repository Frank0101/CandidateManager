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
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace CandidateManager.Test.Unit
{
    public class SessionsControllerTest
    {
        [TestFixture]
        public class When_A_Session_Controller_Is_Present
        {
            private IEnumerable<CandidateModel> _candidateModels;
            private IEnumerable<SelectListItem> _candidateListItems;
            private IEnumerable<ExerciseModel> _exerciseModels;
            private IEnumerable<SelectListItem> _exerciseListItems;
            private IList<Guid> _guids;
            private IEnumerable<SessionModel> _sessionModels;
            private IEnumerable<SessionViewModel> _sessionViewModels;
            private MailMessage _mailMessage;

            private Mock<IMapperOneWay<CandidateModel, SelectListItem>> _candidateListItemMapperMock;
            private Mock<IMapperOneWay<ExerciseModel, SelectListItem>> _exerciseListItemMapperMock;
            private Mock<IMapper<SessionModel, SessionViewModel>> _mapperMock;
            private Mock<ICandidatesRepository> _candidatesRepositoryMock;
            private Mock<IExercisesRepository> _exercisesRepositoryMock;
            private Mock<ISessionsRepository> _sessionsRepositoryMock;
            private Mock<ISessionPublishedEmailBuilder> _sessionPublishedEmailBuilderMock;
            private Mock<IEmailService> _emailServiceMock;
            private SessionsController _controller;

            [SetUp]
            public void SetUp()
            {
                _candidateModels = new List<CandidateModel>
                {
                    new CandidateModel
                    {
                        Id = 1,
                        Name = "Test Candidate 1 Name",
                        Surname = "Test Candidate 1 Surname",
                        Email = "Test Candidate 1 Email"
                    },
                    new CandidateModel
                    {
                        Id = 2,
                        Name = "Test Candidate 2 Name",
                        Surname = "Test Candidate 2 Surname",
                        Email = "Test Candidate 2 Email"
                    },
                };
                _candidateListItems = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Test Candidate 1 Name"
                    },
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Test Candidate 2 Name"
                    }
                };
                _exerciseModels = new List<ExerciseModel>
                {
                    new ExerciseModel
                    {
                        Id = 1,
                        Name = "Test Exercise 1 Name",
                        Description = "Test Exercise 1 Description",
                        FileName = "Test Exercise 1 FileName",
                        FileData = new byte[0]
                    },
                    new ExerciseModel
                    {
                        Id = 2,
                        Name = "Test Exercise 2 Name",
                        Description = "Test Exercise 2 Description",
                        FileName = "Test Exercise 2 FileName",
                        FileData = new byte[0]
                    },
                };
                _exerciseListItems = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "1",
                        Text = "Test Exercise 1 Name"
                    },
                    new SelectListItem
                    {
                        Value = "2",
                        Text = "Test Exercise 2 Name"
                    }
                };
                _guids = new[]
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid()
                };
                _sessionModels = new List<SessionModel>
                {
                    new SessionModel
                    {
                        Id = _guids[0],
                        CandidateId = 1,
                        ExerciseId = 1,
                        FileName = "Test Session 1 FileName",
                        FileData = new byte[0]
                    },
                    new SessionModel
                    {
                        Id = _guids[1],
                        CandidateId = 2,
                        ExerciseId = 2,
                        FileName = "Test Session 2 FileName",
                        FileData = new byte[0]
                    },
                    new SessionModel
                    {
                        Id = _guids[2],
                        CandidateId = 3,
                        ExerciseId = 3,
                        FileName = "Test Session 3 FileName",
                        FileData = new byte[0]
                    }
                };
                _sessionViewModels = new List<SessionViewModel>
                {
                    new SessionViewModel
                    {
                        Id = _guids[0],
                        CandidateId = 1,
                        ExerciseId = 1,
                        FileName = "Test Session 1 FileName",
                    },
                    new SessionViewModel
                    {
                        Id = _guids[1],
                        CandidateId = 2,
                        ExerciseId = 2,
                        FileName = "Test Session 2 FileName",
                    },
                    new SessionViewModel
                    {
                        Id = _guids[2],
                        CandidateId = 3,
                        ExerciseId = 3,
                        FileName = "Test Session 3 FileName",
                    }
                };
                _mailMessage = new MailMessage("sender@test.com", "recipient@test.com")
                {
                    Subject = "test message subject",
                    Body = "test message body"
                };

                _candidateListItemMapperMock = new Mock<IMapperOneWay<CandidateModel, SelectListItem>>();
                _candidateListItemMapperMock.Setup(o => o.Map(It.IsAny<CandidateModel>())).Returns((CandidateModel m)
                    => _candidateListItems.First(li => li.Value == m.Id.ToString()));
                _exerciseListItemMapperMock = new Mock<IMapperOneWay<ExerciseModel, SelectListItem>>();
                _exerciseListItemMapperMock.Setup(o => o.Map(It.IsAny<ExerciseModel>())).Returns((ExerciseModel m)
                    => _exerciseListItems.First(li => li.Value == m.Id.ToString()));
                _mapperMock = new Mock<IMapper<SessionModel, SessionViewModel>>();
                _mapperMock.Setup(o => o.Map(It.IsAny<SessionModel>())).Returns((SessionModel m)
                    => _sessionViewModels.First(vm => vm.Id == m.Id));
                _mapperMock.Setup(o => o.Map(It.IsAny<SessionViewModel>())).Returns((SessionViewModel vm)
                    => _sessionModels.First(m => m.Id == vm.Id));
                _candidatesRepositoryMock = new Mock<ICandidatesRepository>();
                _candidatesRepositoryMock.Setup(o => o.GetAll()).Returns(_candidateModels);
                _exercisesRepositoryMock = new Mock<IExercisesRepository>();
                _exercisesRepositoryMock.Setup(o => o.GetAll()).Returns(_exerciseModels);
                _sessionsRepositoryMock = new Mock<ISessionsRepository>();
                _sessionsRepositoryMock.Setup(o => o.GetAll()).Returns(_sessionModels);
                _sessionsRepositoryMock.Setup(o => o.GetById(It.IsAny<Guid>())).Returns(_sessionModels.First());
                _sessionPublishedEmailBuilderMock = new Mock<ISessionPublishedEmailBuilder>();
                _sessionPublishedEmailBuilderMock.Setup(o => o.Get(It.IsAny<SessionViewModel>()))
                    .Returns(_mailMessage);
                _emailServiceMock = new Mock<IEmailService>();
                _controller = new SessionsController(_mapperMock.Object, _candidateListItemMapperMock.Object,
                    _exerciseListItemMapperMock.Object, _candidatesRepositoryMock.Object,
                    _exercisesRepositoryMock.Object, _sessionsRepositoryMock.Object,
                    _sessionPublishedEmailBuilderMock.Object, _emailServiceMock.Object);
            }

            [Test]
            public void It_Should_Serve_A_List_Request()
            {
                var actionResult = _controller.List();

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as IEnumerable<SessionViewModel>;
                Assert.IsNotNull(model);
                Assert.AreEqual(_sessionViewModels, model);
            }

            [Test]
            public void It_Should_Serve_A_Create_Request_In_Get()
            {
                var actionResult = _controller.Create();

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as SessionFormViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_candidateListItems, model.Candidates);
                Assert.AreEqual(_exerciseListItems, model.Exercises);
                Assert.IsNull(model.Session);
            }

            [Test]
            public void It_Should_Serve_A_CreateByCandidate_Request_In_Get()
            {
                var actionResult = _controller.CreateByCandidate(1);

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("Create", result.ViewName);
                var model = result.Model as SessionFormViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_candidateListItems, model.Candidates);
                Assert.AreEqual(_exerciseListItems, model.Exercises);
                Assert.IsNotNull(model.Session);
                Assert.AreEqual(1, model.Session.CandidateId);
            }

            [Test]
            public void It_Should_Serve_A_Create_Request_In_Post()
            {
                var actionResult = _controller.Create(_sessionViewModels.First());

                _sessionsRepositoryMock.Verify(o => o.AddNew(It.Is<SessionModel>(m =>
                    m == _sessionModels.First())), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Serve_A_Detail_Request_In_Get()
            {
                var guid = _guids[0];
                var actionResult = _controller.Detail(guid);

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as SessionFormViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_candidateListItems, model.Candidates);
                Assert.AreEqual(_exerciseListItems, model.Exercises);
                Assert.AreEqual(_sessionViewModels.First(), model.Session);
            }

            [Test]
            public void It_Should_Serve_A_Detail_Request_In_Post()
            {
                var actionResult = _controller.Detail(_sessionViewModels.First());

                _sessionsRepositoryMock.Verify(o => o.Update(It.Is<SessionModel>(m =>
                    m == _sessionModels.First())), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Serve_A_Delete_Request()
            {
                var guid = _guids[0];
                var actionResult = _controller.Delete(guid);

                _sessionsRepositoryMock.Verify(o => o.Delete(It.Is<Guid>(m =>
                    m == guid)), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Serve_A_Publish_Request()
            {
                var guid = _guids[0];
                var actionResult = _controller.Publish(guid);

                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Change_The_SessionStatus_When_Serving_A_Publish_Request_For_A_Created_Session()
            {
                var guid = _guids[0];
                _controller.Publish(guid);

                _sessionsRepositoryMock.Verify(o => o.Update(It.Is<SessionModel>(m =>
                    m == _sessionModels.First())), Times.Once);
                Assert.AreEqual(SessionStatus.Published, _sessionModels.First().Status);
            }

            [Test]
            public void It_Shouldnt_Change_The_SessionStatus_When_Serving_A_Publish_Request_For_A_Published_Session()
            {
                var guid = _guids[0];
                _sessionModels.First().Status = SessionStatus.Published;
                _controller.Publish(guid);

                _sessionsRepositoryMock.Verify(o => o.Update(It.Is<SessionModel>(m =>
                    m == _sessionModels.First())), Times.Never);
            }

            [Test]
            public void It_Should_Send_An_Email_When_Serving_A_Publish_Request_For_A_Created_Session()
            {
                var guid = _guids[0];
                _controller.Publish(guid);

                _sessionPublishedEmailBuilderMock.Verify(o => o.Get(It.Is<SessionViewModel>(m =>
                    m == _sessionViewModels.First())), Times.Once);
                _emailServiceMock.Verify(o => o.SendEmail(It.Is<MailMessage>(m => m == _mailMessage)), Times.Once);
            }

            [Test]
            public void It_Shouldnt_Send_An_Email_When_Serving_A_Publish_Request_For_A_Published_Session()
            {
                var guid = _guids[0];
                _sessionModels.First().Status = SessionStatus.Published;
                _controller.Publish(guid);

                _sessionPublishedEmailBuilderMock.Verify(o => o.Get(It.Is<SessionViewModel>(m =>
                    m == _sessionViewModels.First())), Times.Never);
                _emailServiceMock.Verify(o => o.SendEmail(It.Is<MailMessage>(m => m == _mailMessage)), Times.Never);
            }

            [Test]
            public void It_Shouldnt_Throw_An_Expection_While_Sending_An_Email_For_A_Publish_Request()
            {
                var guid = _guids[0];
                _emailServiceMock.Setup(o => o.SendEmail(It.IsAny<MailMessage>()))
                    .Callback(() => { throw new Exception(); });

                Assert.DoesNotThrow(() => _controller.Publish(guid));
            }

            [Test]
            public void It_Shoud_Serve_A_Download_Request()
            {
                var guid = _guids[0];
                var actionResult = _controller.Download(guid);

                var result = actionResult as FileContentResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(result.FileDownloadName, "Test Session 1 FileName");
            }
        }
    }
}
