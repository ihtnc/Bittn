using System.Collections.Generic;

namespace Bittn.Api.Repositories.Models
{
    public class PagedResponse<T> : PagedResponse
    {
        public IEnumerable<T> Data { get; set; }
    }

    public class PagedResponse
    {
        public int? PrevPageIndex { get; set; }
        public int CurrentPageIndex { get; set; }
        public int? NextPageIndex { get; set; }
    }
}