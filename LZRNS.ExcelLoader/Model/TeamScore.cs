using System;
using System.Collections.Generic;

namespace LZRNS.ExcelLoader
{
    public class TeamScore
    {
        private List<PlayerScore> playerScores;
        private string againstTeam = "-";

        public TeamScore()
        {
            this.playerScores = new List<PlayerScore>();
        }
        

        #region Properties
        public string AgainstTeam { get { return againstTeam; } set { againstTeam = value; } }
        public DateTime MatchDate { get; set; }
        public string RoundName { get; set; }
        public List<PlayerScore> PlayerScores { get { return playerScores; } }
        #endregion Properties

        #region Public Methods
        public void AddPlayerScore (PlayerScore pl)
        {
            playerScores.Add(pl);
        }
        #endregion Public Methods
        
    }
}
