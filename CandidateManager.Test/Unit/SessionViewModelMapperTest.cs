using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Utils;
using CandidateManager.Web.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace CandidateManager.Test.Unit
{
    public class SessionViewModelMapperTest
    {
        [TestFixture]
        public class When_A_SessionViewModelMapper_Is_Present
        {
            private CandidateModel _candidateModel;
            private ExerciseModel _exerciseModel;
            private CandidateViewModel _candidateViewModel;
            private ExerciseViewModel _exerciseViewModel;

            private Mock<IMapper<CandidateModel, CandidateViewModel>> _candidateMapperMock;
            private Mock<IMapper<ExerciseModel, ExerciseViewModel>> _exerciseMapperMock;
            private Mock<ICandidatesRepository> _candidatesRepositoryMock;
            private Mock<IExercisesRepository> _exercisesRepositoryMock;
            private IMapper<SessionModel, SessionViewModel> _mapper;

            [SetUp]
            public void SetUp()
            {
                _candidateModel = new CandidateModel
                {
                    Id = 1,
                    Name = "Test Candidate Name",
                    Surname = "Test Candidate Surname",
                    Email = "Test Candidate Email"
                };
                _exerciseModel = new ExerciseModel
                {
                    Id = 2,
                    Name = "Test Exercise Name",
                    Description = "Test Exercise Description",
                    FileName = "Test Exercise FileName"
                };
                _candidateViewModel = new CandidateViewModel
                {
                    Id = 1,
                    Name = "Test Candidate Name",
                    Surname = "Test Candidate Surname",
                    Email = "Test Candidate Email"
                };
                _exerciseViewModel = new ExerciseViewModel
                {
                    Id = 2,
                    Name = "Test Exercise Name",
                    Description = "Test Exercise Description",
                    FileName = "Test Exercise FileName"
                };

                _candidateMapperMock = new Mock<IMapper<CandidateModel, CandidateViewModel>>();
                _candidateMapperMock.Setup(o => o.Map(It.IsAny<CandidateModel>())).Returns(_candidateViewModel);
                _exerciseMapperMock = new Mock<IMapper<ExerciseModel, ExerciseViewModel>>();
                _exerciseMapperMock.Setup(o => o.Map(It.IsAny<ExerciseModel>())).Returns(_exerciseViewModel);
                _candidatesRepositoryMock = new Mock<ICandidatesRepository>();
                _candidatesRepositoryMock.Setup(o => o.GetById(It.IsAny<int>())).Returns(_candidateModel);
                _exercisesRepositoryMock = new Mock<IExercisesRepository>();
                _exercisesRepositoryMock.Setup(o => o.GetById(It.IsAny<int>())).Returns(_exerciseModel);
                _mapper = new SessionViewModelMapper(_candidateMapperMock.Object, _exerciseMapperMock.Object,
                    _candidatesRepositoryMock.Object, _exercisesRepositoryMock.Object);
            }

            [Test]
            public void It_Should_Perform_A_Direct_Mapping()
            {
                var model = new SessionModel
                {
                    Id = Guid.NewGuid(),
                    CandidateId = 1,
                    ExerciseId = 2,
                    AvailableFrom = new DateTime(2016, 01, 28, 12, 0, 0),
                    AvailableTo = new DateTime(2016, 01, 29, 12, 0, 0),
                    MaxDuration = 180,
                    Status = SessionStatus.Created,
                    StartedAt = new DateTime(2016, 01, 28, 14, 0, 0),
                    SubmittedAt = new DateTime(2016, 01, 28, 18, 0, 0),
                    FileName = "Test Session FileName",
                    FileData = Encoding.UTF8.GetBytes("Test Session FileData")
                };
                var viewModel = _mapper.Map(model);

                Assert.AreEqual(model.Id, viewModel.Id);
                Assert.AreEqual(model.CandidateId, viewModel.CandidateId);
                Assert.AreEqual(model.ExerciseId, viewModel.ExerciseId);
                Assert.AreEqual(model.AvailableFrom, viewModel.AvailableFrom);
                Assert.AreEqual(model.AvailableTo, viewModel.AvailableTo);
                Assert.AreEqual(model.Status, viewModel.Status);
                Assert.AreEqual(model.StartedAt, viewModel.StartedAt);
                Assert.AreEqual(model.SubmittedAt, viewModel.SubmittedAt);
                Assert.AreEqual(model.FileName, viewModel.FileName);
                Assert.IsNull(viewModel.File);
                Assert.AreEqual(_candidateViewModel, viewModel.Candidate);
                Assert.AreEqual(_exerciseViewModel, viewModel.Exercise);
            }

            [Test]
            public void It_Should_Perform_An_Inverse_Mapping()
            {
                var fileMock = new Mock<HttpPostedFileBase>();
                fileMock.Setup(o => o.FileName).Returns("Test Session FileName");
                fileMock.Setup(o => o.InputStream)
                    .Returns(new MemoryStream(Encoding.UTF8.GetBytes("Test Session FileData")));

                var viewModel = new SessionViewModel
                {
                    Id = Guid.NewGuid(),
                    CandidateId = 1,
                    ExerciseId = 2,
                    AvailableFrom = new DateTime(2016, 01, 28, 12, 0, 0),
                    AvailableTo = new DateTime(2016, 01, 29, 12, 0, 0),
                    MaxDuration = 180,
                    Status = SessionStatus.Created,
                    StartedAt = new DateTime(2016, 01, 28, 14, 0, 0),
                    SubmittedAt = new DateTime(2016, 01, 28, 18, 0, 0),
                    File = fileMock.Object
                };
                var model = _mapper.Map(viewModel);

                Assert.AreEqual(viewModel.Id, model.Id);
                Assert.AreEqual(viewModel.CandidateId, model.CandidateId);
                Assert.AreEqual(viewModel.ExerciseId, model.ExerciseId);
                Assert.AreEqual(viewModel.AvailableFrom, model.AvailableFrom);
                Assert.AreEqual(viewModel.AvailableTo, model.AvailableTo);
                Assert.AreEqual(viewModel.Status, model.Status);
                Assert.AreEqual(viewModel.StartedAt, model.StartedAt);
                Assert.AreEqual(viewModel.SubmittedAt, model.SubmittedAt);
                Assert.AreEqual(viewModel.File.FileName, model.FileName);
                Assert.AreEqual(((MemoryStream)viewModel.File.InputStream).ToArray(), model.FileData);
            }

            [Test]
            public void It_Should_Perform_An_Inverse_Mapping_When_File_Is_Null()
            {
                var viewModel = new SessionViewModel
                {
                    Id = Guid.NewGuid(),
                    CandidateId = 1,
                    ExerciseId = 2,
                    AvailableFrom = new DateTime(2016, 01, 28, 12, 0, 0),
                    AvailableTo = new DateTime(2016, 01, 29, 12, 0, 0),
                    MaxDuration = 180,
                    Status = SessionStatus.Created,
                    StartedAt = new DateTime(2016, 01, 28, 14, 0, 0),
                    SubmittedAt = new DateTime(2016, 01, 28, 18, 0, 0),
                    File = null
                };
                var model = _mapper.Map(viewModel);

                Assert.AreEqual(viewModel.Id, model.Id);
                Assert.AreEqual(viewModel.CandidateId, model.CandidateId);
                Assert.AreEqual(viewModel.ExerciseId, model.ExerciseId);
                Assert.AreEqual(viewModel.AvailableFrom, model.AvailableFrom);
                Assert.AreEqual(viewModel.AvailableTo, model.AvailableTo);
                Assert.AreEqual(viewModel.Status, model.Status);
                Assert.AreEqual(viewModel.StartedAt, model.StartedAt);
                Assert.AreEqual(viewModel.SubmittedAt, model.SubmittedAt);
                Assert.IsNull(model.FileName);
                Assert.IsNull(model.FileData);
            }
        }
    }
}
