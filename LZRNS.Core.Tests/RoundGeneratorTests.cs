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
        private readonly LeagueSeason _leagueSeason;
        private readonly RoundGenerator _sut;

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
            var teams = GenerateTeams(numberOfTeams: 2);

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            var round = Assert.Single(roundsWithGames);
            Assert.NotNull(round);

            var game = Assert.Single(round.Games);
            Assert.NotNull(game);
        }

        [Fact]
        public void GenerateRoundsWithGames_FourTeams_GenerateThreeRoundsWithTwoGamesEach()
        {
            var teams = GenerateTeams(numberOfTeams: 4);

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            Assert.Equal(3, roundsWithGames.Count);
            Assert.All(roundsWithGames, round => Assert.Equal(2, round.Games.Count));
        }

        [Fact]
        public void GenerateRoundsWithGames_FourTeams_TeamIsContainedInEachRoundOnce()
        {
            var teams = GenerateTeams(numberOfTeams: 4);

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            Assert.All(roundsWithGames, round => AssertEachTeamIsContainedInRoundOnce(teams, round));
        }

        [Fact]
        public void GenerateRoundsWithGames_ThreeTeams_EachTeamIsNotPlayingInOnlyOneRound()
        {
            var teams = GenerateTeams(numberOfTeams: 3);

            var roundsWithGames = _sut.GenerateRoundsWithGames(teams, _leagueSeason).ToList();

            Assert.All(teams, team => AssertTeamIsMissingInOnlyOneRound(team, roundsWithGames));
        }

        private static void AssertTeamIsMissingInOnlyOneRound(Team team, IEnumerable<Round> roundsWithGames)
        {
            var roundsWithoutTeam = roundsWithGames.Where(round => !DoesRoundContainTeam(round, team)).ToList();
            Assert.Single(roundsWithoutTeam);
        }

        private static bool DoesRoundContainTeam(Round round, Team team)
        {
            return round.Games.Any(game => DoesGameContainTeam(game, team));
        }

        private static bool DoesGameContainTeam(Game game, Team team)
        {
            return game.TeamAId.Equals(team.Id) || game.TeamBId.Equals(team.Id);
        }

        private static void AssertEachTeamIsContainedInRoundOnce(IEnumerable<Team> teams, Round round)
        {
            var gameTeamIds = round.Games.SelectMany(game => new[] { game.TeamAId, game.TeamBId }).ToList();

            Assert.All(teams, team =>
            {
                Assert.Single(gameTeamIds, id => id.Equals(team.Id));
            });
        }

        private static Team GenerateTeam(int index)
        {
            return new Team { Id = Guid.NewGuid(), TeamName = $"T{index}" };
        }

        private static List<Team> GenerateTeams(int numberOfTeams)
        {
            return Enumerable.Range(1, numberOfTeams).Select(GenerateTeam).ToList();
        }
    }
}