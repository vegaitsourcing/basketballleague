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

        public RoundScheduleOptions RoundScheduleOptions { get; set; } = new RoundScheduleOptions();

        [Required(ErrorMessage = "Liga u sezoni je obavezno polje.")]
        [DisplayName("Liga u sezoni")]
        public Guid LeagueSeasonId { get; set; }

        public IEnumerable<SelectListItem> LeagueSeasons { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}