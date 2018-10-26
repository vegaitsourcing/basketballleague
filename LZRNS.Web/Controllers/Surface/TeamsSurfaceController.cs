using System;
using System.Linq;
using System.Web.Mvc;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using LZRNS.Models.Extensions;

namespace LZRNS.Web.Controllers.Surface
{
	public class TeamsSurfaceController : BaseSurfaceController
	{
		private readonly ITeamRepository _teamRepo;

		public TeamsSurfaceController(ITeamRepository teamRepo)
		{
			_teamRepo = teamRepo;
		}
		[HttpPost]
		public ActionResult SearchResults(string searchString)
		{
			if (string.IsNullOrWhiteSpace(searchString)) return new EmptyResult();

			//TODO: ON FORM SUBMIT E PREVENT DEFAULT NOT DOING SHIT JOHNATAN HOW IDK
			var model = _teamRepo.GetAll()
				.Where(x => x.TeamName.ToLower().Contains(searchString.ToLower()))
				.OrderBy(x => x.TeamName)
				.Select(y => new Tuple<string, string>(
					$"{Umbraco.GetSingleContentOfType<TeamDetailsModel>().Url}?id={y.Id.ToString()}",
					y.TeamName))
				.ToList();

			return PartialView(model);
		}
	}
}