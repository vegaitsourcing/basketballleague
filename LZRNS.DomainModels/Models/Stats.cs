using LZRNS.DomainModel.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LZRNS.DomainModels.Models
{
	public class Stats : AbstractModel
	{
		[Required]
		public Guid GameId { get; set; }

		[ForeignKey("GameId")]
		public virtual Game Game { get; set; }

		[Required]
		public Guid PlayerId { get; set; }

		[ForeignKey("PlayerId")]
		public virtual Player Player { get; set; }

		[Required(ErrorMessage = "Broj dresa je obavezno polje.")]
		[StringLength(2, MinimumLength = 1)]
		[DisplayName("Broj dresa")]
		[RegularExpression("^[0-9]*$", ErrorMessage = "Broj dresa mora biti broj.")]
		public string JerseyNumber { get; set; }

		[Required]
		public int MinutesPlayed { get; set; }

		[Required]
		public int TwoPtMissed { get; set; }

		[Required]
		public int TwoPtMade { get; set; }

		public int TwoPtA => TwoPtMade + TwoPtMissed;

		public double TwoPtPerc =>
			TwoPtA is 0 ? 0 : Math.Round((double)TwoPtMade / TwoPtA * 100, 1);

		[Required]
		public int ThreePtMissed { get; set; }

		[Required]
		public int ThreePtMade { get; set; }


		public int ThreePtA => ThreePtMade + ThreePtMissed;

		public double ThreePtPerc =>
			ThreePtA is 0 ? 0 : Math.Round((double)ThreePtMade / ThreePtA * 100, 1);

		public int FgA => TwoPtA + ThreePtA;

		public int FgM => TwoPtMade + ThreePtMade;

		public double FgPerc =>
			FgA is 0 ? 0 : Math.Round((double)FgM / FgA * 100, 1);

		[Required]
		public int FtMissed { get; set; }

		[Required]
		public int FtMade { get; set; }

		public int FtA => FtMade + FtMissed;

		public double FtPerc => 
			FtA is 0 ? 0 : Math.Round((double)FtMade / FtA * 100, 1);

		[Required]
		public int OReb { get; set; }

		[Required]
		public int DReb { get; set; }

		public int Reb => OReb + DReb;

		[Required]
		public int Ast { get; set; }

		[Required]
		public int To { get; set; }

		[Required]
		public int Stl { get; set; }

		[Required]
		public int Blk { get; set; }

		public int Pts => 2 * TwoPtMade + 3 * ThreePtMade + FtMade;

		public int Eff => 
			(Pts + Reb + Ast + Stl + Blk) - (FgA + FtA + To);

		[Required]
		public int Fd { get; set; }

		[Required]
		public int Fc { get; set; }

		public int Pir => 
			(Pts + Reb + Ast + Stl + Blk + Fd)
			- ((FgA - FgM) + (FtA - FtMade) + To + Fc);

		public Td Td { get; set; }
	}

	public enum Td { T, TT, TD1, TD2, D1, D2 };
}
