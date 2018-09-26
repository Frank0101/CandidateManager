using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Utils;
using NUnit.Framework;
using System.Web.Mvc;

namespace CandidateManager.Test.Unit
{
    public class CandidateListItemMapperTest
    {
        [TestFixture]
        public class When_A_CandidateListItemMapper_Is_Present
        {
            private IMapperOneWay<CandidateModel, SelectListItem> _mapper;

            [SetUp]
            public void SetUp()
            {
                _mapper = new CandidateListItemMapper();
            }

            [Test]
            public void It_Should_Perform_A_Direct_Mapping()
            {
                var model = new CandidateModel
                {
                    Id = 1,
                    Name = "Name",
                    Surname = "Surname",
                };
                var listItem = _mapper.Map(model);

                Assert.AreEqual("1", listItem.Value);
                Assert.AreEqual("Name Surname", listItem.Text);
            }
        }
    }
}
