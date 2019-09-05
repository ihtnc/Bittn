namespace Bittn.Api.Services.Models
{
    public class PagedRequest
    {
        public int? PageIndex { get; set; }
        public string SortField { get; set; }
        public bool? SortDescending { get; set; }
    }
}