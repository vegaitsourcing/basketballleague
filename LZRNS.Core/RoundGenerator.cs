using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.Core
{
    public class RoundGenerator : IRoundGenerator
    {
        public IEnumerable<Round> GenerateRoundsWithGames(List<Team> teams, LeagueSeason leagueSeason)
        {
            // TODO: Optimize algorithm in order to support double Round Robin brackets
            if (teams.Count % 2 != 0)
            {
                // TODO: Add support for odd number of teams if required. (Add empty bye team or use default empty team from the DB because of FK constraints)
                var byeTeam = new Team
                {
                    Id = Guid.NewGuid(),
                    LeagueSeasonId = leagueSeason.Id,
                    TeamName = "BYE team"
                };

                //teams.Add(byeTeam);
                //yield break; // TODO: Remove when proper odd team number support is defined
            }

            var numberOfRounds = teams.Count - 1;
            var numberOfGamesPerRound = teams.Count / 2;

            var rotatedTeams = new List<Team>();
            //takes the second half of teams
            rotatedTeams.AddRange(teams.Skip(numberOfGamesPerRound).Take(numberOfGamesPerRound));

            rotatedTeams.AddRange(teams.Skip(1).Take(numberOfGamesPerRound - 1).ToArray().Reverse());

            var numberOfTeams = rotatedTeams.Count;

            for (var roundNumber = 0; roundNumber < numberOfRounds; roundNumber++)
            {
                var games = new List<Game>();

                var round = new Round
                {
                    Id = Guid.NewGuid(),
                    Games = new List<Game>(),
                    LeagueSeasonId = leagueSeason.Id,
                    RoundName = string.Format("{0}", roundNumber + 1)
                };

                var teamIndex = roundNumber % numberOfTeams;

                games.Add(new Game
                {
                    Id = Guid.NewGuid(),
                    RoundId = round.Id,
                    SeasonId = leagueSeason.Season.Id,
                    TeamAId = teams[0].Id,
                    //TeamA = teams[0].TeamName,
                    TeamBId = rotatedTeams[teamIndex].Id,
                    DateTime = DateTime.Now // TODO: DateTime is required at the moment, either remove it or set default time here
                });

                for (var index = 1; index < numberOfGamesPerRound; index++)
                {
                    var teamAIndex = (roundNumber + index) % numberOfTeams;
                    var teamBIndex = (roundNumber + numberOfTeams - index) % numberOfTeams;

                    games.Add(new Game
                    {
                        Id = Guid.NewGuid(),
                        RoundId = round.Id,
                        SeasonId = leagueSeason.Season.Id,
                        TeamAId = rotatedTeams[teamBIndex].Id,
                        TeamBId = rotatedTeams[teamAIndex].Id,
                        DateTime = DateTime.UtcNow // TODO: DateTime is required at the moment, either remove it or set default time here
                    });
                }
                round.Games = games;
                yield return round;
            }
        }
    }
}