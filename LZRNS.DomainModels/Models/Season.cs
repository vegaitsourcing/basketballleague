using System.Collections.Generic;

namespace LZRNS.DomainModel.Models
{
    public class Season : AbstractModel
    {
        public string Name { get; set; }

        public List<Team> Teams { get; set; }

        public List<Game> Games {get;set;}
    }
}