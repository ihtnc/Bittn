using System.Collections.Generic;

namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class HospitalObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<WaitingListObject> WaitingList { get; set; }
        public LocationObject Location { get; set; }
    }
}