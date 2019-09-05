using System;
using System.Linq;
using Bittn.Api.Repositories;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Services;
using Bittn.Api.Services.Models;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Bittn.Api.Tests.Services
{
    public class BittnServiceTests
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IBittnRepository _bittnRepository;

        private readonly BittnService _service;

        public BittnServiceTests()
        {
            _hospitalRepository = Substitute.For<IHospitalRepository>();
            _bittnRepository = Substitute.For<IBittnRepository>();

            _service = new BittnService(_hospitalRepository, _bittnRepository);
        }

        [Fact]
        public async void GetConditions_Should_Call_IHospitalRepository_SearchIllnesses()
        {
            SearchIllnessesRequest arg = null;
            _hospitalRepository.SearchIllnesses(Arg.Do<SearchIllnessesRequest>(a => arg = a)).Returns(new SearchIllnessesResponse());

            var request = new GetConditionsRequest
            {
                Name = "name",
                PageIndex = 123
            };

            await _service.GetConditions(request);

            await _hospitalRepository.Received(1).SearchIllnesses(Arg.Any<SearchIllnessesRequest>());
            arg.ItemsPerPage.Should().BeNull();
            arg.IllnessName.Should().Be(request.Name);
            arg.PageIndex.Should().Be(request.PageIndex);
        }

        [Fact]
        public async void GetConditions_Should_Return_Correctly()
        {
            var response = new SearchIllnessesResponse
            {
                CurrentPageIndex = 123,
                NextPageIndex = 456,
                PrevPageIndex = 789,
                Data = new [] { new IllnessDetails() }
            };
            _hospitalRepository.SearchIllnesses(Arg.Any<SearchIllnessesRequest>()).Returns(response);

            var actual = await _service.GetConditions(new GetConditionsRequest());

            actual.NextPageIndex.Should().Be(response.NextPageIndex);
            actual.PrevPageIndex.Should().Be(response.PrevPageIndex);
            actual.Data.Should().BeEquivalentTo(response.Data);
        }

        [Fact]
        public async void FindHelp_Should_Call_IHospitalRepository_SearchHospitals()
        {
            SearchHospitalsRequest arg = null;
            _hospitalRepository
                .SearchHospitals(Arg.Do<SearchHospitalsRequest>(a => arg = a))
                .Returns(new SearchHospitalsResponse
                {
                    Data = new HospitalDetails[0]
                });

            var request = new FindHelpRequest
            {
                PageIndex = 123
            };

            await _service.FindHelp(request);

            await _hospitalRepository.Received(1).SearchHospitals(Arg.Any<SearchHospitalsRequest>());
            arg.ItemsPerPage.Should().BeNull();
            arg.HospitalName.Should().BeNull();
            arg.PageIndex.Should().BeNull();
        }

        [Fact]
        public async void FindHelp_Should_Return_Correctly()
        {
            var hospitalId = 111;
            var painLevel = 1;
            var response = new SearchHospitalsResponse
            {
                CurrentPageIndex = 123,
                NextPageIndex = 456,
                PrevPageIndex = 789,
                Data = new []
                {
                    new HospitalDetails
                    {
                        Id = hospitalId,
                        Queue = new []
                        {
                            new QueueDetails { PainLevel = painLevel }
                        }
                    }
                }
            };
            _hospitalRepository.SearchHospitals(Arg.Any<SearchHospitalsRequest>()).Returns(response);

            var request = new FindHelpRequest { SeverityLevel = painLevel };
            var actual = await _service.FindHelp(request);

            actual.PrevPageIndex.Should().Be(null);
            actual.NextPageIndex.Should().Be(null);
            actual.Data.Should().HaveCount(1);
            actual.Data.First().Id.Should().Be(hospitalId);
        }

        [Fact]
        public async void BookPatient_Should_Call_IBittnRepository_BookPatient()
        {
            BookingDetails arg = null;
            _bittnRepository
                .BookPatient(Arg.Do<BookingDetails>(a => arg = a))
                .Returns(new BookingDetails());

            var request = new BookPatientRequest
            {
                ConditionId = 123,
                ConditionName = "conditionName",
                HelpId = 456,
                HelpName = "helpName",
                PatientName = "patientName",
                SeverityLevel = 789
            };

            await _service.BookPatient(request);

            await _bittnRepository.Received(1).BookPatient(Arg.Any<BookingDetails>());
            arg.ConditionId.Should().Be(request.ConditionId);
            arg.ConditionName.Should().Be(request.ConditionName);
            arg.HelpId.Should().Be(request.HelpId);
            arg.HelpName.Should().Be(request.HelpName);
            arg.PatientName.Should().Be(request.PatientName);
            arg.SeverityLevel.Should().Be(request.SeverityLevel);
            arg.CreateDate.ToString("yyyy-MM-dd").Should().Be(DateTimeOffset.UtcNow.ToString("yyyy-MM-dd"));
        }

        [Fact]
        public async void BookPatient_Should_Return_Correctly()
        {
            var response = new BookingDetails { Id = 123 };
            _bittnRepository.BookPatient(Arg.Any<BookingDetails>()).Returns(response);

            var actual = await _service.BookPatient(new BookPatientRequest());

            actual.Id.Should().Be(response.Id);
        }

        [Fact]
        public async void GetBookings_Should_Call_IBittnRepository_RetrieveBookings()
        {
            RetrieveBookingsRequest arg = null;
            _bittnRepository.RetrieveBookings(Arg.Do<RetrieveBookingsRequest>(a => arg = a)).Returns(new RetrieveBookingsResponse());

            var request = new GetBookingsRequest
            {
                PageIndex = 123
            };

            await _service.GetBookings(request);

            await _bittnRepository.Received(1).RetrieveBookings(Arg.Any<RetrieveBookingsRequest>());
            arg.ItemsPerPage.Should().BeNull();
            arg.PageIndex.Should().Be(request.PageIndex);
        }

        [Fact]
        public async void GetBookings_Should_Return_Correctly()
        {
            var response = new RetrieveBookingsResponse
            {
                CurrentPageIndex = 123,
                NextPageIndex = 456,
                PrevPageIndex = 789,
                Data = new [] { new BookingDetails() }
            };
            _bittnRepository.RetrieveBookings(Arg.Any<RetrieveBookingsRequest>()).Returns(response);

            var actual = await _service.GetBookings(new GetBookingsRequest());

            actual.NextPageIndex.Should().Be(response.NextPageIndex);
            actual.PrevPageIndex.Should().Be(response.PrevPageIndex);
            actual.Data.Should().BeEquivalentTo(response.Data);
        }
    }
}