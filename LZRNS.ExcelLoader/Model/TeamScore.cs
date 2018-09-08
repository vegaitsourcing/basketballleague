using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.ExcelLoader
{
    class TeamScore
    {
        private List<PlayerScore> playerScores;

        public TeamScore()
        {
            this.playerScores = new List<PlayerScore>();
        }
        

        #region Properties
        public string AgainstTeam { get; set; }
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
