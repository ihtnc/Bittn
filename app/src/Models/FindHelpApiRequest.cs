using Newtonsoft.Json;

namespace Bittn.Api.Models
{
    public class FindHelpApiRequest
    {
        [JsonRequired]
        public int ConditionId { get; set; }
        [JsonRequired]
        public int Severity { get; set; }
        public int? Page { get; set; }
        public string SortField { get; set; }
        public bool? SortDescending { get; set; }
    }
}