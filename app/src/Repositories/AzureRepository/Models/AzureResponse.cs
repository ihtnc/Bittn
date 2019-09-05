namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class AzureResponse<T>
    {
        public T _embedded { get; set; }
        public LinksObject _links { get; set; }
        public PageObject Page { get; set; }
    }
}