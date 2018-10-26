using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LZRNS.DomainModels.ViewModels;

namespace LZRNS.DomainModel.Models
{
	public class Team : AbstractModel
	{
		[Required(ErrorMessage = "Naziv tima je obavezno polje.")]
		[DisplayName("Naziv tima")]
		public string TeamName { get; set; }

		public string Image { get; set; }

		public virtual Team PreviousTeamRef { get; set; }

		[DisplayName("Tim iz prošle sezone")]
		public Guid? PreviousTeamGuid { get; set; }

		public virtual ICollection<Player> Players { get; set; }

		[DisplayName("Trener")]
		public string Coach { get; set; }

		public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

		public virtual ICollection<Game> Games =>
			this.LeagueSeason?.Rounds
				.SelectMany(x => x.Games)
				.Where(y => y.TeamAId.Equals(this.Id) || y.TeamBId.Equals((this.Id)))
				.ToList() ?? Enumerable.Empty<Game>().ToList();

		public virtual LeagueSeason LeagueSeason { get; set; }

		[Required(ErrorMessage = "Liga u sezoni je obavezno polje.")]
		[DisplayName("Liga u sezoni")]
		public Guid LeagueSeasonId { get; set; }

		public Guid TeamScoreId { get; set; }

		#region [ViewModel Properties]

		[NotMapped]
		[DisplayName("Slika")]
		public HttpPostedFileBase ImageFile { get; set; }
		[NotMapped]
		public IEnumerable<SelectListItem> Teams { get; set; }
		[NotMapped]
		public IEnumerable<SelectListItem> LeagueSeasons { get; set; }
		[NotMapped]
		public IEnumerable<SelectListItem> AvailablePlayers { get; set; }
		#endregion

		public LeaderboardPlacing GetLeaderBoardPlacing(string roundName)
		{
			return new LeaderboardPlacing(roundName, this);
		}
	}
}
