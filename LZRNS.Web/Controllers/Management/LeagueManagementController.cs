using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class LeagueManagementController : RenderMvcController
	{
		private ILeagueRepository _leagueRepo;
		public LeagueManagementController(ILeagueRepository leagueRepo)
		{
			_leagueRepo = leagueRepo;
		}


		public ActionResult Index(LeagueManagementModel model)
		{
			model.Leagues = _leagueRepo.GetAll();

			return View(model);
		}
	}
}