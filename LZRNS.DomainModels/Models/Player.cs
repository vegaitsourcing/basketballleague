using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LZRNS.DomainModel.Models
{
    public class Player : AbstractModel
    {
        [Required]
        public string Name { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Range(0, 250)]
        public int Height { get; set; }

        [Required]
        [Range(0, 250)]
        public int Weight { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int YearOfBirth { get; set; }

        public virtual ICollection<Stats> Stats { get; set; }

        public virtual ICollection<PlayerPerSeason> PlayersPerSeason { get; set; }
    }
}
