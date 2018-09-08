using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Models
{
    public class StatsPerGame
    {
        Game Game { get; }

        Team Team { get; }

        #region Points

        #region Pts
        
        public int Points
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Pts);
            }
        }

        #endregion

        #region TwoPt

        public int TwoPtA
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).TwoPtA);
            }
            set { }
        }

        public int TwoPtM
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).TwoPtMade);
            }
            set { }
        }

        public double TwoPtPercA
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
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).ThreePtA);
            }
            set { }
        }

        public int ThreePtM
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).ThreePtMade);
            }
            set { }
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
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).FtA);
            }
            set { }
        }

        public int FtM
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).FtMade);
            }
            set { }
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
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Reb);
            }
            set { }
        }

        public int OReb
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).OReb);
            }
            set { }
        }

        public int DReb
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).DReb);
            }
            set { }
        }
        
        #endregion

        #region Assists

        public int Ast
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Ast);
            }
            set { }
        }

        #endregion

        #region TO

        public int To
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).To);
            }
            set { }
        }

        #endregion

        #region Steals

        public int Stl
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Stl);
            }
            set { }
        }

        #endregion

        #region Blocks

        public int Blk
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Blk);
            }
            set { }
        }

        #endregion

        #region Minutes

        public int Min
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).MinutesPlayed);
            }
            set { }
        }

        #endregion

        #region Eff

        public int Eff
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id).Eff);
            }
            set { }
        }

        #endregion
    }
}
