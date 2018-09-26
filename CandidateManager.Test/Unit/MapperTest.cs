using CandidateManager.Core.Utils;
using CandidateManager.Infra.Utils;
using NUnit.Framework;
using System;

namespace CandidateManager.Test.Unit
{
    public class MapperTest
    {
        public class TestClass1
        {
            public int Param1 { get; set; }
            public bool Param2 { get; set; }
            public string Param3 { get; set; }
            public DateTime Param4 { get; set; }
        }

        public class TestClass2
        {
            public int Param1 { get; set; }
            public bool Param2 { get; set; }
            public string Param3 { get; set; }
            public DateTime Param4 { get; set; }
        }

        [TestFixture]
        public class When_A_Mapper_Is_Present
        {
            private IMapper<TestClass1, TestClass2> _mapper;

            [SetUp]
            public void SetUp()
            {
                _mapper = new Mapper<TestClass1, TestClass2>();
            }

            [Test]
            public void It_Should_Perform_A_Direct_Mapping()
            {
                var testClass1 = new TestClass1
                {
                    Param1 = 1,
                    Param2 = true,
                    Param3 = "3",
                    Param4 = DateTime.Now
                };
                var testClass2 = _mapper.Map(testClass1);

                Assert.AreEqual(testClass1.Param1, testClass2.Param1);
                Assert.AreEqual(testClass1.Param2, testClass2.Param2);
                Assert.AreEqual(testClass1.Param3, testClass2.Param3);
                Assert.AreEqual(testClass1.Param4, testClass2.Param4);
            }

            [Test]
            public void It_Should_Perform_An_Inverse_Mapping()
            {
                var testClass2 = new TestClass2
                {
                    Param1 = 1,
                    Param2 = true,
                    Param3 = "3",
                    Param4 = DateTime.Now
                };
                var testClass1 = _mapper.Map(testClass2);

                Assert.AreEqual(testClass2.Param1, testClass1.Param1);
                Assert.AreEqual(testClass2.Param2, testClass1.Param2);
                Assert.AreEqual(testClass2.Param3, testClass1.Param3);
                Assert.AreEqual(testClass2.Param4, testClass1.Param4);
            }
        }
    }
}
