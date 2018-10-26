using System.Collections.Generic;
using System.Linq;
using LZRNS.DomainModel.Models;

namespace LZRNS.DomainModels.ViewModels
{
	public class TeamDetails
	{
		private Team Team { get; }
		private string RoundName { get; }
		private LeaderboardPlacing Placing { get; }

		public TeamDetails(Team team, string roundName)
		{
			this.Team = team;
			this.RoundName = roundName;
			this.Placing = Team.GetLeaderBoardPlacing(this.RoundName);
		}

		public string TeamName => this.Team.TeamName;
		public int Ranking => GetRanking();
		public int Wins => this.Placing.Wins;
		public int Defeats => this.Placing.Defeats;
		public string Image => this.Team.Image;

		public IEnumerable<PlayerInTeamDetails> Players =>
			this.Team.PlayersPerSeason.Select(x => (PlayerInTeamDetails)x.Player);
		public IEnumerable<GameForTeamDetails> CurrentSeasonGames =>
			GetGamesForSeasonTeam(this.Team, this.RoundName);
		public IEnumerable<IGrouping<int, GameForTeamDetails>> GamesHistory => this.CurrentSeasonGames
			.Concat(GetGamesHistoryByPreviousTeam(this.Team.PreviousTeamRef))
			.GroupBy(x => x.SeasonStartYear);

		#region [Private Methods]

		private IEnumerable<GameForTeamDetails> GetGamesForSeasonTeam(Team seasonTeam, string roundName = null) =>
			seasonTeam?.Games
				.OrderByDescending(x => x.DateTime)
				.Where(x => string.IsNullOrWhiteSpace(roundName) ||
						x.Round.RoundName.CompareTo(roundName) <= 0)
				.Select(x => MapToGameForTeamDetails(x, this.TeamName)) ?? Enumerable.Empty<GameForTeamDetails>();

		private IEnumerable<GameForTeamDetails> GetGamesHistoryByPreviousTeam(Team team) =>
			team != null ?
				GetGamesForSeasonTeam(team)
					.Concat(GetGamesHistoryByPreviousTeam(team.PreviousTeamRef)) :
				Enumerable.Empty<GameForTeamDetails>();


		private int GetRanking() =>
			this.Team.LeagueSeason.Teams
				.Select(x => x.GetLeaderBoardPlacing(this.RoundName))
				.OrderByDescending(x => x.Pts)
				.ThenByDescending(x => x.Diff)
				.ThenBy(x => x.TeamName)
				.ToList()
				.Select((t, i) => new { team = t, index = i })
				.First(x => x.team.TeamName.Equals(this.TeamName))
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

		#endregion
	}
}
