using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.ViewModels
{
    public class TeamDetails
    {
        private Team Team { get; }
        private string RoundName { get; }
        private LeaderboardPlacing Placing { get; }

        public TeamDetails(Team team, string roundName)
        {
            Team = team;
            RoundName = roundName;
            Placing = Team.GetLeaderBoardPlacing(RoundName);
        }

        public string TeamName => Team.TeamName;
        public int Ranking => GetRanking();
        public int Wins => Placing.Wins;
        public int Defeats => Placing.Defeats;
        public string Image => Team.Image;

        public IEnumerable<PlayerInTeamDetails> Players =>
            Team.PlayersPerSeason.Select(x => (PlayerInTeamDetails)x.Player);

        public IEnumerable<GameForTeamDetails> CurrentSeasonGames =>
            GetGamesForSeasonTeam(Team, RoundName);

        public IEnumerable<IGrouping<int, GameForTeamDetails>> GamesHistory => CurrentSeasonGames
            .Concat(GetGamesHistoryByPreviousTeam(Team.PreviousTeamRef))
            .GroupBy(x => x.SeasonStartYear);

        private IEnumerable<GameForTeamDetails> GetGamesForSeasonTeam(Team seasonTeam, string roundName = null) =>
            seasonTeam?.Games
                .OrderByDescending(x => x.DateTime)
                .Where(x => string.IsNullOrWhiteSpace(roundName) ||
                            string.Compare(x.Round.RoundName, roundName, StringComparison.Ordinal) <= 0)
                .Select(x => MapToGameForTeamDetails(x, TeamName)) ?? Enumerable.Empty<GameForTeamDetails>();

        private IEnumerable<GameForTeamDetails> GetGamesHistoryByPreviousTeam(Team team) =>
            team != null ?
                GetGamesForSeasonTeam(team)
                    .Concat(GetGamesHistoryByPreviousTeam(team.PreviousTeamRef)) :
                Enumerable.Empty<GameForTeamDetails>();

        private int GetRanking() =>
            Team.LeagueSeason.Teams
                .Select(x => x.GetLeaderBoardPlacing(RoundName))
                .OrderByDescending(x => x.Pts)
                .ThenByDescending(x => x.Diff)
                .ThenBy(x => x.TeamName)
                .ToList()
                .Select((t, i) => new { team = t, index = i })
                .First(x => x.team.TeamName.Equals(TeamName))
                .index
            + 1;

        private GameForTeamDetails MapToGameForTeamDetails(Game g, string teamName) =>
            new GameForTeamDetails(
                g.Season.SeasonStartYear,
                g.Round.RoundName,
                g.TeamA.TeamName.Equals(teamName) ? g.TeamB.TeamName : g.TeamA.TeamName,
                g.DateTime,
                $"{g.StatsPerGameA.Pts} : {g.StatsPerGameB.Pts}",
                g.TeamA.TeamName.Equals(teamName) ? g.StatsPerGameA : g.StatsPerGameB
            );
    }
}