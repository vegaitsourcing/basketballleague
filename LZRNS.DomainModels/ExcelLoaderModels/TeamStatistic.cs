using System.Collections.Generic;

namespace LZRNS.DomainModels.ExcelLoaderModels
{
    public class TeamStatistic
    {
        public TeamStatistic(string tName)
        {
            TeamName = tName ?? "-";
            TeamScores = new List<TeamScore>();
        }

        public string TeamName { get; }

        public List<TeamScore> TeamScores { get; }

        public void AddTeamScore(TeamScore ts)
        {
            TeamScores.Add(ts);
        }
    }
}