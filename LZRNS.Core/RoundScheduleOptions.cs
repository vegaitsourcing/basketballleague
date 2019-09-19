using System;

namespace LZRNS.Core
{
    public class RoundScheduleOptions
    {
        public RoundScheduleOptions(DateTime? firstRoundStartDate = null, TimeSpan? roundStartTime = null, uint intervalBetweenRoundsInDays = 7, uint intervalBetweenGamesInMinutes = 120)
        {
            FirstRoundStartDate = firstRoundStartDate ?? DateTime.Today;
            IntervalBetweenRoundsInDays = intervalBetweenRoundsInDays;
            IntervalBetweenGamesInMinutes = intervalBetweenGamesInMinutes;
            RoundStartTime = roundStartTime ?? TimeSpan.FromHours(13);
        }

        /// <summary>
        /// Start date of the first round to schedule (time information is not important)
        /// </summary>
        public DateTime FirstRoundStartDate { get; set; }

        public uint IntervalBetweenGamesInMinutes { get; set; }
        public uint IntervalBetweenRoundsInDays { get; set; }

        /// <summary>
        /// Default start time of each round (date information is not important)
        /// </summary>
        public TimeSpan RoundStartTime { get; set; }
    }
}