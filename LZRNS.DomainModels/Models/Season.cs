using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace LZRNS.DomainModel.Models
{
	public class Season : AbstractModel
	{
		[Required(ErrorMessage = "Naziv sezone je obavezno polje.")]
		[DisplayName("Naziv sezone")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Godina početka sezone je obavezno polje.")]
		[DisplayName("Godina početka sezone")]
		public int SeasonStartYear { get; set; }

		public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; }

		[NotMapped]
		public IList<SelectListItem> Leagues { get; set; }
	}
}