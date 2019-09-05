using Newtonsoft.Json;

namespace Bittn.Api.Models
{
    public class BookPatientApiRequest
    {
        [JsonRequired]
        public string PatientName { get; set; }
        [JsonRequired]
        public int ConditionId { get; set; }
        [JsonRequired]
        public string ConditionName { get; set; }
        [JsonRequired]
        public int Severity { get; set; }
        [JsonRequired]
        public int HelpId { get; set; }
        [JsonRequired]
        public string HelpName { get; set; }
    }
}