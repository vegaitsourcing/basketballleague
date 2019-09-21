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
    public class Game : AbstractModel, ICloneable
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

        public virtual ICollection<Referee> Referees { get; set; } = new List<Referee>();

        [NotMapped]
        public IEnumerable<SelectListItem> Teams { get; set; } = Enumerable.Empty<SelectListItem>();

        [NotMapped]
        public virtual ICollection<StatsPerGame> StatsPerGame { get; } = new List<StatsPerGame>();

        [NotMapped]
        public ICollection<Stats> TeamAPlayerStats =>
            TeamA.PlayersPerSeason
                .Where(x => x.Player.Stats
                    .Any(y => y.GameId == Id))
                .Select(z => z.Player)
                .Select(k => k.Stats
                    .FirstOrDefault(s => s.PlayerId == k.Id && s.GameId == Id))
                .OrderBy(w => w.Player.GetFullName)
                .ToList();

        [NotMapped]
        public ICollection<Stats> TeamBPlayerStats =>
            TeamB.PlayersPerSeason
                .Where(x => x.Player.Stats
                    .Any(y => y.GameId == Id))
                .Select(z => z.Player)
                .Select(k => k.Stats
                    .FirstOrDefault(s => s.PlayerId == k.Id && s.GameId == Id))
                .OrderBy(w => w.Player.GetFullName)
                .ToList();

        [NotMapped]
        public StatsPerGame StatsPerGameA =>
                new StatsPerGame(Id, TeamA);

        [NotMapped]
        public StatsPerGame StatsPerGameB =>
                new StatsPerGame(Id, TeamB);

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}