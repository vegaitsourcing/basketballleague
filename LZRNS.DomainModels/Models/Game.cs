using System;
using System.Collections.Generic;

namespace LZRNS.DomainModel.Models
{
    public class Game: AbstractModel
    {
        public Season Season { get; set; }

        public Guid SeasonId { get; set; }

        public Guid Round { get; set; }

        public DateTime DateTime { get; set; }

        public Team TeamA { get; set; }

        public Guid TeamAId { get; set; }

        public Team TeamB { get; set; }

        public Guid TeamBId { get; set; }

        public List<Referee> Referees { get; set; }

        public int PointsA { get; set; }

        public int PointsB { get; set; }
    }
}