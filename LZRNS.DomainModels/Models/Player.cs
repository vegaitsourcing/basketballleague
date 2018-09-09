using LZRNS.DomainModels.Models;
using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LZRNS.DomainModels.Helper;

namespace LZRNS.DomainModel.Models
{
    public class Player : AbstractModel
    {
        [Required]
        public string Name { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Image { get; set; }

        [Required]
        [Range(0, 250)]
        public int Height { get; set; }

        [Required]
        [Range(0, 250)]
        public int Weight { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int YearOfBirth { get; set; }

        public virtual ICollection<Stats> Stats { get; set; }

        public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

        #region Career high

        public CareerHigh Pts
        {
            get
            {
                Stats stats = Stats.OrderByDescending(s => s.Pts).First();
                Guid gameId = Stats.OrderByDescending(s => s.Pts).First().GameId;
                string TeamName;
                if (stats.Game.TeamA.Id == gameId)
                {
                    TeamName = stats.Game.TeamA.TeamName;
                }
                else
                {
                    TeamName = stats.Game.TeamB.TeamName;
                }
                CareerHigh careerHigh = new CareerHigh()
                {
                    Quantity = stats.Pts,
                    DateTime = stats.Game.DateTime,
                    OpsiteTeamName = TeamName
                };
                return careerHigh;
            }
        }

        public int Reb
        {
            get
            {
                return Stats.OrderByDescending(s => s.Reb).First().Reb;
            }
        }

        public int Ast
        {
            get
            {
                return Stats.OrderByDescending(s => s.Ast).First().Ast;
            }
        }

        public int Stl
        {
            get
            {
                return Stats.OrderByDescending(s => s.Stl).First().Stl;
            }
        }

        public int Blk
        {
            get
            {
                return Stats.OrderByDescending(s => s.Blk).First().Blk;
            }
        }

        public int Eff
        {
            get
            {
                return Stats.OrderByDescending(s => s.Eff).First().Eff;
            }
        }

        #endregion
    }
}
