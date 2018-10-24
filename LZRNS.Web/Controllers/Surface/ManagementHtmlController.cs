using System;
using System.Linq;
using System.Web.Mvc;
using LZRNS.DomainModels.Repository.Interfaces;

namespace LZRNS.Web.Controllers.Surface
{
	public class ManagementHtmlController : BaseSurfaceController
	{
		private readonly IGameRepository _gameRepo;

		public ManagementHtmlController(IGameRepository gameRepo)
		{
			_gameRepo = gameRepo;
		}

		[ChildActionOnly]
		public ActionResult ScheduledGames(int seasonStartYear, string roundName, DateTime? gamesDate)
		{
			if (seasonStartYear.Equals(default(int)) ||
				string.IsNullOrWhiteSpace(roundName) ||
				!gamesDate.HasValue) return new EmptyResult();

			var model = _gameRepo.GetGamesForSeasonAndRound(seasonStartYear, roundName)
				.Where(x => x.DateTime.Date == gamesDate.Value.Date);

			return PartialView(model);
		}

		public ActionResult GameResults(int seasonStartYear, string roundName)
		{
			if (seasonStartYear.Equals(default(int)) ||
				string.IsNullOrWhiteSpace(roundName)) return new EmptyResult();

			return PartialView(_gameRepo.GetGamesForSeasonAndRound(seasonStartYear, roundName).ToList());
		}
	}
}