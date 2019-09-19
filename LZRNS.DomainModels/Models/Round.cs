using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace LZRNS.DomainModels.Models
{
    public class Round : AbstractModel, ICloneable
    {
        public string RoundName { get; set; }

        //fix this in other way
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();

        public virtual LeagueSeason LeagueSeason { get; set; }

        [Required(ErrorMessage = "Liga u sezoni je obavezno polje.")]
        [DisplayName("Liga u sezoni")]
        public Guid LeagueSeasonId { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> LeagueSeasons { get; set; } = Enumerable.Empty<SelectListItem>();

        public object Clone()
        {
            return new Round
            {
                Id = Id,
                Games = Games,
                LeagueSeason = LeagueSeason,
                LeagueSeasonId = LeagueSeasonId,
                LeagueSeasons = LeagueSeasons,
                RoundName = RoundName
            };
        }
    }
}