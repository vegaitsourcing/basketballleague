using System;
using System.Collections.Generic;

namespace LZRNS.ExcelLoader
{
    public class TeamScore
    {
        public TeamScore()
        {
            PlayerScores = new List<PlayerScore>();
        }

        public string AgainstTeam { get; set; } = "-";
        public DateTime MatchDate { get; set; }
        public List<PlayerScore> PlayerScores { get; }
        public string RoundName { get; set; }

        public void AddPlayerScore(PlayerScore pl)
        {
            PlayerScores.Add(pl);
        }
    }
}