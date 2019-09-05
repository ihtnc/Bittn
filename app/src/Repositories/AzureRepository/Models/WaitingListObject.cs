namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class WaitingListObject
    {
        public int PatientCount { get; set; }
        public int LevelOfPain { get; set; }
        public int AverageProcessTime { get; set; }
    }
}