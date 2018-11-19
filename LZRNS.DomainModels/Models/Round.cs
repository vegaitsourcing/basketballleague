using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace LZRNS.DomainModels.Models
{
	public class Round : AbstractModel
	{
		public string RoundName { get; set; }

		public virtual ICollection<Game> Games { get; set; }

		public virtual LeagueSeason LeagueSeason { get; set; }

		[Required(ErrorMessage = "Liga u sezoni je obavezno polje.")]
		[DisplayName("Liga u sezoni")]
		public Guid LeagueSeasonId { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> LeagueSeasons { get; set; }
	}
}
