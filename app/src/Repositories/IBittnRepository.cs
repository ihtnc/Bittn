using System.Threading.Tasks;
using Bittn.Api.Repositories.Models;

namespace Bittn.Api.Repositories
{
    public interface IBittnRepository
    {
        Task<BookingDetails> BookPatient(BookingDetails request);
        Task<RetrieveBookingsResponse> RetrieveBookings(RetrieveBookingsRequest request);
    }
}