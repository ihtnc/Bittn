using System.Collections.Generic;
using System.Linq;
using Bittn.Api.Repositories.AzureRepository.Models;
using Bittn.Api.Repositories.Models;

namespace Bittn.Api.Repositories.AzureRepository
{
    public static class AzureRepositoryMappingExtensions
    {
        public static SearchIllnessesResponse ToSearchIllnessesResponse(this IllnessesResponse source)
        {
            if(source == null || source._embedded == null) { return null; }

            var response = new SearchIllnessesResponse
            {
                Data = source._embedded.Illnesses.ToIllnessDetailsList(),
            };

            return response.PopulatePageDetails(source.Page);
        }

        public static IEnumerable<IllnessDetails> ToIllnessDetailsList(this IEnumerable<AzureIllness> source)
        {
            return source?.Select(ToIllnessDetails);
        }

        public static IllnessDetails ToIllnessDetails(this AzureIllness source)
        {
            return source == null || source.Illness == null ? null : new IllnessDetails
            {
                Id = source.Illness.Id,
                Name = source.Illness.Name
            };
        }

        public static SearchHospitalsResponse ToSearchHospitalsResponse(this HospitalsResponse source)
        {
            if(source == null || source._embedded == null) { return null; }

            var response = new SearchHospitalsResponse
            {
                Data = source._embedded.Hospitals.ToHospitalDetailsList()
            };

            return response.PopulatePageDetails(source.Page);
        }

        public static IEnumerable<HospitalDetails> ToHospitalDetailsList(this IEnumerable<HospitalObject> source)
        {
            return source?.Select(ToHospitalDetails);
        }

        public static HospitalDetails ToHospitalDetails(this HospitalObject source)
        {
            return source == null ? null : new HospitalDetails
            {
                Id = source.Id,
                Name = source.Name,
                Location = source.Location.ToLocationDetails(),
                Queue = source.WaitingList.ToQueueDetailsList()
            };
        }

        public static IEnumerable<QueueDetails> ToQueueDetailsList(this IEnumerable<WaitingListObject> source)
        {
            return source?.Select(ToQueueDetails);
        }

        public static QueueDetails ToQueueDetails(this WaitingListObject source)
        {
            return source == null ? null : new QueueDetails
            {
                PatientCount = source.PatientCount,
                PainLevel = source.LevelOfPain,
                AverageQueueTime = source.AverageProcessTime
            };
        }

        public static LocationDetails ToLocationDetails(this LocationObject source)
        {
            return source == null ? null : new LocationDetails
            {
                Latitude = source.Lat,
                Longitude = source.Lng
            };
        }

        public static T PopulatePageDetails<T>(this T source, PageObject page)
            where T : PagedResponse
        {
            if(source == null || page == null) { return source; }

            int? prev = null, next = null;
            if(page.Number < 0) { next = 0; }
            else if(page.Number > page.TotalPages) { prev = page.TotalPages; }
            else
            {
                prev = page.Number > 0 ? page.Number - 1 : (int?)null;
                next = page.Number < page.TotalPages ? page.Number + 1 : (int?)null;
            }

            source.CurrentPageIndex = page.Number;
            source.PrevPageIndex = prev;
            source.NextPageIndex = next;
            return source;
        }
    }
}