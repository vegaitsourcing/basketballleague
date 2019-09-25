using LZRNS.Core;
using LZRNS.DomainModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LZRNS.Models.ViewModel
{
    public class RoundManagementSurfaceViewModel
    {
        public LeagueSeason LeagueSeason { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime FirstRoundStartDate { get; set; } = DateTime.Today;

        public uint IntervalBetweenGamesInMinutes { get; set; } = 120;
        public uint IntervalBetweenRoundsInDays { get; set; } = 7;

        [DataType(DataType.Time)]
        public TimeSpan RoundStartTime { get; set; } = TimeSpan.FromHours(13);

        [Required(ErrorMessage = "Liga u sezoni je obavezno polje.")]
        [DisplayName("Liga u sezoni")]
        public Guid LeagueSeasonId { get; set; }

        public IEnumerable<SelectListItem> LeagueSeasons { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}