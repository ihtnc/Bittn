namespace Bittn.Api.Repositories.Models
{
    public class RetrieveBookingsRequest : PagedRequest
    {
        public string SortField { get; set; }
        public bool? SortDescending { get; set; }
    }
}