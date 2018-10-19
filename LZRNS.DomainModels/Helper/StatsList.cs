using LZRNS.DomainModel.Models;
using System.Collections.Generic;
using System.Linq;

namespace LZRNS.DomainModels.Helper
{
	public static class StatsList
	{
		public static List<int> TotalStats(Team team)
		{
			List<int> totalStats = new List<int>();
			int totalWins = 0;
			int totalLosts = 0;
			int totalPtsScored = 0;
			int totalPtsReceived = 0;
			if(team.Games != null && team.Games.Any())
			{
				foreach (Game game in team.Games)
				{
					if (game.TeamAId == team.Id)
					{
						totalPtsScored += game.TeamA.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts;
						totalPtsReceived += game.TeamB.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts;

						if ((game.TeamA.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts) >
							(game.TeamB.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts))
						{
							totalWins++;
						}
						else
						{
							totalLosts++;
						}
					}
					else if ((game.TeamBId == team.Id))
					{
						totalPtsScored += game.TeamB.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts;
						totalPtsReceived += game.TeamA.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts;

						if ((game.TeamB.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts) >
							(game.TeamA.StatsPerGame.FirstOrDefault(spg => spg.GameId == game.Id).Pts))
						{
							totalWins++;
						}
						else
						{
							totalLosts++;
						}
					}
				}
			}
			totalStats.Add(totalWins);
			totalStats.Add(totalLosts);
			totalStats.Add(totalPtsScored);
			totalStats.Add(totalPtsReceived);
			return totalStats;
		}
	}
}
