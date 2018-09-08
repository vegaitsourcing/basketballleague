using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LZRNS.DomainModel.Models
{
    public class Game: AbstractModel
    {
        public virtual Season Season { get; set; }

        [ForeignKey("Season")]
        public Guid SeasonId { get; set; }

        [Required]
        public virtual Round Round { get; set; }

        [ForeignKey("Round")]
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

        public virtual List<Referee> Referees { get; set; }

        [Required]
        public int PointsA { get; set; }

        [Required]
        public int PointsB { get; set; }
    }
}