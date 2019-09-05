using System.Collections.Generic;

namespace Bittn.Api.Repositories.AzureRepository.Models
{
    public class IllnessesObject
    {
        public IEnumerable<AzureIllness> Illnesses { get; set; }
    }
}