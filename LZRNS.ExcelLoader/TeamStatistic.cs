using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    class TeamStatistic
    {
        private String teamName;
        private List<TeamScore> teamScores;

        public TeamStatistic(String tName)
        {
            this.teamName = tName;
            teamScores = new List<TeamScore>();
        }

        public String TeamName
        {
            get { return teamName; }
        }

        public List<TeamScore> TeamScores
        {
            get { return teamScores; }
        }
    }
}
