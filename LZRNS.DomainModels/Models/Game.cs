using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace LZRNS.DomainModel.Models
{
	public class Game : AbstractModel
	{
		public virtual Season Season { get; set; }

		[Required]
		[ForeignKey("Season")]
		public Guid SeasonId { get; set; }

		[ForeignKey("RoundId")]
		public virtual Round Round { get; set; }

		[Required]
		public Guid RoundId { get; set; }


		[Required(ErrorMessage = "Morate uneti datum.")]
		[DisplayName("Datum")]
		public DateTime DateTime { get; set; }

		[ForeignKey("TeamAId")]
		public virtual Team TeamA { get; set; }

		[DisplayName("Prvi tim")]
		[Required(ErrorMessage = "Morate selektovati prvi tim.")]
		public Guid TeamAId { get; set; }

		[ForeignKey("TeamBId")]
		public virtual Team TeamB { get; set; }

		[DisplayName("Drugi tim")]
		[Required(ErrorMessage = "Morate selektovati drugi tim.")]
		public Guid TeamBId { get; set; }

		public virtual ICollection<Referee> Referees { get; set; }

		[NotMapped]
		public IEnumerable<SelectListItem> Teams { get; set; }

		[NotMapped]
		public virtual ICollection<StatsPerGame> StatsPerGame { get; }

		[NotMapped]
		public ICollection<Stats> TeamAPlayerStats =>
			this.TeamA.PlayersPerSeason
				.Where(x => x.Player.Stats
					.Any(y => y.GameId == this.Id))
				.Select(z => z.Player)
				.Select(k => k.Stats
					.FirstOrDefault(s => s.PlayerId == k.Id && s.GameId == this.Id))
				.OrderBy(w => w.Player.GetFullName)
				.ToList();

		[NotMapped]
		public ICollection<Stats> TeamBPlayerStats =>
			this.TeamB.PlayersPerSeason
				.Where(x => x.Player.Stats
					.Any(y => y.GameId == this.Id))
				.Select(z => z.Player)
				.Select(k => k.Stats
					.FirstOrDefault(s => s.PlayerId == k.Id && s.GameId == this.Id))
				.OrderBy(w => w.Player.GetFullName)
				.ToList();

		[NotMapped]
		public StatsPerGame StatsPerGameA =>
				new StatsPerGame(this.Id, this.TeamA);

		[NotMapped]
		public StatsPerGame StatsPerGameB =>
				new StatsPerGame(this.Id, this.TeamB);

		public Team ReturnTeam(Guid id)
		{
			if (TeamAId == id)
			{
				return TeamA;
			}
			else if (TeamBId == id)
			{
				return TeamB;
			}
			else
			{
				return null;
			}
		}
	}
}