using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Bittn.Api.Config;
using Bittn.Api.Http;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Repositories.AzureRepository.Models;

namespace Bittn.Api.Repositories.AzureRepository
{
    public class AzureRepository : IHospitalRepository
    {
        private const int DEFAULT_ITEMS_PER_PAGE = 10;

        private readonly HospitalConfig _config;
        private readonly IApiClient _apiClient;
        private readonly IApiRequestProvider _requestProvider;

        public AzureRepository(IOptions<HospitalConfig> options, IApiClient apiClient, IApiRequestProvider requestProvider)
        {
            _config = options.Value ?? throw new InvalidOperationException("Missing config.");
            _apiClient = apiClient;
            _requestProvider = requestProvider;
        }

        public async Task<SearchIllnessesResponse> SearchIllnesses(SearchIllnessesRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.IllnessName))
            {
                var nonFiltered = await SearchAzureIllnesses(request.ItemsPerPage, request.PageIndex);
                return nonFiltered.ToSearchIllnessesResponse();
            }

            var illnesses = await SearchAzureIllnesses(null, null);
            var list = new List<IllnessDetails>();
            var next = illnesses._links?.Next?.Href;
            var items = illnesses._embedded.Illnesses.ToIllnessDetailsList();
            var filtered = items.Where(i => i.Name.Contains(request.IllnessName, StringComparison.OrdinalIgnoreCase));
            list.AddRange(filtered);

            var limit = request.ItemsPerPage ?? DEFAULT_ITEMS_PER_PAGE;
            var page = request.PageIndex ?? 0;
            var maxItems = limit + (limit * page);

            while(list.Count() < maxItems && !string.IsNullOrWhiteSpace(next))
            {
                illnesses = await SearchAzureIllnesses(next);
                next = illnesses._links?.Next?.Href;
                var nextItems = illnesses._embedded.Illnesses.ToIllnessDetailsList();
                var nextFiltered = nextItems.Where(i => i.Name.Contains(request.IllnessName));
                list.AddRange(nextFiltered);
            }

            var response = list.GetPage(limit, page);
            var lastPageIndex = list.Count() / limit;
            var currentIndex = request.PageIndex ?? 0;
            var prevIndex = currentIndex > 0 ? currentIndex - 1 : (int?)null;
            var nextIndex = currentIndex < lastPageIndex ? currentIndex + 1 : (int?)null;

            return new SearchIllnessesResponse
            {
                Data = response,
                CurrentPageIndex = currentIndex,
                PrevPageIndex = prevIndex,
                NextPageIndex = nextIndex
            };
        }

        public async Task<SearchHospitalsResponse> SearchHospitals(SearchHospitalsRequest request)
        {
            if(request.PageIndex != null || request.ItemsPerPage != null)
            {
                var paged = await SearchAzureHospitals(request.ItemsPerPage, request.PageIndex);
                return paged.ToSearchHospitalsResponse();
            }

            var illnesses = await SearchAzureHospitals(null, null);
            var list = new List<HospitalObject>();
            var next = illnesses._links?.Next?.Href;
            var items = illnesses._embedded.Hospitals;
            list.AddRange(items);

            while(!string.IsNullOrWhiteSpace(next))
            {
                illnesses = await SearchAzureHospitals(next);
                next = illnesses._links?.Next?.Href;
                var nextItems = illnesses._embedded.Hospitals;
                list.AddRange(nextItems);
            }

            return new SearchHospitalsResponse
            {
                Data = list.ToHospitalDetailsList(),
                CurrentPageIndex = 0,
                PrevPageIndex = null,
                NextPageIndex = null
            };
        }

        private async Task<IllnessesResponse> SearchAzureIllnesses(int? limit, int? page)
        {
            var url = $"{_config.Database.TrimEnd('/')}/illnesses";

            var queries = new Dictionary<string, string>();
            if(limit.HasValue) { queries.Add("limit", $"{limit}"); }
            if(page.HasValue) { queries.Add("page", $"{page}"); }

            var list = queries.Select(q => $"{q.Key}={q.Value}");
            var queryString = list.Any() ? $"?{string.Join("&", list)}" : string.Empty;

            var response = await SearchAzureIllnesses($"{url}{queryString}");
            return response;
        }

        private async Task<IllnessesResponse> SearchAzureIllnesses(string url)
        {
            var get = _requestProvider.CreateGetRequest(url);
            var response = await _apiClient.Send(get, async res =>
            {
                var body = await res.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<IllnessesResponse>(body);
                return obj;
            });

            return response;
        }

        private async Task<HospitalsResponse> SearchAzureHospitals(int? limit, int? page)
        {
            var url = $"{_config.Database.TrimEnd('/')}/hospitals";

            var queries = new Dictionary<string, string>();
            if(limit.HasValue) { queries.Add("limit", $"{limit}"); }
            if(page.HasValue) { queries.Add("page", $"{page}"); }

            var list = queries.Select(q => $"{q.Key}={q.Value}");
            var queryString = list.Any() ? $"?{string.Join("&", list)}" : string.Empty;

            var response = await SearchAzureHospitals($"{url}{queryString}");
            return response;
        }

        private async Task<HospitalsResponse> SearchAzureHospitals(string url)
        {
            var get = _requestProvider.CreateGetRequest(url);
            var response = await _apiClient.Send(get, async res =>
            {
                var body = await res.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<HospitalsResponse>(body);
                return obj;
            });

            return response;
        }
    }
}