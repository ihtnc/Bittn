using System;
using System.Linq;
using System.Threading.Tasks;
using Bittn.Api.Repositories;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Services.Models;

namespace Bittn.Api.Services
{
    public interface IBittnService
    {
        Task<GetConditionsResponse> GetConditions(GetConditionsRequest request);
        Task<FindHelpResponse> FindHelp(FindHelpRequest request);

        Task<BookPatientResponse> BookPatient(BookPatientRequest request);
        Task<GetBookingsResponse> GetBookings(GetBookingsRequest request);
        Task<CancelBookingResponse> CancelBooking(CancelBookingRequest request);
    }

    public class BittnService : IBittnService
    {
        private const int DEFAULT_ITEMS_PER_PAGE = 10;

        private readonly IHospitalRepository _hospitalRepository;
        private readonly IBittnRepository _bittnRepository;

        public BittnService(IHospitalRepository hospitalRepository, IBittnRepository bittnRepository)
        {
            _hospitalRepository = hospitalRepository;
            _bittnRepository = bittnRepository;
        }

        public async Task<GetConditionsResponse> GetConditions(GetConditionsRequest request)
        {
            var illnesses = await _hospitalRepository.SearchIllnesses(new SearchIllnessesRequest
            {
                IllnessName = request.Name,
                PageIndex = request.PageIndex
            });

            return new GetConditionsResponse
            {
                Data = illnesses.Data,
                PrevPageIndex = illnesses.PrevPageIndex,
                NextPageIndex = illnesses.NextPageIndex
            };
        }

        public async Task<FindHelpResponse> FindHelp(FindHelpRequest request)
        {
            var hospitals = await _hospitalRepository.SearchHospitals(new SearchHospitalsRequest());

            var help = BittnServiceHelper.GetHelpDetails(hospitals.Data, request.ConditionId, request.SeverityLevel);
            var sorted = help.Sort(request.SortField, request.SortDescending);
            var paged = sorted.GetPage(DEFAULT_ITEMS_PER_PAGE, request.PageIndex);

            var lastPageIndex = help.Count() / DEFAULT_ITEMS_PER_PAGE;
            var currentIndex = request.PageIndex ?? 0;
            var prevIndex = currentIndex > 0 ? currentIndex - 1 : (int?)null;
            var nextIndex = currentIndex < lastPageIndex ? currentIndex + 1 : (int?)null;

            return new FindHelpResponse
            {
                Data = paged,
                PrevPageIndex = prevIndex,
                NextPageIndex = nextIndex
            };
        }

        public async Task<BookPatientResponse> BookPatient(BookPatientRequest request)
        {
            var added = await _bittnRepository.AddBooking(new BookingDetails
            {
                ConditionId = request.ConditionId,
                ConditionName = request.ConditionName,
                HelpId = request.HelpId,
                HelpName = request.HelpName,
                PatientName = request.PatientName,
                SeverityLevel = request.SeverityLevel,
                CreateDate = DateTimeOffset.UtcNow
            });

            return new BookPatientResponse
            {
                Id = added.Id
            };
        }
        public async Task<GetBookingsResponse> GetBookings(GetBookingsRequest request)
        {
            var response = await _bittnRepository.RetrieveBookings(new RetrieveBookingsRequest
            {
                PageIndex = request.PageIndex,
                SortField = request.SortField,
                SortDescending = request.SortDescending
            });

            return new GetBookingsResponse
            {
                Data = response.Data,
                PrevPageIndex = response.PrevPageIndex,
                NextPageIndex = response.NextPageIndex
            };
        }

        public async Task<CancelBookingResponse> CancelBooking(CancelBookingRequest request)
        {
            var response = await _bittnRepository.DeleteBooking(new DeleteBookingRequest
            {
                Id = request.BookingId
            });
            return new CancelBookingResponse
            {
                Deleted = response.Deleted
            };
        }
    }
}