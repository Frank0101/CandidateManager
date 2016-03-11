using CandidateManager.Core.DAL.Repositories;
using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.DAL.Entities;
using CandidateManager.DAL.Repositories;
using CandidateManager.Infra.Utils;
using NUnit.Framework;
using System.Data.Entity;
using System.Linq;

namespace CandidateManager.Test.Integration
{
    public class CandidatesRepositoryTest
    {
        [TestFixture]
        public class When_A_CandidatesRepository_Is_Present
        {
            private IMapper<CandidateModel, CandidateEntity> _mapper;
            private CandidateManagerContext _context;
            private DbContextTransaction _transaction;
            private ICandidatesRepository _candidatesRepository;

            private CandidateModel _candidateModel;

            [SetUp]
            public void SetUp()
            {
                _mapper = new Mapper<CandidateModel, CandidateEntity>();
                _context = new CandidateManagerContext();
                _transaction = _context.Database.BeginTransaction();
                _candidatesRepository = new CandidatesRepository(_mapper, _context);

                _candidateModel = new CandidateModel
                {
                    Name = "Test Candidate Name",
                    Surname = "Test Candidate Surname",
                    Email = "Test Candidate Email"
                };
            }

            [Test]
            public void It_Should_Create_A_New_Candidate()
            {
                var addNewResult = _candidatesRepository.AddNew(_candidateModel);

                Assert.IsNotNull(addNewResult);
                Assert.AreNotEqual(0, addNewResult.Id);
                Assert.AreEqual(_candidateModel.Name, addNewResult.Name);
                Assert.AreEqual(_candidateModel.Surname, addNewResult.Surname);
                Assert.AreEqual(_candidateModel.Email, addNewResult.Email);
            }

            [Test]
            public void It_Should_Get_All_The_Candidates()
            {
                var addNewResult = _candidatesRepository.AddNew(_candidateModel);
                var getAllResult = _candidatesRepository.GetAll();

                Assert.IsTrue(getAllResult.Any(result =>
                    result.Id == addNewResult.Id
                    && result.Name == addNewResult.Name
                    && result.Surname == addNewResult.Surname
                    && result.Email == addNewResult.Email));
            }

            [Test]
            public void It_Should_Get_A_Candidate_By_Id()
            {
                var addNewResult = _candidatesRepository.AddNew(_candidateModel);
                var getByIdResult = _candidatesRepository.GetById(addNewResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.Name, getByIdResult.Name);
                Assert.AreEqual(addNewResult.Surname, getByIdResult.Surname);
                Assert.AreEqual(addNewResult.Email, getByIdResult.Email);
            }

            [Test]
            public void It_Should_Update_A_Candidate()
            {
                var addNewResult = _candidatesRepository.AddNew(_candidateModel);
                addNewResult.Name += " - Updated";
                addNewResult.Surname += " - Updated";
                addNewResult.Email += " - Updated";
                var updateResult = _candidatesRepository.Update(addNewResult);
                var getByIdResult = _candidatesRepository.GetById(updateResult.Id);

                Assert.IsNotNull(getByIdResult);
                Assert.AreEqual(addNewResult.Id, getByIdResult.Id);
                Assert.AreEqual(addNewResult.Name, getByIdResult.Name);
                Assert.AreEqual(addNewResult.Surname, getByIdResult.Surname);
                Assert.AreEqual(addNewResult.Email, getByIdResult.Email);
            }

            [Test]
            public void It_Should_Delete_A_Candidate()
            {
                var addNewResult = _candidatesRepository.AddNew(_candidateModel);
                _candidatesRepository.Delete(addNewResult.Id);
                var getByIdResult = _candidatesRepository.GetById(addNewResult.Id);

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
