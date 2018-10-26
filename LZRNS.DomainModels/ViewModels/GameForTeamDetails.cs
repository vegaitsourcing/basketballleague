using System;
using LZRNS.DomainModels.Models;

namespace LZRNS.DomainModels.ViewModels
{
	public class GameForTeamDetails
	{
		public GameForTeamDetails(int seasonStartYear, string roundName, string teamName, DateTime dateTime, string result, StatsPerGame statsPerGame)
		{
			RoundName = roundName;
			TeamName = teamName;
			DateTime = dateTime;
			Result = result;
			StatsPerGame = statsPerGame;
			SeasonStartYear = seasonStartYear;
		}

		public string RoundName { get; }
		public string TeamName { get; }
		public DateTime DateTime { get; }
		public string Result { get; }
		public StatsPerGame StatsPerGame { get; }
		public int SeasonStartYear { get; }
	}
}