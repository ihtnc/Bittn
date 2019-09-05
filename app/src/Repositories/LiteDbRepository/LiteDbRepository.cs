using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bittn.Api.Repositories.Models;

namespace Bittn.Api.Repositories.AzureRepository
{
    public class LiteDbRepository : IBittnRepository
    {
        private const int DEFAULT_ITEMS_PER_PAGE = 10;

        private readonly ILiteDbProvider _provider;

        public LiteDbRepository(ILiteDbProvider provider)
        {
            _provider = provider;
        }

        public Task<BookingDetails> BookPatient(BookingDetails request)
        {
            BookingDetails added;

            using(var db = _provider.GetDatabase())
            {
                var bookings = db.GetCollection<BookingDetails>("bookings");
                bookings.EnsureIndex(x => x.Id, true);

                bookings.Insert(request);

                var id = request.Id;
                added = bookings.FindById(id);
            }

            return Task.FromResult(added);
        }

        public Task<RetrieveBookingsResponse> RetrieveBookings(RetrieveBookingsRequest request)
        {
            RetrieveBookingsResponse response;
            IEnumerable<BookingDetails> items;

            using(var db = _provider.GetDatabase())
            {
                var bookings = db.GetCollection<BookingDetails>("bookings");
                items = bookings.FindAll();
            }

            items = items.Sort(request.SortField, request.SortDescending);

            var itemsPerPage = request.ItemsPerPage ?? DEFAULT_ITEMS_PER_PAGE;
            var currentIndex = request.PageIndex ?? 0;
            var maxItems = itemsPerPage + (itemsPerPage * currentIndex);

            var paged = items.Skip(maxItems - itemsPerPage).Take(itemsPerPage);
            var lastPageIndex = items.Count() / itemsPerPage;
            var prevIndex = currentIndex > 0 ? currentIndex - 1 : (int?)null;
            var nextIndex = currentIndex < lastPageIndex ? currentIndex + 1 : (int?)null;

            response = new RetrieveBookingsResponse
            {
                Data = paged,
                CurrentPageIndex = currentIndex,
                PrevPageIndex = prevIndex,
                NextPageIndex = nextIndex
            };

            return Task.FromResult(response);
        }
    }
}