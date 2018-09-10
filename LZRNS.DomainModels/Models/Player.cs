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

        //[Required] when we are importing data from history (round statistic, we do not have this information)
        [Range(0, 250)]
        public int Height { get; set; }

        //[Required] when we are importing data from history (round statistic, we do not have this information)
        [Range(0, 250)]
        public int Weight { get; set; }


        [Range(1900, 2100)]
        public int YearOfBirth { get; set; }

        public virtual ICollection<Stats> Stats { get; set; }

        public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

        #region Career high


        public Stats CareerHighPts
        {
            get
            {
                return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Pts).First() : null;
            }
        }

        public Stats CareerHighReb
        {
            get
            {
                return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Reb).First() : null;
            }
        }

        public Stats CareerHighAst
        {
            get
            {
                return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Ast).First() : null;
            }
        }

        public Stats CareerHighStl
        {
            get
            {
                return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Stl).First() : null;
            }
        }

        public Stats CareerHighBlk
        {
            get
            {
                return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Blk).First() : null;
            }
        }

        public Stats CareerHighEff
        {
            get
            {
                return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Eff).First() : null;
            }
        }

        public string GetFullName
        {
            get
            {
                return Name + " " + LastName;
            }
        }

        #endregion
    }
}
