using System.Threading.Tasks;
using Bittn.Api.Repositories.Models;

namespace Bittn.Api.Repositories
{
    public interface IHospitalRepository
    {
        Task<SearchIllnessesResponse> SearchIllnesses(SearchIllnessesRequest request);
        Task<SearchHospitalsResponse> SearchHospitals(SearchHospitalsRequest request);
    }
}