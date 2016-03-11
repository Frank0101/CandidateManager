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
    public class ExercisesControllerTest
    {
        [TestFixture]
        public class When_An_ExercisesController_Is_Present
        {
            private IEnumerable<ExerciseModel> _exerciseModels;
            private IEnumerable<ExerciseViewModel> _exerciseViewModels;

            private Mock<IMapper<ExerciseModel, ExerciseViewModel>> _mapperMock;
            private Mock<IExercisesRepository> _exercisesRepositoryMock;
            private ExercisesController _controller;

            [SetUp]
            public void SetUp()
            {
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
                    new ExerciseModel
                    {
                        Id = 3,
                        Name = "Test Exercise 3 Name",
                        Description = "Test Exercise 3 Description",
                        FileName = "Test Exercise 3 FileName",
                        FileData = new byte[0]
                    }
                };
                _exerciseViewModels = new List<ExerciseViewModel>
                {
                    new ExerciseViewModel
                    {
                        Id = 1,
                        Name = "Test Exercise 1 Name",
                        Description = "Test Exercise 1 Description",
                        FileName = "Test Exercise 1 FileName"
                    },
                    new ExerciseViewModel
                    {
                        Id = 2,
                        Name = "Test Exercise 2 Name",
                        Description = "Test Exercise 2 Description",
                        FileName = "Test Exercise 2 FileName"
                    },
                    new ExerciseViewModel
                    {
                        Id = 3,
                        Name = "Test Exercise 3 Name",
                        Description = "Test Exercise 3 Description",
                        FileName = "Test Exercise 3 FileName"
                    }
                };

                _mapperMock = new Mock<IMapper<ExerciseModel, ExerciseViewModel>>();
                _mapperMock.Setup(o => o.Map(It.IsAny<ExerciseModel>())).Returns((ExerciseModel m)
                    => _exerciseViewModels.First(vm => vm.Id == m.Id));
                _mapperMock.Setup(o => o.Map(It.IsAny<ExerciseViewModel>())).Returns((ExerciseViewModel vm)
                    => _exerciseModels.First(m => m.Id == vm.Id));
                _exercisesRepositoryMock = new Mock<IExercisesRepository>();
                _exercisesRepositoryMock.Setup(o => o.GetAll()).Returns(_exerciseModels);
                _exercisesRepositoryMock.Setup(o => o.GetById(It.IsAny<int>())).Returns(_exerciseModels.First());
                _controller = new ExercisesController(_mapperMock.Object,
                    _exercisesRepositoryMock.Object);
            }

            [Test]
            public void It_Should_Serve_A_List_Request()
            {
                var actionResult = _controller.List();

                var result = actionResult as ViewResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("", result.ViewName);
                var model = result.Model as IEnumerable<ExerciseViewModel>;
                Assert.IsNotNull(model);
                Assert.AreEqual(_exerciseViewModels, model);
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
                var actionResult = _controller.Create(_exerciseViewModels.First());

                _exercisesRepositoryMock.Verify(o => o.AddNew(It.Is<ExerciseModel>(m =>
                    m == _exerciseModels.First())), Times.Once());
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
                var model = result.Model as ExerciseViewModel;
                Assert.IsNotNull(model);
                Assert.AreEqual(_exerciseViewModels.First(), model);
            }

            [Test]
            public void It_Should_Serve_A_Detail_Request_In_Post()
            {
                var actionResult = _controller.Detail(_exerciseViewModels.First());

                _exercisesRepositoryMock.Verify(o => o.Update(It.Is<ExerciseModel>(m =>
                    m == _exerciseModels.First())), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Should_Serve_A_Delete_Request()
            {
                var actionResult = _controller.Delete(1);

                _exercisesRepositoryMock.Verify(o => o.Delete(It.Is<int>(m =>
                    m == 1)), Times.Once());
                var result = actionResult as RedirectToRouteResult;
                Assert.IsNotNull(result);
                Assert.AreEqual("List", result.RouteValues["action"]);
            }

            [Test]
            public void It_Shoud_Serve_A_Download_Request()
            {
                var actionResult = _controller.Download(1);

                var result = actionResult as FileContentResult;
                Assert.IsNotNull(result);
                Assert.AreEqual(result.FileDownloadName, "Test Exercise 1 FileName");
            }
        }
    }
}
