using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Controllers;
using CandidateManager.Web.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CandidateManager.Test.Unit
{
    public class CandidatesControllerTest
    {
        [TestFixture]
        public class When_A_CandidatesController_Is_Present
        {
            private IEnumerable<CandidateModel> _candidateModels;
            private IEnumerable<CandidateViewModel> _candidateViewModels;

            private Mock<IMapper<CandidateModel, CandidateViewModel>> _mapperMock;
            private Mock<ICandidatesRepository> _candidatesRepositoryMock;
            private CandidatesController _controller;

            [SetUp]
            public void SetUp()
            {
                _candidateModels = new List<CandidateModel>
                {
                    new CandidateModel
                    {
                        Id=1,
                        Name = "Test Candidate 1 Name",
                        Surname = "Test Candidate 1 Surname",
                        Email = "Test Candidate 1 Email"
                    },
                    new CandidateModel
                    {
                        Id=2,
                        Name = "Test Candidate 2 Name",
                        Surname = "Test Candidate 2 Surname",
                        Email = "Test Candidate 2 Email"
                    },
                    new CandidateModel
                    {
                        Id=3,
                        Name = "Test Candidate 3 Name",
                        Surname = "Test Candidate 3 Surname",
                        Email = "Test Candidate 3 Email"
                    }
                };
                _candidateViewModels = new List<CandidateViewModel>
                {
                    new CandidateViewModel
                    {
                        Id=1,
                        Name = "Test Candidate 1 Name",
                        Surname = "Test Candidate 1 Surname",
                        Email = "Test Candidate 1 Email"
                    },
                    new CandidateViewModel
                    {
                        Id=2,
                        Name = "Test Candidate 2 Name",
                        Surname = "Test Candidate 2 Surname",
                        Email = "Test Candidate 2 Email"
                    },
                    new CandidateViewModel
                    {
                        Id=3,
                        Name = "Test Candidate 3 Name",
                        Surname = "Test Candidate 3 Surname",
                        Email = "Test Candidate 3 Email"
                    }
                };

                _mapperMock = new Mock<IMapper<CandidateModel, CandidateViewModel>>();
                _mapperMock.Setup(o => o.Map(It.IsAny<CandidateModel>())).Returns((CandidateModel m)
                    => _candidateViewModels.First(vm => vm.Id == m.Id));
                _mapperMock.Setup(o => o.Map(It.IsAny<CandidateViewModel>())).Returns((CandidateViewModel vm)
                    => _candidateModels.First(m => m.Id == vm.Id));
                _candidatesRepositoryMock = new Mock<ICandidatesRepository>();
                _candidatesRepositoryMock.Setup(o => o.GetAll()).Returns(_candidateModels);
                _candidatesRepositoryMock.Setup(o => o.GetById(It.IsAny<int>())).Returns(_candidateModels.First());
                _controller = new CandidatesController(_mapperMock.Object,
                    _candidatesRepositoryMock.Object);
            }

            [Test]
            public void It_Should_Serve_A_List_Request()
            {
                var actionResult = _controller.List();

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as IEnumerable<CandidateViewModel>;
                Assert.IsNotNull(model);
                Assert.AreEqual(_candidateViewModels, model);
            }

            [Test]
            public void It_Should_Serve_A_Create_Request_In_Get()
            {
                var actionResult = _controller.Create();

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                Assert.IsNull(result.Model);
            }

            [Test]
            public void It_Should_Serve_A_Create_Request_In_Post()
            {
                var actionResult = _controller.Create(_candidateViewModels.First());

                _candidatesRepositoryMock.Verify(o => o.AddNew(It.Is<CandidateModel>(m =>
                    m == _candidateModels.First())), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Serve_A_Detail_Request_In_Get()
            {
                var actionResult = _controller.Detail(1);

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as CandidateViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_candidateViewModels.First(), model);
            }

            [Test]
            public void It_Should_Serve_A_Detail_Request_In_Post()
            {
                var actionResult = _controller.Detail(_candidateViewModels.First());

                _candidatesRepositoryMock.Verify(o => o.Update(It.Is<CandidateModel>(m =>
                    m == _candidateModels.First())), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Serve_A_Delete_Request()
            {
                var actionResult = _controller.Delete(1);

                _candidatesRepositoryMock.Verify(o => o.Delete(It.Is<int>(m =>
                    m == 1)), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }
        }
    }
}
