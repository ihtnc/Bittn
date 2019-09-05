namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class PageObject
    {
        public int Size { get; set; }
        public int TotalElements { get; set; }
        public int TotalPages { get; set; }
        public int Number { get; set; }
    }
}