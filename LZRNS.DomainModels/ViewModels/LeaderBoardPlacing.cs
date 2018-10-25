using System;
using System.Linq;
using LZRNS.DomainModel.Models;
using System.Collections.Generic;
using LZRNS.DomainModels.Models;

namespace LZRNS.DomainModels.ViewModels
{
	public class LeaderboardPlacing
	{
		private string RoundName { get; }
		private Team Team { get; }

		public LeaderboardPlacing(string roundName, Team team)
		{
			this.RoundName = roundName;
			this.Team = team;
		}

		public string TeamName => this.Team.TeamName;
		public int Pts => 2 * Wins;
		public int Wins => this.GamesWinners.Count(x => x == this.Team.Id);
		public int Defeats => this.GamesWinners.Count(x => x != this.Team.Id);
		public int Diff => ScoredPts - ReceivedPts;

		public int ScoredPts => this.GamesTillRound
			.Select(x => x.TeamAId == this.Team.Id ?
				x.StatsPerGameA : x.StatsPerGameB)
			.Sum(y => y.Pts);

		public int ReceivedPts => this.GamesTillRound
			.Select(x => x.TeamAId != this.Team.Id ?
				x.StatsPerGameA : x.StatsPerGameB)
			.Sum(y => y.Pts);


		private IEnumerable<StatsPerGame> TeamStatsPerGame => 
			this.GamesTillRound
				.Select(x => x.TeamAId == this.Team.Id ? 
					x.StatsPerGameA : x.StatsPerGameB);

		private IEnumerable<Game> GamesTillRound => 
			this.Team.Games
				.Where(x => x.Round.RoundName.CompareTo(this.RoundName) <= 0);

		private IEnumerable<Guid> GamesWinners => 
			this.GamesTillRound
				.Select(x => x.StatsPerGameA.Pts > x.StatsPerGameB.Pts?
				x.TeamAId : x.TeamBId);

	}
}
