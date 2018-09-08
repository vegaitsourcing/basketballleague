﻿using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Models
{
    public class StatsPerGame
    {
        public virtual Game Game { get; }

        public Guid GameId { get; }

        public virtual Team Team { get; }

        public Guid TeamId { get; }

        #region Points

        #region Pts

        public int Pts
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).Pts);
            }
        }

        #endregion

        #region TwoPt

        public int TwoPtA
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).TwoPtA);
            }
        }

        public int TwoPtM
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).TwoPtMade);
            }
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
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).ThreePtA);
            }
        }

        public int ThreePtM
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).ThreePtMade);
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
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).FtA);
            }
        }

        public int FtM
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).FtMade);
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
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).Reb);
            }
        }

        public int OReb
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).OReb);
            }
        }

        public int DReb
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).DReb);
            }
        }
        
        #endregion

        #region Assists

        public int Ast
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).Ast);
            }
        }

        #endregion

        #region TO

        public int To
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).To);
            }
        }

        #endregion

        #region Steals

        public int Stl
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).Stl);
            }
        }

        #endregion

        #region Blocks

        public int Blk
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).Blk);
            }
        }

        #endregion

        #region Minutes

        public int Min
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).MinutesPlayed);
            }
        }

        #endregion

        #region Eff

        public int Eff
        {
            get
            {
                return Team.Players.Sum(p => p.Stats.FirstOrDefault(s => s.PlayerId == p.Id && s.GameId == GameId).Eff);
            }
        }

        #endregion
    }
}
