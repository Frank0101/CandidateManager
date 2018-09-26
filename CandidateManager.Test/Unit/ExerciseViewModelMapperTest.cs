using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Utils;
using CandidateManager.Web.ViewModels;
using Moq;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Web;

namespace CandidateManager.Test.Unit
{
    public class ExerciseViewModelMapperTest
    {
        [TestFixture]
        public class When_An_ExerciseViewModelMapper_Is_Present
        {
            private IMapper<ExerciseModel, ExerciseViewModel> _mapper;

            [SetUp]
            public void SetUp()
            {
                _mapper = new ExerciseViewModelMapper();
            }

            [Test]
            public void It_Should_Perform_A_Direct_Mapping()
            {
                var model = new ExerciseModel
                {
                    Id = 1,
                    Name = "Test Exercise Name",
                    Description = "Test Exercise Description",
                    FileName = "Test Exercise FileName",
                    FileData = Encoding.UTF8.GetBytes("Test Exercise FileData")
                };
                var viewModel = _mapper.Map(model);

                Assert.AreEqual(model.Id, viewModel.Id);
                Assert.AreEqual(model.Name, viewModel.Name);
                Assert.AreEqual(model.Description, viewModel.Description);
                Assert.AreEqual(model.FileName, viewModel.FileName);
                Assert.IsNull(viewModel.File);
            }

            [Test]
            public void It_Should_Perform_An_Inverse_Mapping()
            {
                var fileMock = new Mock<HttpPostedFileBase>();
                fileMock.Setup(o => o.FileName).Returns("Test Exercise FileName");
                fileMock.Setup(o => o.InputStream)
                    .Returns(new MemoryStream(Encoding.UTF8.GetBytes("Test Exercise FileData")));

                var viewModel = new ExerciseViewModel
                {
                    Id = 1,
                    Name = "Test Exercise Name",
                    Description = "Test Exercise Description",
                    File = fileMock.Object
                };
                var model = _mapper.Map(viewModel);

                Assert.AreEqual(viewModel.Id, model.Id);
                Assert.AreEqual(viewModel.Name, model.Name);
                Assert.AreEqual(viewModel.Description, model.Description);
                Assert.AreEqual(viewModel.File.FileName, model.FileName);
                Assert.AreEqual(((MemoryStream)viewModel.File.InputStream).ToArray(), model.FileData);
            }

            [Test]
            public void It_Should_Perform_An_Inverse_Mapping_When_File_Is_Null()
            {
                var viewModel = new ExerciseViewModel
                {
                    Id = 1,
                    Name = "Test Exercise Name",
                    Description = "Test Exercise Description",
                    File = null
                };
                var model = _mapper.Map(viewModel);

                Assert.AreEqual(viewModel.Id, model.Id);
                Assert.AreEqual(viewModel.Name, model.Name);
                Assert.AreEqual(viewModel.Description, model.Description);
                Assert.IsNull(model.FileName);
                Assert.IsNull(model.FileData);
            }
        }
    }
}
