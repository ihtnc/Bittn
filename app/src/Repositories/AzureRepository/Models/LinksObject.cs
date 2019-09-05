namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class LinksObject
    {
        public LinkObject Self { get; set; }
        public LinkObject Next { get; set; }
        public LinkObject Prev { get; set; }
    }
}