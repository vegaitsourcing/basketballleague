using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;

namespace LZRNS.DomainModels.Models
{
	public class LeagueSeason : AbstractModel
	{

		public Guid LeagueId { get; set; }
		public Guid SeasonId { get; set; }
		[IgnoreDataMember]
		public virtual Season Season { get; set; }
		public virtual League League { get; set; }
		[DataType(DataType.MultilineText)]
		[AllowHtml]
        [DisplayName("Rezime")]
        public string Summary { get; set; }

		public virtual ICollection<Team> Teams { get; set; }

		public virtual ICollection<Round> Rounds { get; set; }

		public string FullName => Season.Name + " - " + League.Name;
	}
}
