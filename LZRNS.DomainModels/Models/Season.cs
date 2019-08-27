using LZRNS.DomainModels.Models;
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
        [StringLength(120, ErrorMessage ="Naziv sezone ne može biti duži od 120 karaktera.")]
        public string Name { get; set; }

		[Required(ErrorMessage = "Godina početka sezone je obavezno polje.")]
		[DisplayName("Godina početka sezone")]
        [Range(1900, 2100, ErrorMessage = "Vrednost mora biti između 1900 i 2100.")]
        public int SeasonStartYear { get; set; }

		public virtual ICollection<LeagueSeason> LeagueSeasons { get; set; }

		[NotMapped]
		public IList<SelectListItem> LeagueList { get; set; }
	}
}