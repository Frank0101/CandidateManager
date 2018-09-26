using CandidateManager.Core.Models;
using CandidateManager.Core.Utils;
using CandidateManager.Web.Utils;
using NUnit.Framework;
using System.Web.Mvc;

namespace CandidateManager.Test.Unit
{
    public class ExerciseListItemMapperTest
    {
        [TestFixture]
        public class When_An_ExerciseListItemMapper_Is_Present
        {
            private IMapperOneWay<ExerciseModel, SelectListItem> _mapper;

            [SetUp]
            public void SetUp()
            {
                _mapper = new ExerciseListItemMapper();
            }

            [Test]
            public void It_Should_Perform_A_Direct_Mapping()
            {
                var model = new ExerciseModel
                {
                    Id = 1,
                    Name = "Name"
                };
                var listItem = _mapper.Map(model);

                Assert.AreEqual("1", listItem.Value);
                Assert.AreEqual("Name", listItem.Text);
            }
        }
    }
}
