using System;
using System.Collections.Generic;

namespace LZRNS.ExcelLoader
{
    public class TeamStatistic
    {
        private String teamName = "-";
        private List<TeamScore> teamScores;

        public TeamStatistic(String tName)
        {
            this.teamName = tName;
            teamScores = new List<TeamScore>();
        }

        #region Properies
        public String TeamName
        {
            get { return teamName; }
        }

        public List<TeamScore> TeamScores
        {
            get { return teamScores; }
        }
        #endregion Properies

        #region Public Methods
        public void AddTeamScore (TeamScore ts)
        {
            teamScores.Add(ts);
        }
        #endregion Public Methods


    }
}
