using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.Core
{
    public class RoundScheduler : IRoundScheduler
    {
        public IEnumerable<Round> ScheduleRounds(IEnumerable<Round> rounds, RoundScheduleOptions options = null)
        {
            if (options is null)
            {
                options = new RoundScheduleOptions();
            }

            return rounds.Select((round, index) => ScheduleRound(round, index, options)).ToList();
        }

        private static Round ScheduleRound(Round round, int roundIndex, RoundScheduleOptions options)
        {
            var scheduledRound = (Round)round.Clone();
            var startDate = GetStartDateForRound(roundIndex, options);
            scheduledRound.Games = ScheduleGames(round, options, startDate);
            return scheduledRound;
        }

        private static DateTime GetStartDateForRound(int roundIndex, RoundScheduleOptions options)
        {
            return options.FirstRoundStartDate.AddDays(roundIndex * options.IntervalBetweenRoundsInDays).ChangeTime(hours: 0) + options.RoundStartTime;
        }

        private static List<Game> ScheduleGames(Round round, RoundScheduleOptions options, DateTime startDate)
        {
            return round.Games.Select((game, index) => ScheduleGame(game, index, startDate, options)).ToList();
        }

        private static Game ScheduleGame(Game game, int gameIndex, DateTime startDate, RoundScheduleOptions options)
        {
            var scheduledGame = (Game)game.Clone();
            scheduledGame.DateTime = GetStartDateForGame(gameIndex, startDate, options);
            return scheduledGame;
        }

        private static DateTime GetStartDateForGame(int gameIndex, DateTime startDate, RoundScheduleOptions options)
        {
            return startDate.AddMinutes(gameIndex * options.IntervalBetweenGamesInMinutes);
        }
    }
}