using Bittn.Api.Repositories.Models;

namespace Bittn.Api.Services.Models
{
    public class HelpDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IllnessId { get; set; }
        public int PainLevel { get; set; }
        public int QueueLength { get; set; }
        public int AverageProcessTime { get; set; }
        public int WaitingTime { get; set; }
        public LocationDetails Location { get; set; }
    }
}