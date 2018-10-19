using LZRNS.DomainModels.Helper;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

		[NotMapped]
		public virtual ICollection<StatsPerGame> StatsPerGame { get; set; }

		public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

		public virtual ICollection<Game> Games { get; set; }

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
		public IList<SelectListItem> PlayerList { get; set; }
		#endregion

		#region Points

		#region Pts

		public int Pts
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Pts) ?? 0;
			}
		}

		#endregion

		#region TwoPt

		public int TwoPtA
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.TwoPtA) ?? 0;
			}
		}

		public int TwoPtM
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.TwoPtM) ?? 0;
			}
		}

		public double TwoPtPerc
		{
			get
			{
				return TwoPtA.Equals(0) ? 0 : (TwoPtM / TwoPtA) * 100;
			}
		}

		#endregion

		#region ThreePt

		public int ThreePtA
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.ThreePtA) ?? 0;
			}
		}

		public int ThreePtM
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.ThreePtM) ?? 0;
			}
		}

		public double ThreePtPerc
		{
			get
			{
				return ThreePtA.Equals(0) ? 0 : (ThreePtM / ThreePtA) * 100;
			}
		}

		#endregion ThreePt

		#region Ft

		public int FtA
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.FtA) ?? 0;
			}
		}

		public int FtM
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.FtM) ?? 0;
			}
		}

		public double FtPerc
		{
			get
			{
				return FtA.Equals(0) ? 0 : (FtM / FtA) * 100;
			}
		}

		#endregion

		#region Fg

		public int FgA
		{
			get
			{
				return TwoPtA + ThreePtA;
			}
		}

		public int FgM
		{
			get
			{
				return TwoPtM + ThreePtM;
			}
		}

		public double FgPerc
		{
			get
			{
				return FgA.Equals(0) ? 0 : (FgM / FgA) * 100;
			}
		}

		#endregion

		#endregion

		#region Rebounds

		public int Reb
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Reb) ?? 0;
			}
		}

		public int OReb
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.OReb) ?? 0;
			}
		}

		public int DReb
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.DReb) ?? 0;
			}
		}

		#endregion

		#region Assists

		public int Ast
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Ast) ?? 0;
			}
		}

		#endregion

		#region TO

		public int To
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.To) ?? 0;
			}
		}

		#endregion

		#region Steals

		public int Stl
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Stl) ?? 0;
			}
		}

		#endregion

		#region Blocks

		public int Blk
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Blk) ?? 0;
			}
		}

		#endregion

		#region Minutes

		public int Min
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Min) ?? 0;
			}
		}

		#endregion

		#region Eff

		public int Eff
		{
			get
			{
				return StatsPerGame?.Sum(spg => spg.Eff) ?? 0;
			}
		}

		#endregion

		#region Stats per season

		public int Points
		{
			get
			{
				return 2 * StatsList.TotalStats(this)[0] + StatsList.TotalStats(this)[1];
			}
		}

		public int Wins
		{
			get
			{
				return StatsList.TotalStats(this)[0];
			}
		}

		public int Losts
		{
			get
			{
				return StatsList.TotalStats(this)[1];
			}
		}

		public double WLPerc
		{
			get
			{
				return Wins.Equals(0) && Losts.Equals(0) ? 0 : Wins / (Wins + Losts) * 100;
			}
		}

		public int TotalPtsScored
		{
			get
			{
				return StatsList.TotalStats(this)[2];
			}
		}

		public int TotalPtsReceived
		{
			get
			{
				return StatsList.TotalStats(this)[3];
			}
		}

		public int TotalPtsDifference
		{
			get
			{
				return TotalPtsScored - TotalPtsReceived;
			}
		}

		#endregion
	}
}
