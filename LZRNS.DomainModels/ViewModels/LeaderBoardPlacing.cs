using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.ViewModels
{
    public class LeaderboardPlacing
    {
        private string RoundName { get; }
        private Team Team { get; }

        public LeaderboardPlacing(string roundName, Team team)
        {
            RoundName = roundName;
            Team = team;
        }

        public string TeamName => Team.TeamName;
        public int Pts => 2 * Wins;
        public int Wins => GamesWinners.Count(x => x == Team.Id);
        public int Defeats => GamesWinners.Count(x => x != Team.Id);
        public int Diff => ScoredPts - ReceivedPts;

        public int ScoredPts => GamesTillRound
            .Select(x => x.TeamAId == Team.Id ?
                x.StatsPerGameA : x.StatsPerGameB)
            .Sum(y => y.Pts);

        public int ReceivedPts => GamesTillRound
            .Select(x => x.TeamAId != Team.Id ?
                x.StatsPerGameA : x.StatsPerGameB)
            .Sum(y => y.Pts);

        private IEnumerable<StatsPerGame> TeamStatsPerGame =>
            GamesTillRound
                .Select(x => x.TeamAId == Team.Id ?
                    x.StatsPerGameA : x.StatsPerGameB);

        private IEnumerable<Game> GamesTillRound =>
            Team.Games
                .Where(x => string.Compare(x.Round.RoundName, RoundName, StringComparison.Ordinal) <= 0);

        private IEnumerable<Guid> GamesWinners =>
            GamesTillRound
                .Select(x => x.StatsPerGameA.Pts > x.StatsPerGameB.Pts ?
                x.TeamAId : x.TeamBId);
    }
}