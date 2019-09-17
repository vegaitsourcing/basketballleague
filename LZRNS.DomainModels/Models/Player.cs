using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required(ErrorMessage = "Visina je obavezno polje.")]
        [Range(0, 250, ErrorMessage = "Vrednost mora biti između 0 i 250")]
        [DisplayName("Visina")]
        public int Height { get; set; }

        [Required(ErrorMessage = "Težina je obavezno polje.")]
        [Range(0, 250, ErrorMessage = "Vrednost mora biti između 0 i 250")]
        [DisplayName("Težina")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Godište je obavezno polje.")]
        [DisplayName("Godište")]
        public int YearOfBirth { get; set; }

        [NotMapped]
        [DisplayName("Slika")]
        public HttpPostedFileBase ImageFile { get; set; }

        [ForeignKey(nameof(Team_Id))]
        public virtual Team Team { get; set; }

        public Guid? Team_Id { get; set; }

        //this field should represent unique id of every player, possibly JMBG or some identifaction number; better to be string, not guid for possible later changes (provision of jmbg e.g.)
        public string UId { get; set; }

        public virtual ICollection<Stats> Stats { get; set; }

        public virtual ICollection<PlayerPerTeam> PlayersPerSeason { get; set; }

        public string GetFullName => Name + " " + LastName;

        public string GetFullNameWithMiddleName => Name + " " + MiddleName + " " + LastName;
    }
}