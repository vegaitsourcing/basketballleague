using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LZRNS.DomainModel.Models
{
	public class Player : AbstractModel
	{
		[Required(ErrorMessage = "Ime je obavezno polje.")]
		[DisplayName("Ime")]
		public string Name { get; set; }

		[DisplayName("Srednje ime")]
		public string MiddleName { get; set; }

		[Required(ErrorMessage = "Prezime je obavezno polje.")]
		[DisplayName("Prezime")]
		public string LastName { get; set; }

		public string Image { get; set; }

		//[Required] when we are importing data from history (round statistic, we do not have this information)
		[Required(ErrorMessage = "Visina je obavezno polje.")]
		[Range(0, 250, ErrorMessage = "Vrednost mora biti između 0 i 250")]
		[DisplayName("Visina")]
		public int Height { get; set; }

		//[Required] when we are importing data from history (round statistic, we do not have this information)
		[Required(ErrorMessage = "Težina je obavezno polje.")]
		[Range(0, 250, ErrorMessage = "Vrednost mora biti između 0 i 250")]
		[DisplayName("Težina")]
		public int Weight { get; set; }


		[Required(ErrorMessage = "Godište je obavezno polje.")]
		//[Range(1900, 2100, ErrorMessage = "Vrednost mora biti između 1900 i 2100")]
		[DisplayName("Godište")]
		public int YearOfBirth { get; set; }

		[NotMapped]
		[DisplayName("Slika")]
		public HttpPostedFileBase ImageFile { get; set; }

        [ForeignKey("Team_Id")]
        public virtual Team Team { get; set; }
        public Guid? Team_Id { get; set; }

        //this field should represent unique id of every player, possibly JMBG or some identifaction number; better to be string, not guid for possible later changes (provision of jmbg e.g.)
        public string UId { get; set; }

        public virtual ICollection<Stats> Stats { get; set; }

		public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

        

		#region Career high


		public Stats CareerHighPts
		{
			get
			{
				return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Pts).First() : null;
			}
		}

		public Stats CareerHighReb
		{
			get
			{
				return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Reb).First() : null;
			}
		}

		public Stats CareerHighAst
		{
			get
			{
				return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Ast).First() : null;
			}
		}

		public Stats CareerHighStl
		{
			get
			{
				return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Stl).First() : null;
			}
		}

		public Stats CareerHighBlk
		{
			get
			{
				return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Blk).First() : null;
			}
		}

		public Stats CareerHighEff
		{
			get
			{
				return Stats != null && Stats.Any() ? Stats.OrderByDescending(s => s.Eff).First() : null;
			}
		}

		public string GetFullName
		{
			get
			{
				return Name + " " + LastName;
			}
		}


        public string GetFullNameWithMiddleName
        {
            get
            {
                return Name + " " + MiddleName + " " + LastName;
            }
        }
        #endregion
    }
}
