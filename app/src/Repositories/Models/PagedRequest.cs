namespace Bittn.Api.Repositories.Models
{
    public class PagedRequest
    {
        public int? ItemsPerPage { get; set; }
        public int? PageIndex { get; set; }
    }
}