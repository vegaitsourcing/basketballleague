using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual List<Game> Games { get; set; }
    }
}
