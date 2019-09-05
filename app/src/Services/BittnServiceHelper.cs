using System.Collections.Generic;
using System.Linq;
using Bittn.Api.Repositories.Models;
using Bittn.Api.Services.Models;

namespace Bittn.Api.Services
{
    public class BittnServiceHelper
    {
        public static IEnumerable<HelpDetails> GetHelpDetails(IEnumerable<HospitalDetails> source, int illnessId, int painLevel)
        {
            return source
                .Where(item => item.Queue?.Any(q => q.PainLevel == painLevel) == true)
                .Select(item =>
                {
                    var queue = item.Queue.Single(q => q.PainLevel == painLevel);
                    var waitTime = queue.AverageQueueTime * queue.PatientCount;

                    return new HelpDetails
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Location = item.Location,
                        IllnessId = illnessId,
                        PainLevel = painLevel,
                        AverageProcessTime = queue.AverageQueueTime,
                        QueueLength = queue.PatientCount,
                        WaitingTime = waitTime
                    };
                });
        }
    }
}