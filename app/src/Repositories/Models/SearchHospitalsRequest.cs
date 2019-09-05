namespace Bittn.Api.Repositories.Models
{
    public class SearchHospitalsRequest : PagedRequest
    {
        public string HospitalName { get; set; }
    }
}