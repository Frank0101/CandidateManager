using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using CandidateManager.DAL.Repositories;
using CandidateManager.Infra.Utils;
using NUnit.Framework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CandidateManager.Test.Integration
{
    public class TestRepositoryTest
    {
        [TestFixture]
        public class When_A_SessionsRepository_Is_Present
        {
            private IMapper<CandidateModel, CandidateEntity> _candidatesMapper;
            private IMapper<ExerciseModel, ExerciseEntity> _exercisesMapper;
            private IMapper<SessionModel, SessionEntity> _sessionsMapper;
            private CandidateManagerContext _context;
            private DbContextTransaction _transaction;
            private ICandidatesRepository _candidatesRepository;
            private IExercisesRepository _exercisesRepository;
            private ISessionsRepository _sessionsRepository;

            private CandidateModel _candidateModel;
            private ExerciseModel _exerciseModel;
            private SessionModel _sessionModel;

            [SetUp]
            public void SetUp()
            {
                _candidatesMapper = new Mapper<CandidateModel, CandidateEntity>();
                _exercisesMapper = new Mapper<ExerciseModel, ExerciseEntity>();
                _sessionsMapper = new Mapper<SessionModel, SessionEntity>();
                _context = new CandidateManagerContext();
                _transaction = _context.Database.BeginTransaction();
                _candidatesRepository = new CandidatesRepository(_candidatesMapper, _context);
                _exercisesRepository = new ExercisesRepository(_exercisesMapper, _context);
                _sessionsRepository = new SessionsRepository(_sessionsMapper, _context);

                _candidateModel = _candidatesRepository.AddNew(new CandidateModel
                {
                    Name = "Test Candidate Name",
                    Surname = "Test Candidate Surname",
                    Email = "Test Candidate Email"
                });
                _exerciseModel = _exercisesRepository.AddNew(new ExerciseModel
                {
                    Name = "Test Exercise Name",
                    FileName = "Test Exercise FileName",
                    FileData = Encoding.UTF8.GetBytes("Test Exercise FileData")
                });
                _sessionModel = new SessionModel
                {
                    CandidateId = _candidateModel.Id,
                    ExerciseId = _exerciseModel.Id,
                    AvailableFrom = DateTime.Now,
                    AvailableTo = DateTime.Now.AddDays(3),
                    MaxDuration = 180,
                    Status = SessionStatus.Submitted,
                    StartedAt = DateTime.Now,
                    SubmittedAt = DateTime.Now,
                    FileName = "Test Session FileName",
                    FileData = Encoding.UTF8.GetBytes("Test Session FileData")
                };
            }

            [Test]
            public void It_Should_Create_A_New_Session()
            {
                var addNewResult = _sessionsRepository.AddNew(_sessionModel);

                Assert.IsNotNull(addNewResult);
                Assert.AreNotEqual(Guid.Empty, addNewResult.Id);
                Assert.AreEqual(_sessionModel.CandidateId, addNewResult.CandidateId);
                Assert.AreEqual(_sessionModel.ExerciseId, addNewResult.ExerciseId);
                Assert.AreEqual(_sessionModel.AvailableFrom, addNewResult.AvailableFrom);
                Assert.AreEqual(_sessionModel.AvailableTo, addNewResult.AvailableTo);
                Assert.AreEqual(_sessionModel.MaxDuration, addNewResult.MaxDuration);
                Assert.AreEqual(_sessionModel.Status, addNewResult.Status);
                Assert.AreEqual(_sessionModel.StartedAt, addNewResult.StartedAt);
                Assert.AreEqual(_sessionModel.SubmittedAt, addNewResult.SubmittedAt);
                Assert.AreEqual(_sessionModel.FileName, addNewResult.FileName);
                Assert.AreEqual(_sessionModel.FileData, addNewResult.FileData);
            }

            [Test]
            public void It_Should_Get_All_The_Sessions()
            {
                var addNewResult = _sessionsRepository.AddNew(_sessionModel);
                var getAllResult = _sessionsRepository.GetAll();

                Assert.IsTrue(getAllResult.Any(result =>
                    result.Id == addNewResult.Id
                    && result.CandidateId == addNewResult.CandidateId
                    && result.ExerciseId == addNewResult.ExerciseId
                    && result.AvailableFrom == addNewResult.AvailableFrom
                    && result.AvailableTo == addNewResult.AvailableTo
                    && result.MaxDuration == addNewResult.MaxDuration
                    && result.Status == addNewResult.Status
                    && result.StartedAt == addNewResult.StartedAt
                    && result.SubmittedAt == addNewResult.SubmittedAt
                    && result.FileName == addNewResult.FileName
                    && result.FileData == addNewResult.FileData));
            }

            [Test]
            public void It_Should_Get_A_Session_By_Id()
            {
                var addNewResult = _sessionsRepository.AddNew(_sessionModel);
                var getByIdResult = _sessionsRepository.GetById(addNewResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.CandidateId, getByIdResult.CandidateId);
                Assert.AreEqual(addNewResult.ExerciseId, getByIdResult.ExerciseId);
                Assert.AreEqual(addNewResult.AvailableFrom, getByIdResult.AvailableFrom);
                Assert.AreEqual(addNewResult.AvailableTo, getByIdResult.AvailableTo);
                Assert.AreEqual(addNewResult.MaxDuration, getByIdResult.MaxDuration);
                Assert.AreEqual(addNewResult.Status, getByIdResult.Status);
                Assert.AreEqual(addNewResult.StartedAt, getByIdResult.StartedAt);
                Assert.AreEqual(addNewResult.SubmittedAt, getByIdResult.SubmittedAt);
                Assert.AreEqual(addNewResult.FileName, getByIdResult.FileName);
                Assert.AreEqual(addNewResult.FileData, getByIdResult.FileData);
            }

            [Test]
            public void It_Should_Update_A_Session()
            {
                _candidateModel = _candidatesRepository.AddNew(new CandidateModel
                {
                    Name = "New Test Candidate Name",
                    Surname = "New Test Candidate Surname",
                    Email = "New Test Candidate Email"
                });
                _exerciseModel = _exercisesRepository.AddNew(new ExerciseModel
                {
                    Name = "New Test Exercise Name",
                    FileName = "New Test Exercise FileName",
                    FileData = Encoding.UTF8.GetBytes("New Test Exercise FileData")
                });

                var addNewResult = _sessionsRepository.AddNew(_sessionModel);
                addNewResult.CandidateId = _candidateModel.Id;
                addNewResult.ExerciseId = _exerciseModel.Id;
                addNewResult.AvailableFrom = DateTime.Now.AddDays(1);
                addNewResult.AvailableTo = DateTime.Now.AddDays(1);
                addNewResult.MaxDuration += 60;
                addNewResult.Status = SessionStatus.Started;
                addNewResult.StartedAt = DateTime.Now.AddDays(1);
                addNewResult.SubmittedAt = DateTime.Now.AddDays(1);
                addNewResult.FileName += " - Updated";
                addNewResult.FileData = addNewResult.FileData
                    .Concat(Encoding.UTF8.GetBytes(" - Updated")).ToArray();
                var updateResult = _sessionsRepository.Update(addNewResult);
                var getByIdResult = _sessionsRepository.GetById(updateResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.CandidateId, getByIdResult.CandidateId);
                Assert.AreEqual(addNewResult.ExerciseId, getByIdResult.ExerciseId);
                Assert.AreEqual(addNewResult.AvailableFrom, getByIdResult.AvailableFrom);
                Assert.AreEqual(addNewResult.AvailableTo, getByIdResult.AvailableTo);
                Assert.AreEqual(addNewResult.MaxDuration, getByIdResult.MaxDuration);
                Assert.AreEqual(addNewResult.Status, getByIdResult.Status);
                Assert.AreEqual(addNewResult.StartedAt, getByIdResult.StartedAt);
                Assert.AreEqual(addNewResult.SubmittedAt, getByIdResult.SubmittedAt);
                Assert.AreEqual(addNewResult.FileName, getByIdResult.FileName);
                Assert.AreEqual(addNewResult.FileData, getByIdResult.FileData);
            }

            [Test]
            public void It_Should_Update_A_Session_When_File_Is_Null()
            {
                _candidateModel = _candidatesRepository.AddNew(new CandidateModel
                {
                    Name = "New Test Candidate Name",
                    Surname = "New Test Candidate Surname",
                    Email = "New Test Candidate Email"
                });
                _exerciseModel = _exercisesRepository.AddNew(new ExerciseModel
                {
                    Name = "New Test Exercise Name",
                    FileName = "New Test Exercise FileName",
                    FileData = Encoding.UTF8.GetBytes("New Test Exercise FileData")
                });

                var addNewResult = _sessionsRepository.AddNew(_sessionModel);
                addNewResult.CandidateId = _candidateModel.Id;
                addNewResult.ExerciseId = _exerciseModel.Id;
                addNewResult.AvailableFrom = DateTime.Now.AddDays(1);
                addNewResult.AvailableTo = DateTime.Now.AddDays(1);
                addNewResult.MaxDuration += 60;
                addNewResult.Status = SessionStatus.Started;
                addNewResult.StartedAt = DateTime.Now.AddDays(1);
                addNewResult.SubmittedAt = DateTime.Now.AddDays(1);
                addNewResult.FileName = null;
                addNewResult.FileData = null;
                var updateResult = _sessionsRepository.Update(addNewResult);
                var getByIdResult = _sessionsRepository.GetById(updateResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.CandidateId, getByIdResult.CandidateId);
                Assert.AreEqual(addNewResult.ExerciseId, getByIdResult.ExerciseId);
                Assert.AreEqual(addNewResult.AvailableFrom, getByIdResult.AvailableFrom);
                Assert.AreEqual(addNewResult.AvailableTo, getByIdResult.AvailableTo);
                Assert.AreEqual(addNewResult.MaxDuration, getByIdResult.MaxDuration);
                Assert.AreEqual(addNewResult.Status, getByIdResult.Status);
                Assert.AreEqual(addNewResult.StartedAt, getByIdResult.StartedAt);
                Assert.AreEqual(addNewResult.SubmittedAt, getByIdResult.SubmittedAt);
                Assert.AreEqual(_sessionModel.FileName, getByIdResult.FileName);
                Assert.AreEqual(_sessionModel.FileData, getByIdResult.FileData);
            }

            [Test]
            public void It_Should_Delete_A_Session()
            {
                var addNewResult = _sessionsRepository.AddNew(_sessionModel);
                _sessionsRepository.Delete(addNewResult.Id);
                var getByIdResult = _sessionsRepository.GetById(addNewResult.Id);

                Assert.IsNull(getByIdResult);
            }

            [TearDown]
            public void TearDown()
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _context.Dispose();
            }
        }
    }
}
