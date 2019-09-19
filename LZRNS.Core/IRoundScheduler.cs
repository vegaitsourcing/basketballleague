using LZRNS.DomainModels.Models;
using System.Collections.Generic;

namespace LZRNS.Core
{
    public interface IRoundScheduler
    {
        IEnumerable<Round> ScheduleRounds(IEnumerable<Round> rounds, RoundScheduleOptions options);
    }
}