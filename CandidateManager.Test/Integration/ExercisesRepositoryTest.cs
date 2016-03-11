using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using CandidateManager.DAL.Repositories;
using CandidateManager.Infra.Utils;
using NUnit.Framework;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace CandidateManager.Test.Integration
{
    public class ExercisesRepositoryTest
    {
        [TestFixture]
        public class When_An_ExercisesRepository_Is_Present
        {
            private IMapper<ExerciseModel, ExerciseEntity> _mapper;
            private CandidateManagerContext _context;
            private DbContextTransaction _transaction;
            private IExercisesRepository _exercisesRepository;

            private ExerciseModel _exerciseModel;

            [SetUp]
            public void SetUp()
            {
                _mapper = new Mapper<ExerciseModel, ExerciseEntity>();
                _context = new CandidateManagerContext();
                _transaction = _context.Database.BeginTransaction();
                _exercisesRepository = new ExercisesRepository(_mapper, _context);

                _exerciseModel = new ExerciseModel
                {
                    Name = "Test Exercise Name",
                    FileName = "Test Exercise FileName",
                    FileData = Encoding.UTF8.GetBytes("Test Exercise FileData")
                };
            }

            [Test]
            public void It_Should_Create_A_New_Exercise()
            {
                var addNewResult = _exercisesRepository.AddNew(_exerciseModel);

                Assert.IsNotNull(addNewResult);
                Assert.AreNotEqual(0, addNewResult.Id);
                Assert.AreEqual(_exerciseModel.Name, addNewResult.Name);
                Assert.AreEqual(_exerciseModel.FileName, addNewResult.FileName);
                Assert.AreEqual(_exerciseModel.FileData, addNewResult.FileData);
            }

            [Test]
            public void It_Should_Get_All_The_Exercises()
            {
                var addNewResult = _exercisesRepository.AddNew(_exerciseModel);
                var getAllResult = _exercisesRepository.GetAll();

                Assert.IsTrue(getAllResult.Any(result =>
                    result.Id == addNewResult.Id
                    && result.Name == addNewResult.Name
                    && result.FileName == addNewResult.FileName
                    && result.FileData == addNewResult.FileData));
            }

            [Test]
            public void It_Should_Get_An_Exercise_By_Id()
            {
                var addNewResult = _exercisesRepository.AddNew(_exerciseModel);
                var getByIdResult = _exercisesRepository.GetById(addNewResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.Name, getByIdResult.Name);
                Assert.AreEqual(addNewResult.FileName, getByIdResult.FileName);
                Assert.AreEqual(addNewResult.FileData, getByIdResult.FileData);
            }

            [Test]
            public void It_Should_Update_An_Exercise()
            {
                var addNewResult = _exercisesRepository.AddNew(_exerciseModel);
                addNewResult.Name += " - Updated";
                addNewResult.FileName += " - Updated";
                addNewResult.FileData = addNewResult.FileData
                    .Concat(Encoding.UTF8.GetBytes(" - Updated")).ToArray();
                var updateResult = _exercisesRepository.Update(addNewResult);
                var getByIdResult = _exercisesRepository.GetById(updateResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.Name, getByIdResult.Name);
                Assert.AreEqual(addNewResult.FileName, getByIdResult.FileName);
                Assert.AreEqual(addNewResult.FileData, getByIdResult.FileData);
            }

            [Test]
            public void It_Should_Update_An_Exercise_When_File_Is_Null()
            {
                var addNewResult = _exercisesRepository.AddNew(_exerciseModel);
                addNewResult.Name += " - Updated";
                addNewResult.FileName = null;
                addNewResult.FileData = null;
                var updateResult = _exercisesRepository.Update(addNewResult);
                var getByIdResult = _exercisesRepository.GetById(updateResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.Name, getByIdResult.Name);
                Assert.AreEqual(_exerciseModel.FileName, getByIdResult.FileName);
                Assert.AreEqual(_exerciseModel.FileData, getByIdResult.FileData);
            }

            [Test]
            public void It_Should_Delete_An_Exercise()
            {
                var addNewResult = _exercisesRepository.AddNew(_exerciseModel);
                _exercisesRepository.Delete(addNewResult.Id);
                var getByIdResult = _exercisesRepository.GetById(addNewResult.Id);

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
