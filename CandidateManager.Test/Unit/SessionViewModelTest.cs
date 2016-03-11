using CandidateManager.Web.ViewModels;
using NUnit.Framework;
using System;

namespace CandidateManager.Test.Unit
{
    public class SessionViewModelTest
    {
        [TestFixture]
        public class When_A_SessionViewModel_Is_Present
        {
            [Test]
            public void It_Should_Calculate_The_ExpiredAt()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                    StartedAt = DateTime.Now.AddDays(-1)
                };

                Assert.AreEqual(
                    viewModel.StartedAt.Value.AddHours(viewModel.MaxDuration.Value),
                    viewModel.ExpiredAt);
            }

            [Test]
            public void It_Should_Calculate_The_ExpiredAt_When_StartedAt_Is_Null()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                };

                Assert.IsNull(viewModel.ExpiredAt);
            }

            [Test]
            public void It_Should_Calculate_The_RemainingDuration()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                    StartedAt = DateTime.Now.AddDays(-1)
                };

                var result = viewModel.RemainingDuration.Value;
                var check = (viewModel.ExpiredAt.Value - DateTime.Now);
                Assert.AreEqual(check, result);
            }

            [Test]
            public void It_Should_Calculate_The_RemainingDuration_When_StartedAt_Is_Null()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                };

                Assert.IsNull(viewModel.RemainingDuration);
            }

            [Test]
            public void It_Should_Calculate_The_RemainingDurationHours()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                    StartedAt = DateTime.Now.AddDays(-1)
                };

                var result = viewModel.RemainingDurationHours.Value;
                var check = (int)Math.Floor((viewModel.ExpiredAt.Value - DateTime.Now).TotalHours);
                Assert.AreEqual(check, result);
            }

            [Test]
            public void It_Should_Calculate_The_RemainingDurationHours_When_StartedAt_Is_Null()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                };

                Assert.IsNull(viewModel.RemainingDurationHours);
            }

            [Test]
            public void It_Should_Calculate_The_RemainingDurationMinutes()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                    StartedAt = DateTime.Now.AddDays(-1)
                };

                var result = viewModel.RemainingDurationMinutes.Value;
                var check = (viewModel.ExpiredAt.Value - DateTime.Now).Minutes;
                Assert.AreEqual(check, result);
            }

            [Test]
            public void It_Should_Calculate_The_RemainingDurationMinutes_When_StartedAt_Is_Null()
            {
                var viewModel = new SessionViewModel
                {
                    MaxDuration = 48,
                };

                Assert.IsNull(viewModel.RemainingDurationMinutes);
            }
        }
    }
}
