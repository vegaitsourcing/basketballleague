using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.ViewModels
{
    public class CareerHighStats
    {
        Player Player { get; set; }

        #region Career high pts

        public int Pts
        {
            get
            {
                return Player.CareerHighPts.Pts;
            }
        }

        public DateTime PtsDate
        {
            get
            {
                return Player.CareerHighPts.Game.DateTime;
            }
        }

        public string PtsVS
        {
            get
            {
                string TeamName;
                if (!Player.CareerHighPts.Game.TeamA.Players.Any(p => p.Id == Player.Id))
                {
                    TeamName = Player.CareerHighPts.Game.TeamB.TeamName;
                }
                else
                {
                    TeamName = Player.CareerHighPts.Game.TeamA.TeamName;
                }
                return TeamName;
            }
        }

        #endregion

        #region Career high reb

        public int Reb
        {
            get
            {
                return Player.CareerHighReb.Reb;
            }
        }

        public DateTime RebDate
        {
            get
            {
                return Player.CareerHighReb.Game.DateTime;
            }
        }

        public string RebVS
        {
            get
            {
                string TeamName;
                if (!Player.CareerHighReb.Game.TeamA.Players.Any(p => p.Id == Player.Id))
                {
                    TeamName = Player.CareerHighReb.Game.TeamB.TeamName;
                }
                else
                {
                    TeamName = Player.CareerHighReb.Game.TeamA.TeamName;
                }
                return TeamName;
            }
        }

        #endregion

        #region Career high ast

        public int Ast
        {
            get
            {
                return Player.CareerHighAst.Ast;
            }
        }

        public DateTime AstDate
        {
            get
            {
                return Player.CareerHighAst.Game.DateTime;
            }
        }

        public string AstVS
        {
            get
            {
                string TeamName;
                if (!Player.CareerHighAst.Game.TeamA.Players.Any(p => p.Id == Player.Id))
                {
                    TeamName = Player.CareerHighAst.Game.TeamB.TeamName;
                }
                else
                {
                    TeamName = Player.CareerHighAst.Game.TeamA.TeamName;
                }
                return TeamName;
            }
        }

        #endregion

        #region Career high stl

        public int Stl
        {
            get
            {
                return Player.CareerHighStl.Stl;
            }
        }

        public DateTime StlDate
        {
            get
            {
                return Player.CareerHighStl.Game.DateTime;
            }
        }

        public string StlVS
        {
            get
            {
                string TeamName;
                if (!Player.CareerHighStl.Game.TeamA.Players.Any(p => p.Id == Player.Id))
                {
                    TeamName = Player.CareerHighStl.Game.TeamB.TeamName;
                }
                else
                {
                    TeamName = Player.CareerHighStl.Game.TeamA.TeamName;
                }
                return TeamName;
            }
        }

        #endregion

        #region Career high blk

        public int Blk
        {
            get
            {
                return Player.CareerHighBlk.Blk;
            }
        }

        public DateTime BlkDate
        {
            get
            {
                return Player.CareerHighBlk.Game.DateTime;
            }
        }

        public string BlkVS
        {
            get
            {
                string TeamName;
                if (!Player.CareerHighBlk.Game.TeamA.Players.Any(p => p.Id == Player.Id))
                {
                    TeamName = Player.CareerHighBlk.Game.TeamB.TeamName;
                }
                else
                {
                    TeamName = Player.CareerHighBlk.Game.TeamA.TeamName;
                }
                return TeamName;
            }
        }

        #endregion

        #region Career high eff

        public int Eff
        {
            get
            {
                return Player.CareerHighEff.Eff;
            }
        }

        public DateTime EffDate
        {
            get
            {
                return Player.CareerHighEff.Game.DateTime;
            }
        }

        public string EffVS
        {
            get
            {
                string TeamName;
                if (!Player.CareerHighEff.Game.TeamA.Players.Any(p => p.Id == Player.Id))
                {
                    TeamName = Player.CareerHighEff.Game.TeamB.TeamName;
                }
                else
                {
                    TeamName = Player.CareerHighEff.Game.TeamA.TeamName;
                }
                return TeamName;
            }
        }

        #endregion
    }
}
