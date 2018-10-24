using System.Linq;
using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.Management
{
	public class ManagementController : RenderMvcController
	{
		private ISeasonRepository _seasonRepo;

		public ManagementController(ISeasonRepository seasonRepo)
		{
			_seasonRepo = seasonRepo;
		}

		public ActionResult Index(ManagementModel model)
		{
			model.ResultsRoundGames = _seasonRepo.GetSeasonByYear(model.StatisticsSettings.SeasonYearStart)
				.LeagueSeasons.First( /*TODO: INSERT LEAGUE ID PLS*/)
				.Rounds.Where(x => x.RoundName.CompareTo(model.StatisticsSettings.ResultsRound) <= 0)
				.SelectMany(y => y.Games)
				.ToList();

			model.ScheduleRoundGames = _seasonRepo.GetSeasonByYear(model.StatisticsSettings.SeasonYearStart)
				.LeagueSeasons.First( /*TODO: INSERT LEAGUE ID PLS*/)
				.Rounds.Where(x => x.RoundName.CompareTo(model.StatisticsSettings.ScheduleRound) <= 0)
				.SelectMany(y => y.Games)
				.ToList();

			return View(model);
		}
	}
}