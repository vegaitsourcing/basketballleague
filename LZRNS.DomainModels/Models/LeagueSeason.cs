using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LZRNS.DomainModels.Models
{
    public class LeagueSeason : AbstractModel
    {
        public virtual League League { get; set; }

        public Guid LeagueId { get; set; }
        public Guid SeasonId { get; set; }
        public virtual Season Season { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Summary { get; set; }
        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<Round> Rounds { get; set; }
    }
}
