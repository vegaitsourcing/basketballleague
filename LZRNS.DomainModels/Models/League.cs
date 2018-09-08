using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class League : AbstractModel
    {
        [Required]
        public string Name { get; set; }

        public ICollection<Season> Seasons { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}
