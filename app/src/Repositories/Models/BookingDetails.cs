using System;
namespace Bittn.Api.Repositories.Models
{
    public class BookingDetails
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public int ConditionId { get; set; }
        public string ConditionName { get; set; }
        public int SeverityLevel { get; set; }
        public int HelpId { get; set; }
        public string HelpName { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}