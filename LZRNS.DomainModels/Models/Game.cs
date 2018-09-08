using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LZRNS.DomainModel.Models
{
    public class Game : AbstractModel
    {
        public virtual Season Season { get; set; }

        [ForeignKey("Season")]
        public Guid SeasonId { get; set; }

        [Required]
        [ForeignKey("RoundId")]
        public virtual Round Round { get; set; }

        public Guid RoundId { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [ForeignKey("TeamAId")]
        public virtual Team TeamA { get; set; }

        public Guid TeamAId { get; set; }

        [Required]
        [ForeignKey("TeamBId")]
        public virtual Team TeamB { get; set; }

        public Guid TeamBId { get; set; }

        public virtual ICollection<Referee> Referees { get; set; }

        public virtual ICollection<StatsPerGame> StatsPerGame { get; set; }

        public StatsPerGame StatsPerGameA { get; }

        public StatsPerGame StatsPerGameB { get; set; }

        public Team ReturnTeam(Guid id)
        {
            if (TeamAId == id)
            {
                return TeamA;
            }
            else if (TeamBId == id)
            {
                return TeamB;
            }
            else
            {
                return null;
            }
        }
    }
}