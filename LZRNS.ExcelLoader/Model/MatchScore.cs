using System;

namespace LZRNS.ExcelLoader
{
    class MatchScore
    {
        private TeamScore teamA;
        private TeamScore teamB;
        private DateTime matchDate;


        public MatchScore(TeamScore teamA, TeamScore teamB, DateTime matchDate)
        {
            this.teamA = teamA;
            this.teamB = teamB;
            this.matchDate = matchDate;
        }


    }
}
