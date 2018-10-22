using LZRNS.DomainModel.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LZRNS.DomainModels.Models
{
	public class Stats : AbstractModel
	{
		public Stats() { }

		[Required]
		public Guid GameId { get; set; }

		[ForeignKey("GameId")]
		public virtual Game Game { get; set; }

		[Required]
		public Guid PlayerId { get; set; }

		[ForeignKey("PlayerId")]
		public virtual Player Player { get; set; }

		[Required]
		[StringLength(2, MinimumLength = 1)]
		[RegularExpression("^[0-9]*$", ErrorMessage = "JerseyNumber must be numeric")]
		public string JerseyNumber { get; set; }

		[Required]
		public int MinutesPlayed { get; set; }

		[Required]
		public int TwoPtMissed { get; set; }

		[Required]
		public int TwoPtMade { get; set; }

		public int TwoPtA
		{
			get
			{
				return TwoPtMade + TwoPtMissed;
			}
		}

		public double TwoPtPerc
		{
			get
			{
				return TwoPtA is 0 ? 0 : (TwoPtMade / TwoPtA) * 100;
			}
		}

		[Required]
		public int ThreePtMissed { get; set; }

		[Required]
		public int ThreePtMade { get; set; }


		public int ThreePtA
		{
			get
			{
				return ThreePtMade + ThreePtMissed;
			}
		}

		public double ThreePtPerc
		{
			get
			{
				return ThreePtA is 0 ? 0 : (ThreePtMade / ThreePtA) * 100;
			}
		}

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
				return TwoPtMade + ThreePtMade;
			}
		}

		public double FgPerc
		{
			get
			{
				return FgA is 0 ? 0 : (FgM / FgA) * 100;
			}
		}

		[Required]
		public int FtMissed { get; set; }

		[Required]
		public int FtMade { get; set; }

		public int FtA
		{
			get
			{
				return FtMade + FtMissed;
			}
		}

		public double FtPerc
		{
			get
			{
				return FtA is 0 ? 0 : (FtMade / FtA) * 100;
			}
		}

		[Required]
		public int OReb { get; set; }

		[Required]
		public int DReb { get; set; }

		public int Reb
		{
			get
			{
				return OReb + DReb;
			}
		}

		[Required]
		public int Ast { get; set; }

		[Required]
		public int To { get; set; }

		[Required]
		public int Stl { get; set; }

		[Required]
		public int Blk { get; set; }

		public int Pts
		{
			get
			{
				return 2 * TwoPtMade + 3 * ThreePtMade + FtMade;
			}
		}

		public int Eff
		{
			get
			{
				return (Pts + Reb + Ast + Stl + Blk)
					- (FgA + FtA + To);
			}
		}

		[Required]
		public int Fd { get; set; }

		[Required]
		public int Fc { get; set; }

		public int Pir
		{
			get
			{
				return (Pts + Reb + Ast + Stl + Blk + Fd)
				- ((FgA - FgM) + (FtA - FtMade) + To + Fc);
			}
		}

		public Td Td { get; set; }
	}

	public enum Td { T, TT, TD1, TD2, D1, D2 };
}
