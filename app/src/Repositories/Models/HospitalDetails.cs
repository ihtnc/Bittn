using System.Collections.Generic;

namespace Bittn.Api.Repositories.Models
{
    public class HospitalDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<QueueDetails> Queue { get; set; }
        public LocationDetails Location { get; set; }
    }
}