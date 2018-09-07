﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LZRNS.DomainModel.Models
{
    public class Season : AbstractModel
    {
        [Required]
        public string Name { get; set; }

        public virtual List<Team> Teams { get; set; }

        public virtual List<Game> Games {get;set;}
    }
}