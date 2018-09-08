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

        public Guid SeasonId { get; set; }

        [Required]
        [ForeignKey("SeasonId")]
        public virtual Season Season { get; set; }


        public virtual Team PreviousTeamRef { get; set; }

        public Guid PreviousTeamGuid { get; set; }

        public virtual List<Player> Players { get; set; }

        public string Coach { get; set; }

        public virtual List<StatsPerGame> StatsPerGame { get; set; }

        public virtual ICollection<PlayerPerSeason> PlayersPerSeason { get; set; }

        #region Points

        #region Pts

        public int Points
        {
            get
            {
                return StatsPerGame.Sum(spg => spg.Points);
            }
        }

        #endregion

        #region TwoPt

        public int TwoPtAA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).TwoPtA);
            }
            set { }
        }

        public int TwoPtAB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).TwoPtA);
            }
            set { }
        }

        public int TwoPtMA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).TwoPtMade);
            }
            set { }
        }

        public int TwoPtMB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).TwoPtMade);
            }
            set { }
        }

        public double TwoPtPercA
        {
            get
            {
                return (TwoPtMA / TwoPtAA) * 100;
            }
        }

        public double TwoPtPercB
        {
            get
            {
                return (TwoPtMB / TwoPtAB) * 100;
            }
        }

        #endregion

        #region ThreePt

        public int ThreePtAA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).ThreePtA);
            }
            set { }
        }

        public int ThreePtAB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).ThreePtA);
            }
            set { }
        }

        public int ThreePtMA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).ThreePtMade);
            }
            set { }
        }

        public int ThreePtMB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).ThreePtMade);
            }
            set { }
        }

        public double ThreePtPercA
        {
            get
            {
                return (ThreePtMA / ThreePtAA) * 100;
            }
        }

        public double ThreePtPercB
        {
            get
            {
                return (ThreePtMB / ThreePtAB) * 100;
            }
        }

        #endregion ThreePt

        #region Ft

        public int FtAA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).FtA);
            }
            set { }
        }

        public int FtAB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).FtA);
            }
            set { }
        }

        public int FtMA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).FtMade);
            }
            set { }
        }

        public int FtMB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).FtMade);
            }
            set { }
        }

        public double FtPercA
        {
            get
            {
                return (FtMA / FtAA) * 100;
            }
        }

        public double FtPercB
        {
            get
            {
                return (FtMB / FtAB) * 100;
            }
        }

        #endregion

        #region Fg

        public int FgAA
        {
            get
            {
                return TwoPtAA + ThreePtAA;
            }
        }

        public int FgMA
        {
            get
            {
                return TwoPtMA + ThreePtMA;
            }
        }

        public double FgPercA
        {
            get
            {
                return (FgMA / FgAA) * 100;
            }
        }

        public int FgAB
        {
            get
            {
                return TwoPtAB + ThreePtAB;
            }
        }

        public int FgMB
        {
            get
            {
                return TwoPtMB + ThreePtMB;
            }
        }

        public double FgPercB
        {
            get
            {
                return (FgMB / FgAB) * 100;
            }
        }

        #endregion

        #endregion

        #region Rebounds

        public int RebA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Reb);
            }
            set { }
        }

        public int RebB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Reb);
            }
            set { }
        }

        public int ORebA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).OReb);
            }
            set { }
        }

        public int ORebB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).OReb);
            }
            set { }
        }

        public int DRebA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).DReb);
            }
            set { }
        }

        public int DRebB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).DReb);
            }
            set { }
        }

        #endregion

        #region Assists

        public int AstA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Ast);
            }
            set { }
        }

        public int AstB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Ast);
            }
            set { }
        }

        #endregion

        #region TO

        public int ToA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).To);
            }
            set { }
        }

        public int ToB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).To);
            }
            set { }
        }

        #endregion

        #region Steals

        public int StlA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Stl);
            }
            set { }
        }

        public int StlB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Stl);
            }
            set { }
        }

        #endregion

        #region Blocks

        public int BlkA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Blk);
            }
            set { }
        }

        public int BlkB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Blk);
            }
            set { }
        }

        #endregion

        #region Minutes

        public int MinA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).MinutesPlayed);
            }
            set { }
        }

        public int MinB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).MinutesPlayed);
            }
            set { }
        }

        #endregion

        #region Eff

        public int EffA
        {
            get
            {
                return TeamA.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Eff);
            }
            set { }
        }

        public int EffB
        {
            get
            {
                return TeamB.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Eff);
            }
            set { }
        }

        #endregion
    }
}
