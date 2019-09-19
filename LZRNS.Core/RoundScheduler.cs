using LZRNS.DomainModels.Models;
using System.Collections.Generic;

namespace LZRNS.Core
{
    public class RoundScheduler : IRoundScheduler
    {
        public IEnumerable<Round> ScheduleRounds(IEnumerable<Round> rounds)
        {
            return rounds;
        }
    }
}