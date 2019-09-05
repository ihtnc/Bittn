namespace Bittn.Api.Services.Models
{
    public class FindHelpRequest : PagedRequest
    {
        public int ConditionId { get; set; }
        public int SeverityLevel { get; set; }
    }
}