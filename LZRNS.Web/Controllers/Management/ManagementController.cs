using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
	public class ManagementController : RenderMvcController
	{
		private IGameRepository _gameRepo;

		public ManagementController(IGameRepository gameRepo)
		{
			_gameRepo = gameRepo;
		}

		public ActionResult Index(ManagementModel model)
		{
			model.ResultsRoundGames = _gameRepo.GetGamesForSeasonAndRound(model.StatisticsSettings.SeasonYearStart, model.StatisticsSettings.ResultsRound);
			model.ScheduleRoundGames = _gameRepo.GetGamesForSeasonAndRound(model.StatisticsSettings.SeasonYearStart, model.StatisticsSettings.ScheduleRound);

			return View(model);
		}
	}
}