namespace Bittn.Api.Repositories.Models
{
    public class SearchIllnessesRequest : PagedRequest
    {
        public string IllnessName { get; set; }
    }
}