using System.Linq;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Services;
using Xunit;
using FluentAssertions;

namespace Bittn.Api.Tests.Services
{
    public class BittnServiceHelperTests
    {
        [Theory]
        [InlineData(1, 1, 111, 3, 333)]
        [InlineData(2, 2, 222, 4, 888)]
        [InlineData(3, 3, 333, 2, 666)]
        public void GetHelpDetails_Should_Return_Correctly(int painLevel, int expectedPainLevel, int expectedLength, int expectedProcessTime, int expectedWaitTime)
        {
            var hospital = new HospitalDetails
            {
                Id = 444,
                Name = "name",
                Location = new LocationDetails
                {
                    Latitude = 999,
                    Longitude = 888
                },
                Queue = new []
                {
                    new QueueDetails
                    {
                        PainLevel = 1,
                        PatientCount = 111,
                        AverageQueueTime = 3
                    },
                    new QueueDetails
                    {
                        PainLevel = 2,
                        PatientCount = 222,
                        AverageQueueTime = 4
                    },
                    new QueueDetails
                    {
                        PainLevel = 3,
                        PatientCount = 333,
                        AverageQueueTime = 2
                    }
                }
            };

            var illnessId = 123;
            var actual = BittnServiceHelper.GetHelpDetails(new [] { hospital }, illnessId, painLevel);

            actual.Should().HaveCount(1);
            actual.First().Id.Should().Be(hospital.Id);
            actual.First().IllnessId.Should().Be(illnessId);
            actual.First().PainLevel.Should().Be(expectedPainLevel);
            actual.First().Location.Should().Be(hospital.Location);
            actual.First().AverageProcessTime.Should().Be(expectedProcessTime);
            actual.First().QueueLength.Should().Be(expectedLength);
            actual.First().WaitingTime.Should().Be(expectedWaitTime);
        }

        [Theory]
        [InlineData(1, 111)]
        [InlineData(2, 222)]
        [InlineData(3, 333)]
        public void GetHelpDetails_Should_Exclude_Hospitals_That_Has_No_Support_For_PainLevel(int painLevel, int expectedId)
        {
            var hospitals = new []
            {
                new HospitalDetails
                {
                    Id = 111,
                    Queue = new []
                    {
                        new QueueDetails { PainLevel = 1 }
                    }
                },
                new HospitalDetails
                {
                    Id = 222,
                    Queue = new []
                    {
                        new QueueDetails { PainLevel = 2 }
                    }
                },
                new HospitalDetails
                {
                    Id = 333,
                    Queue = new []
                    {
                        new QueueDetails { PainLevel = 3 }
                    }
                }
            };

            var illnessId = 123;
            var actual = BittnServiceHelper.GetHelpDetails(hospitals, illnessId, painLevel);

            actual.Should().HaveCount(1);
            actual.First().Id.Should().Be(expectedId);
        }

        [Fact]
        public void GetHelpDetails_Should_Handle_No_Matching_Hospitals()
        {
            var hospitals = new []
            {
                new HospitalDetails
                {
                    Queue = new []
                    {
                        new QueueDetails { PainLevel = 1 }
                    }
                }
            };

            var illnessId = 123;
            var painLevel = 999;
            var actual = BittnServiceHelper.GetHelpDetails(hospitals, illnessId, painLevel);

            actual.Should().BeEmpty();
        }
    }
}