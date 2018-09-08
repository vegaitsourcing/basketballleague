using LZRNS.DomainModels.Helper;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class Team : AbstractModel
    {
        public string TeamName { get; set; }

        public string Image { get; set; }

        public virtual Team PreviousTeamRef { get; set; }

        public Guid PreviousTeamGuid { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public string Coach { get; set; }

        [NotMapped]
        public virtual ICollection<StatsPerGame> StatsPerGame { get; set; }

        public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual LeagueSeason LeagueSeason { get; set; }

        public Guid LeagueSeasonId { get; set; }

        public Guid TeamScoreId { get; set; }

        #region Points

        #region Pts

        public int Pts
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Pts);
            }
        }

        #endregion

        #region TwoPt

        public int TwoPtA
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.TwoPtA);
            }
        }

        public int TwoPtM
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.TwoPtM);
            }
        }

        public double TwoPtPerc
        {
            get
            {
                return (TwoPtM / TwoPtA) * 100;
            }
        }

        #endregion

        #region ThreePt

        public int ThreePtA
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.ThreePtA);
            }
        }

        public int ThreePtM
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.ThreePtM);
            }
        }

        public double ThreePtPerc
        {
            get
            {
                return (ThreePtM / ThreePtA) * 100;
            }
        }

        #endregion ThreePt

        #region Ft

        public int FtA
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.FtA);
            }
        }

        public int FtM
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.FtM);
            }
        }

        public double FtPerc
        {
            get
            {
                return (FtM / FtA) * 100;
            }
        }

        #endregion

        #region Fg

        public int FgA
        {
            get
            {
                return TwoPtA + ThreePtA;
            }
        }

        public int FgM
        {
            get
            {
                return TwoPtM + ThreePtM;
            }
        }

        public double FgPerc
        {
            get
            {
                return (FgM / FgA) * 100;
            }
        }

        #endregion

        #endregion

        #region Rebounds

        public int Reb
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Reb);
            }
        }

        public int OReb
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.OReb);
            }
        }

        public int DReb
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.DReb);
            }
        }

        #endregion

        #region Assists

        public int Ast
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Ast);
            }
        }

        #endregion

        #region TO

        public int To
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.To);
            }
        }

        #endregion

        #region Steals

        public int Stl
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Stl);
            }
        }

        #endregion

        #region Blocks

        public int Blk
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Blk);
            }
        }

        #endregion

        #region Minutes

        public int Min
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Min);
            }
        }

        #endregion

        #region Eff

        public int Eff
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Eff);
            }
        }

        #endregion

        #region Stats per season

        public int Points
        {
            get
            {
                return 2 * StatsList.TotalStats(this)[0] + StatsList.TotalStats(this)[1];
            }
        }

        public int Wins
        {
            get
            {
                return StatsList.TotalStats(this)[0];
            }
        }

        public int Losts
        {
            get
            {
                return StatsList.TotalStats(this)[1];
            }
        }

        public double WLPerc
        {
            get
            {
                return Wins / (Wins + Losts) * 100;
            }
        }

        public int TotalPtsScored
        {
            get
            {
                return StatsList.TotalStats(this)[2];
            }
        }

        public int TotalPtsReceived
        {
            get
            {
                return StatsList.TotalStats(this)[3];
            }
        }

        public int TotalPtsDifference
        {
            get
            {
                return TotalPtsScored - TotalPtsReceived;
            }
        }

        #endregion
    }
}
