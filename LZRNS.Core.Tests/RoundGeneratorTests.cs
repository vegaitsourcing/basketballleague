using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LZRNS.Core.Tests
{
    public class RoundGeneratorTests
    {
        private readonly RoundGenerator _sut;
        private readonly LeagueSeason _leagueSeason;

        public RoundGeneratorTests()
        {
            _sut = new RoundGenerator();

            var season = new Season();
            var league = new League();
            _leagueSeason = new LeagueSeason
            {
                Id = Guid.NewGuid(),
                League = league,
                LeagueId = league.Id,
                Season = season,
                SeasonId = season.Id
            };
        }

        [Fact]
        public void GenerateRoundsWithGames_TwoTeams_GeneratesOneRoundWithOneGame()
        {
            var teams = new[]
            {
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), TeamName = "T1"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), TeamName = "T2"},
            };

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            var round = Assert.Single(roundsWithGames);
            Assert.NotNull(round);

            var game = Assert.Single(round.Games);
            Assert.NotNull(game);
        }

        [Fact]
        public void GenerateRoundsWithGames_FourTeams_GenerateThreeRoundsWithTwoGamesEach()
        {
            var teams = new[]
            {
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), TeamName = "T1"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), TeamName = "T2"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), TeamName = "T3"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), TeamName = "T4"},
            };

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            Assert.Equal(3, roundsWithGames.Count);
            Assert.All(roundsWithGames, round => Assert.Equal(2, round.Games.Count));
        }

        [Fact]
        public void GenerateRoundsWithGames_FourTeams_TeamIsContainedInEachRoundOnce()
        {
            var teams = new[]
            {
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), TeamName = "T1"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), TeamName = "T2"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), TeamName = "T3"},
                new Team {Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), TeamName = "T4"},
            };

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            Assert.All(roundsWithGames, round => AssertEachTeamIsContainedInRoundOnce(teams, round));
        }

        private static void AssertEachTeamIsContainedInRoundOnce(IEnumerable<Team> teams, Round round)
        {
            var gameTeamIds = round.Games.SelectMany(game => new[] { game.TeamAId, game.TeamBId }).ToList();

            Assert.All(teams, team =>
            {
                Assert.Single(gameTeamIds, id => id.Equals(team.Id));
            });
        }
    }
}