using System.Threading.Tasks;
using Bittn.Api.Repositories.Models;

namespace Bittn.Api.Repositories
{
    public interface IBittnRepository
    {
        Task<BookingDetails> AddBooking(BookingDetails request);
        Task<RetrieveBookingsResponse> RetrieveBookings(RetrieveBookingsRequest request);
        Task<DeleteBookingResponse> DeleteBooking(DeleteBookingRequest request);
    }
}