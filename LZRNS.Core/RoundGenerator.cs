using LZRNS.Common.Extensions;
using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.Core
{
    public class RoundGenerator : IRoundGenerator
    {
        private readonly IRoundScheduler _scheduler;

        public RoundGenerator(IRoundScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public IEnumerable<Round> GenerateRoundsWithGames(IReadOnlyList<Team> teams, LeagueSeason leagueSeason, RoundScheduleOptions options = null)
        {
            var rounds = teams.Count.IsEven()
                ? GenerateRoundsForEvenNumberOfTeams(teams, leagueSeason)
                : GenerateRoundsForOddNumberOfTeams(teams, leagueSeason);

            return _scheduler.ScheduleRounds(rounds, options);
        }

        private static IEnumerable<Round> GenerateRoundsForOddNumberOfTeams(IEnumerable<Team> teams, LeagueSeason leagueSeason)
        {
            var evenTeams = AddInvalidTeam(teams);
            var rounds = GenerateRoundsForEvenNumberOfTeams(evenTeams, leagueSeason);
            return RemovePairsWithInvalidTeam(rounds);
        }

        private static IEnumerable<Round> GenerateRoundsForEvenNumberOfTeams(IReadOnlyList<Team> teams, LeagueSeason leagueSeason)
        {
            int numberOfRounds = teams.Count - 1;
            int numberOfGamesPerRound = teams.Count / 2;

            var rotatedTeams = TakeSecondHalfOfTeams(teams, numberOfGamesPerRound);
            int numberOfTeams = rotatedTeams.Count;

            var generatedRounds = new List<Round>();

            for (int roundNumber = 0; roundNumber < numberOfRounds; roundNumber++)
            {
                var games = new List<Game>();

                var round = new Round
                {
                    Id = Guid.NewGuid(),
                    Games = new List<Game>(),
                    LeagueSeasonId = leagueSeason.Id,
                    RoundName = $"{roundNumber + 1}"
                };

                int teamIndex = roundNumber % numberOfTeams;

                games.Add(new Game
                {
                    Id = Guid.NewGuid(),
                    RoundId = round.Id,
                    SeasonId = leagueSeason.Season.Id,
                    TeamAId = teams[0].Id,
                    TeamBId = rotatedTeams[teamIndex].Id,
                    DateTime = DateTime.Now
                });

                for (int index = 1; index < numberOfGamesPerRound; index++)
                {
                    int teamAIndex = (roundNumber + index) % numberOfTeams;
                    int teamBIndex = (roundNumber + numberOfTeams - index) % numberOfTeams;

                    games.Add(new Game
                    {
                        Id = Guid.NewGuid(),
                        RoundId = round.Id,
                        SeasonId = leagueSeason.Season.Id,
                        TeamAId = rotatedTeams[teamBIndex].Id,
                        TeamBId = rotatedTeams[teamAIndex].Id,
                        DateTime = DateTime.UtcNow
                    });
                }
                round.Games = games;
                generatedRounds.Add(round);
            }

            return generatedRounds;
        }

        private static List<Team> TakeSecondHalfOfTeams(IReadOnlyList<Team> teams, int numberOfGamesPerRound)
        {
            var rotatedTeams = new List<Team>();
            rotatedTeams.AddRange(teams.Skip(numberOfGamesPerRound).Take(numberOfGamesPerRound));
            rotatedTeams.AddRange(teams.Skip(1).Take(numberOfGamesPerRound - 1).ToArray().Reverse());
            return rotatedTeams;
        }

        private static List<Team> AddInvalidTeam(IEnumerable<Team> teams)
        {
            var evenTeams = new List<Team>(teams);
            var invalidTeam = CreateInvalidTeam();
            evenTeams.Add(invalidTeam);
            return evenTeams;
        }

        private static Team CreateInvalidTeam()
        {
            return new Team { Id = Guid.Empty, TeamName = "INVALID_TEAM" };
        }

        private static IEnumerable<Round> RemovePairsWithInvalidTeam(IEnumerable<Round> rounds)
        {
            return rounds.Select(RemovePairsWithInvalidTeamFromRound).ToList();
        }

        private static Round RemovePairsWithInvalidTeamFromRound(Round round)
        {
            var validRound = (Round)round.Clone();
            validRound.Games = round.Games.Where(game => !DoesContainInvalidTeam(game)).ToList();
            return validRound;
        }

        private static bool DoesContainInvalidTeam(Game game)
        {
            return game.TeamAId.Equals(Guid.Empty) || game.TeamBId.Equals(Guid.Empty);
        }
    }
}