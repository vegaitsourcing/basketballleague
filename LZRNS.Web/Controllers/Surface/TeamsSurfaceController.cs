using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Models.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace LZRNS.Web.Controllers.Surface
{
	public class TeamsSurfaceController : BaseSurfaceController
	{
		private readonly ITeamRepository _teamRepo;

		public TeamsSurfaceController(ITeamRepository teamRepo)
		{
			_teamRepo = teamRepo;
		}

		[ChildActionOnly]
		[HttpGet]
		public ActionResult SearchResults(int? seasonYearStart) =>
			PartialView(GetAllTeams(seasonYearStart));

		[HttpPost]
		public ActionResult SearchResults(string searchString, int? seasonYearStart) =>
			string.IsNullOrWhiteSpace(searchString) ?
				PartialView(GetAllTeams(seasonYearStart)) : 
				PartialView(GetAllTeams(seasonYearStart)
					.Where(x => x.Item2.ToLower()
						.Contains(searchString.ToLower())));

		private IEnumerable<Tuple<string, string>> GetAllTeams(int? seasonYearStart) =>
			_teamRepo.GetAll()
				.GroupBy(t => t.TeamName)
				.Where(x => x.Any(t => seasonYearStart == null || t.LeagueSeason.Season.SeasonStartYear.Equals(seasonYearStart)))
				.Select(g => g
					.Where(t => seasonYearStart == null || t.LeagueSeason.Season.SeasonStartYear.Equals(seasonYearStart))
					.OrderByDescending(t => t.LeagueSeason.Season.SeasonStartYear)
					.FirstOrDefault())
				.OrderBy(t => t.TeamName)
				.Select(y => new Tuple<string, string>(
					$"{Umbraco.GetSingleContentOfType<TeamDetailsModel>(CultureInfo.CurrentCulture).Url}?id={y.Id.ToString()}",
					y.TeamName))
				.ToList();
	}
}