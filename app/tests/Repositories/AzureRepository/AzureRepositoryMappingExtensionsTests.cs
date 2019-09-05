using System.Collections.Generic;
using Bittn.Api.Repositories.AzureRepository;
using Bittn.Api.Repositories.AzureRepository.Models;
using Bittn.Api.Repositories.Models;
using Xunit;
using FluentAssertions;

namespace Bittn.Api.Tests.Repositories.AzureRepository
{
    public class AzureRepositoryMappingExtensionsTests
    {
        [Fact]
        public void ToSearchIllnessesResponse_Should_Map_Correctly()
        {
            var source = new IllnessesResponse
            {
                _embedded = new IllnessesObject
                {
                    Illnesses = new []
                    {
                        new AzureIllness { Illness = new IllnessObject { Id = 1 } },
                        new AzureIllness { Illness = new IllnessObject { Id = 2 } },
                        new AzureIllness { Illness = new IllnessObject { Id = 3 } }
                    }
                },
                Page = new PageObject { Number = 123 }
            };

            var actual = source.ToSearchIllnessesResponse();

            actual.Data.Should().HaveSameCount(source._embedded.Illnesses);
            actual.CurrentPageIndex.Should().Be(source.Page.Number);
        }

        [Fact]
        public void ToSearchIllnessesResponse_Should_Handle_Null_Embedded()
        {
            var source = new IllnessesResponse();

            var actual = source.ToSearchIllnessesResponse();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToSearchIllnessesResponse_Should_Handle_Null()
        {
            IllnessesResponse source = null;

            var actual = source.ToSearchIllnessesResponse();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToIllnessDetailsList_Should_Map_Correctly()
        {
            var source = new []
            {
                new AzureIllness { Illness = new IllnessObject { Id = 1 } },
                new AzureIllness { Illness = new IllnessObject { Id = 2 } },
                new AzureIllness { Illness = new IllnessObject { Id = 3 } }
            };

            var actual = source.ToIllnessDetailsList();

            actual.Should().HaveSameCount(source);
        }

        [Fact]
        public void ToIllnessDetailsList_Should_Handle_Null()
        {
            List<AzureIllness> source = null;

            var actual = source.ToIllnessDetailsList();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToIllnessDetails_Should_Map_Correctly()
        {
            var source = new AzureIllness
            {
                Illness = new IllnessObject
                {
                    Id = 123,
                    Name = "name"
                }
            };

            var actual = source.ToIllnessDetails();

            actual.Id.Should().Be(source.Illness.Id);
            actual.Name.Should().Be(source.Illness.Name);
        }

        [Fact]
        public void ToIllnessDetails_Should_Handle_Null_Illness()
        {
            var source = new AzureIllness();
            var actual = source.ToIllnessDetails();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToIllnessDetails_Should_Handle_Null()
        {
            AzureIllness source = null;
            var actual = source.ToIllnessDetails();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToSearchHospitalResponse_Should_Map_Correctly()
        {
            var source = new HospitalsResponse
            {
                _embedded = new HospitalsObject
                {
                    Hospitals = new []
                    {
                        new HospitalObject { Id = 1 },
                        new HospitalObject { Id = 2 },
                        new HospitalObject { Id = 3 }
                    }
                },
                Page = new PageObject { Number = 123 }
            };

            var actual = source.ToSearchHospitalsResponse();

            actual.Data.Should().HaveSameCount(source._embedded.Hospitals);
            actual.CurrentPageIndex.Should().Be(source.Page.Number);
        }

        [Fact]
        public void ToSearchHospitalResponse_Should_Handle_Null_Embedded()
        {
            var source = new HospitalsResponse();

            var actual = source.ToSearchHospitalsResponse();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToSearchHospitalResponse_Should_Handle_Null()
        {
            HospitalsResponse source = null;

            var actual = source.ToSearchHospitalsResponse();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToHospitalDetailsList_Should_Map_Correctly()
        {
            var source = new []
            {
                new HospitalObject { Id = 1 },
                new HospitalObject { Id = 2 },
                new HospitalObject { Id = 3 }
            };

            var actual = source.ToHospitalDetailsList();

            actual.Should().HaveSameCount(source);
        }

        [Fact]
        public void ToHospitalDetailsList_Should_Handle_Null()
        {
            List<HospitalObject> source = null;
            var actual = source.ToHospitalDetailsList();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToHospitalDetails_Should_Map_Correctly()
        {
            var source = new HospitalObject
            {
                Id = 123,
                Name = "name",
                Location = new LocationObject(),
                WaitingList = new []
                {
                    new WaitingListObject(),
                    new WaitingListObject()
                }
            };

            var actual = source.ToHospitalDetails();

            actual.Id.Should().Be(source.Id);
            actual.Name.Should().Be(source.Name);
            actual.Location.Should().NotBeNull();
            actual.Queue.Should().HaveSameCount(source.WaitingList);
        }

        [Fact]
        public void ToHospitalDetails_Should_Handle_Null()
        {
            HospitalObject source = null;
            var actual = source.ToHospitalDetails();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToQueueDetailsList_Should_Map_Correctly()
        {
            var source = new []
            {
                new WaitingListObject { LevelOfPain = 1 },
                new WaitingListObject { LevelOfPain = 2 },
                new WaitingListObject { LevelOfPain = 3 }
            };

            var actual = source.ToQueueDetailsList();

            actual.Should().HaveSameCount(source);
        }

        [Fact]
        public void ToQueueDetailsList_Should_Handle_Null()
        {
            List<WaitingListObject> source = null;
            var actual = source.ToQueueDetailsList();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToQueueDetails_Should_Map_Correctly()
        {
            var source = new WaitingListObject
            {
                PatientCount = 123,
                LevelOfPain = 456,
                AverageProcessTime = 789
            };

            var actual = source.ToQueueDetails();

            actual.PatientCount.Should().Be(source.PatientCount);
            actual.PainLevel.Should().Be(source.LevelOfPain);
            actual.AverageQueueTime.Should().Be(source.AverageProcessTime);
        }

        [Fact]
        public void ToQueueDetails_Should_Handle_Null()
        {
            WaitingListObject source = null;
            var actual = source.ToQueueDetails();

            actual.Should().BeNull();
        }

        [Fact]
        public void ToLocationDetails_Should_Map_Correctly()
        {
            var source = new LocationObject
            {
                Lat = 123,
                Lng = 456
            };

            var actual = source.ToLocationDetails();

            actual.Latitude.Should().Be(source.Lat);
            actual.Longitude.Should().Be(source.Lng);
        }

        [Fact]
        public void ToLocationDetails_Should_Handle_Null()
        {
            LocationObject source = null;
            var actual = source.ToLocationDetails();

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData(0, 0, 0, null, null)]
        [InlineData(0, 1, 0, null, 1)]
        [InlineData(1, 1, 1, 0, null)]
        [InlineData(0, 2, 0, null, 1)]
        [InlineData(1, 2, 1, 0, 2)]
        [InlineData(2, 2, 2, 1, null)]
        [InlineData(-1, 1, -1, null, 0)]
        [InlineData(-99, 1, -99, null, 0)]
        [InlineData(3, 2, 3, 2, null)]
        [InlineData(99, 2, 99, 2, null)]
        public void PopulatePageDetails_Should_Map_Correctly(int pageNumber, int totalPages, int expectedCurrent, int? expectedPrev, int? expectedNext)
        {
            var page = new PageObject
            {
                Number = pageNumber,
                TotalPages = totalPages
            };

            var source = new PagedResponse();
            var actual = source.PopulatePageDetails(page);

            actual.CurrentPageIndex.Should().Be(expectedCurrent);
            actual.PrevPageIndex.Should().Be(expectedPrev);
            actual.NextPageIndex.Should().Be(expectedNext);
       }

       [Fact]
       public void PopulatePageDetails_Should_Handle_Null_Page()
       {
            var source = new PagedResponse();
            var actual = source.PopulatePageDetails(null);

            actual.CurrentPageIndex.Should().Be(default);
            actual.PrevPageIndex.Should().BeNull();
            actual.NextPageIndex.Should().BeNull();
       }

       [Fact]
       public void PopulatePageDetails_Should_Handle_Null_Response()
       {
            PagedResponse source = null;
            var actual = source.PopulatePageDetails(new PageObject());

            actual.Should().BeNull();
       }
    }
}