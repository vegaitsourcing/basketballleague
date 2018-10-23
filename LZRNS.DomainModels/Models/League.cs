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
		public string Name { get; set; }

		public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; }
	}
}
