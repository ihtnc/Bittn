using System.Collections.Generic;

namespace Bittn.Api.Services.Models
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int? PrevPageIndex { get; set; }
        public int? NextPageIndex { get; set; }
    }
}