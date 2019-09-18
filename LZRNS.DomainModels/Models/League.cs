using LZRNS.DomainModels.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LZRNS.DomainModel.Models
{
    public class League : AbstractModel
    {
        [Required(ErrorMessage = "Naziv lige je obavezno polje.")]
        [DisplayName("Naziv lige")]
        [StringLength(100, ErrorMessage = "Naziv lige ne može biti duži od 120 karaktera.")]
        public string Name { get; set; }

        public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; } = new List<LeagueSeason>();
    }
}