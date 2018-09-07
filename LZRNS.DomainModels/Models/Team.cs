using System;
using System.Collections.Generic;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class Team : AbstractModel
    {
        public string TeamName { get; set; }

        public Season Season { get; set; }

        public Team PreviousTeamRef { get; set; }

        public Guid PreviousTeamGuid { get; set; }

        public List<Player> Players { get; set; }

        public string Coach { get; set; }

        public List<Game> Games {get;set;}
    }
}
