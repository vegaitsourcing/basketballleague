using System;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using LZRNS.DomainModels.Repository.Interfaces;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class TeamDetailsController : RenderMvcController
	{
		private ITeamRepository _teamRepo;

		public TeamDetailsController(ITeamRepository teamRepo)
		{
			_teamRepo = teamRepo;
		}

		public ActionResult Index(TeamDetailsModel model, string id)
		{
			if (string.IsNullOrWhiteSpace(id)) Response.Redirect(model.Home.Url);
			if (!Guid.TryParse(id, out var guid)) Response.Redirect(model.Home.Url);

			model.Team = _teamRepo.GetById(guid);

			return CurrentTemplate(model);
		}
	}
}