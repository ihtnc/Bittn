using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Bittn.Api.Config;
using Bittn.Api.Http;
using Bittn.Api.Repositories;
using Bittn.Api.Repositories.AzureRepository;
using Bittn.Api.Repositories.AzureRepository.Models;
using Bittn.Api.Repositories.Models;
using Repository = Bittn.Api.Repositories.AzureRepository.AzureRepository;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Bittn.Api.Tests.Repositories
{
    public class AzureRepositoryTests
    {
        private readonly string _database;
        private readonly IApiClient _apiClient;
        private readonly IApiRequestProvider _requestProvider;

        private readonly IHospitalRepository _repository;

        public AzureRepositoryTests()
        {
            _database = "databaseUrl";

            var options = Substitute.For<IOptions<HospitalConfig>>();
            options.Value.Returns(new HospitalConfig { Database = _database });

            _apiClient = Substitute.For<IApiClient>();
            _requestProvider = Substitute.For<IApiRequestProvider>();

            _repository = new Repository(options, _apiClient, _requestProvider);
        }

        [Theory]
        [InlineData(123, 456, "123", "456")]
        [InlineData(456, null, "456", null)]
        [InlineData(null, 123, null, "123")]
        [InlineData(null, null, null, null)]
        public async void SearchIllnesses_Should_Call_IApiRequestProvider_CreateGetRequest(int? itemsPerPage, int? pageIndex, string expectedLimit, string expectedPage)
        {
            var details = new SearchIllnessesRequest
            {
                ItemsPerPage = itemsPerPage,
                PageIndex = pageIndex
            };
            await _repository.SearchIllnesses(details);

            var query = new List<string>();
            if(expectedLimit != null) { query.Add($"limit={expectedLimit}"); }
            if(expectedPage != null) { query.Add($"page={expectedPage}"); }
            var queryString = query.Any() ? $"?{string.Join("&", query)}" : string.Empty;

            var url = $"{_database}/illnesses{queryString}";
            _requestProvider.Received(1).CreateGetRequest(url);
        }

        [Fact]
        public async void SearchIllnesses_Should_Call_IApiClient_Send()
        {
            Func<HttpResponseMessage, Task<IllnessesResponse>> mapper = null;
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);
            await _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Do<Func<HttpResponseMessage, Task<IllnessesResponse>>>(a => mapper = a));

            await _repository.SearchIllnesses(new SearchIllnessesRequest());

            await _apiClient.Received(1).Send<IllnessesResponse>(request, Arg.Any<Func<HttpResponseMessage, Task<IllnessesResponse>>>());

            var expected = new IllnessesResponse
            {
                _embedded = new IllnessesObject
                {
                    Illnesses = new []
                    {
                        new AzureIllness
                        {
                            Illness = new IllnessObject { Id = 1 }
                        }
                    }
                },
                Page = new PageObject { Number = 123 }
            };
            var body = JObject.FromObject(expected);

            var response = new HttpResponseMessage();
            response.Content = new StringContent(body.ToString());
            var actual = await mapper(response);
            actual.Should().BeEquivalentTo(expected);

            request.Dispose();
        }

        [Fact]
        public async void SearchIllnesses_Should_Call_Return_Correctly()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);

            var response = new IllnessesResponse
            {
                _embedded = new IllnessesObject
                {
                    Illnesses = new []
                    {
                        new AzureIllness
                        {
                            Illness = new IllnessObject { Id = 1 }
                        },
                        new AzureIllness
                        {
                            Illness = new IllnessObject { Id = 2 }
                        },
                        new AzureIllness
                        {
                            Illness = new IllnessObject { Id = 3 }
                        }
                    }
                },
                Page = new PageObject
                {
                    Number = 123,
                    Size = 456,
                    TotalElements = 789,
                    TotalPages = 111
                }
            };
            var expected = response.ToSearchIllnessesResponse();

            _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<Func<HttpResponseMessage, Task<IllnessesResponse>>>()).Returns(response);

            var actual = await _repository.SearchIllnesses(new SearchIllnessesRequest());
            actual.Should().BeEquivalentTo(expected);

            request.Dispose();
        }

        [Theory]
        [InlineData("a", 1)]
        [InlineData("z", 0)]
        [InlineData("o", 4)]
        [InlineData("p", 9)]
        public async void SearchIllnesses_Should_Call_IRequestProvider_CreateGetRequest_Until_Page_Is_Full(string name, int expectedExtraCall)
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);

            var response = new IllnessesResponse
            {
                _embedded = new IllnessesObject
                {
                    Illnesses = new []
                    {
                        new AzureIllness { Illness = new IllnessObject { Name = "aza" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "zao" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "2za" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "aaz" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "zae" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "Zqw" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "eZr" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "zzZ" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "jkz" } },
                        new AzureIllness { Illness = new IllnessObject { Name = "zop" } }
                    }
                },
                _links = new LinksObject
                {
                    Next = new LinkObject { Href = "nextUrl" }
                }
            };
            _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<Func<HttpResponseMessage, Task<IllnessesResponse>>>()).Returns(response);

            var url = $"{_database}/illnesses";

            var search = new SearchIllnessesRequest { IllnessName = name };
            await _repository.SearchIllnesses(search);

            _requestProvider.Received(1).CreateGetRequest(url);
            _requestProvider.Received(expectedExtraCall).CreateGetRequest(response._links.Next.Href);

            request.Dispose();
        }

        [Fact]
        public async void SearchIllnesses_Should_Call_IRequestProvider_CreateGetRequest_Until_There_Are_No_More_Pages()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);

            var embedded = new IllnessesObject
            {
                Illnesses = new []
                {
                    new AzureIllness { Illness = new IllnessObject { Name = "aaa" } }
                }
            };
            var withNext = new LinksObject { Next = new LinkObject { Href = "nextUrl" } };
            var withoutNext = new LinksObject { Next = new LinkObject() };
            var response1 = new IllnessesResponse
            {
                _embedded = embedded,
                _links = withNext
            };
            var response2 = new IllnessesResponse
            {
                _embedded = embedded,
                _links = withNext
            };
            var response3 = new IllnessesResponse
            {
                _embedded = embedded,
                _links = withoutNext
            };
            _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<Func<HttpResponseMessage, Task<IllnessesResponse>>>()).Returns(response1, response2, response3);

            var url = $"{_database}/illnesses";

            var search = new SearchIllnessesRequest { IllnessName = "aa" };
            await _repository.SearchIllnesses(search);

            _requestProvider.Received(1).CreateGetRequest(url);
            _requestProvider.Received(2).CreateGetRequest(withNext.Next.Href);

            request.Dispose();
        }

        [Theory]
        [InlineData(123, 456, "123", "456")]
        [InlineData(456, null, "456", null)]
        [InlineData(null, 123, null, "123")]
        public async void SearchHospitals_Should_Call_IApiRequestProvider_CreateGetRequest(int? itemsPerPage, int? pageIndex, string expectedLimit, string expectedPage)
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);

            var details = new SearchHospitalsRequest
            {
                ItemsPerPage = itemsPerPage,
                PageIndex = pageIndex
            };
            await _repository.SearchHospitals(details);

            var query = new List<string>();
            if(expectedLimit != null) { query.Add($"limit={expectedLimit}"); }
            if(expectedPage != null) { query.Add($"page={expectedPage}"); }
            var queryString = query.Any() ? $"?{string.Join("&", query)}" : string.Empty;

            var url = $"{_database}/hospitals{queryString}";
            _requestProvider.Received(1).CreateGetRequest(url);

            request.Dispose();
        }

        [Fact]
        public async void SearchHospitals_Should_Call_IApiClient_Send()
        {
            Func<HttpResponseMessage, Task<HospitalsResponse>> mapper = null;
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);
            await _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Do<Func<HttpResponseMessage, Task<HospitalsResponse>>>(a => mapper = a));

            await _repository.SearchHospitals(new SearchHospitalsRequest { PageIndex = 0 });

            await _apiClient.Received(1).Send<HospitalsResponse>(request, Arg.Any<Func<HttpResponseMessage, Task<HospitalsResponse>>>());

            var expected = new HospitalsResponse
            {
                _embedded = new HospitalsObject
                {
                    Hospitals = new [] { new HospitalObject() }
                },
                Page = new PageObject { Number = 123 }
            };
            var body = JObject.FromObject(expected);

            var response = new HttpResponseMessage();
            response.Content = new StringContent(body.ToString());
            var actual = await mapper(response);
            actual.Should().BeEquivalentTo(expected);

            request.Dispose();
        }

        [Fact]
        public async void SearchHospitals_Should_Call_Return_Correctly()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);

            var response = new HospitalsResponse
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
                Page = new PageObject
                {
                    Number = 123,
                    Size = 456,
                    TotalElements = 789,
                    TotalPages = 111
                }
            };
            var expected = response.ToSearchHospitalsResponse();

            _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<Func<HttpResponseMessage, Task<HospitalsResponse>>>()).Returns(response);

            var actual = await _repository.SearchHospitals(new SearchHospitalsRequest { PageIndex = 0 });
            actual.Should().BeEquivalentTo(expected);

            request.Dispose();
        }

        [Fact]
        public async void SearchHospitals_Should_Call_IRequestProvider_CreateGetRequest_Until_There_Are_No_More_Pages()
        {
            var request = new HttpRequestMessage();
            _requestProvider.CreateGetRequest(Arg.Any<string>()).Returns(request);

            var embedded = new HospitalsObject
            {
                Hospitals = new []
                {
                    new HospitalObject { }
                }
            };
            var withNext = new LinksObject { Next = new LinkObject { Href = "nextUrl" } };
            var withoutNext = new LinksObject { Next = new LinkObject() };
            var response1 = new HospitalsResponse
            {
                _embedded = embedded,
                _links = withNext
            };
            var response2 = new HospitalsResponse
            {
                _embedded = embedded,
                _links = withNext
            };
            var response3 = new HospitalsResponse
            {
                _embedded = embedded,
                _links = withoutNext
            };
            _apiClient.Send(Arg.Any<HttpRequestMessage>(), Arg.Any<Func<HttpResponseMessage, Task<HospitalsResponse>>>()).Returns(response1, response2, response3);

            var url = $"{_database}/hospitals";

            var search = new SearchHospitalsRequest();
            await _repository.SearchHospitals(search);

            _requestProvider.Received(1).CreateGetRequest(url);
            _requestProvider.Received(2).CreateGetRequest(withNext.Next.Href);

            request.Dispose();
        }
    }
}