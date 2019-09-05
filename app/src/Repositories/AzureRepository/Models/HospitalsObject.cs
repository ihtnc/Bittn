using System.Collections.Generic;

namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class HospitalsObject
    {
        public IEnumerable<HospitalObject> Hospitals { get; set; }
    }
}