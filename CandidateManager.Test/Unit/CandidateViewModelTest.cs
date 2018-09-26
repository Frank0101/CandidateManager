using CandidateManager.Web.ViewModels;
using NUnit.Framework;

namespace CandidateManager.Test.Unit
{
    public class CandidateViewModelTest
    {
        [TestFixture]
        public class When_A_CandidateViewModel_Is_Present
        {
            [Test]
            public void It_Should_Calculate_The_FullName()
            {
                var viewModel = new CandidateViewModel
                {
                    Id = 1,
                    Name = "Name",
                    Surname = "Surname"
                };

                Assert.AreEqual("Name Surname", viewModel.FullName);
            }
        }
    }
}
