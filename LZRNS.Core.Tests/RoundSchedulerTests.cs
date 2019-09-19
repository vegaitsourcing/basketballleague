using LZRNS.Common.Extensions;
using LZRNS.Core;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LZRNS.Core.Tests
{
    public class RoundSchedulerTests
    {
        private readonly RoundScheduler _sut;

        public RoundSchedulerTests()
        {
            _sut = new RoundScheduler();
        }

        [Fact]
        public void ScheduleRounds_SetsStartDateOfFirstGameInFirstRoundAccordingToOptions()
        {
            var rounds = GetRoundsWithGames(2);
            var options = new RoundScheduleOptions(firstRoundStartDate: DateTime.Today.AddDays(2));

            var scheduledRounds = _sut.ScheduleRounds(rounds, options);
            var firstRound = scheduledRounds.First();
            var firstGame = firstRound.Games.First();

            Assert.True(AreSameDay(firstGame.DateTime, options.FirstRoundStartDate), $"Expected '{firstGame.DateTime}' to be same day as '{options.FirstRoundStartDate}'");
        }

        private static bool AreSameDay(DateTime firstDate, DateTime secondDate)
        {
            return firstDate.Year == secondDate.Year
                   && firstDate.Month == secondDate.Month
                   && firstDate.Day == secondDate.Day;
        }

        private static ICollection<Game> GetGames(int numberOfGames = 3)
        {
            return Enumerable.Range(1, numberOfGames).Select(_ => new Game()).ToList();
        }

        private static ICollection<Round> GetRoundsWithGames(int numberOfRounds = 3, int numberOfGames = 3)
        {
            var games = GetGames(numberOfGames);
            return Enumerable.Range(1, numberOfRounds).Select(index => new Round { RoundName = $"R{index}", Games = games }).ToList();
        }
    }
}