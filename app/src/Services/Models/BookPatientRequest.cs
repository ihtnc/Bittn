namespace Bittn.Api.Services.Models
{
    public class BookPatientRequest
    {
        public string PatientName { get; set; }
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
        public int SeverityLevel { get; set; }
        public int HelpId { get; set; }
        public string HelpName { get; set; }
    }
}