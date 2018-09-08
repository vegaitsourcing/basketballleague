﻿using LZRNS.DomainModels.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LZRNS.DomainModel.Models
{
    public class Season : AbstractModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int SeasonStartYear { get; set; }

        public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; }

    }
}