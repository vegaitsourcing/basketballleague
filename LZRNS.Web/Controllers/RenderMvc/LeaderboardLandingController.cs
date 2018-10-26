using System.Linq;
using LZRNS.Models.DocumentTypes.Pages;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using LZRNS.DomainModels.Repository.Interfaces;

namespace LZRNS.Web.Controllers.RenderMvc
{
	public class LeaderboardLandingController : RenderMvcController
	{
		private readonly ISeasonRepository _seasonRepo;

		public LeaderboardLandingController(ISeasonRepository seasonRepo)
		{
			_seasonRepo = seasonRepo;
		}

		public ActionResult Index(LeaderboardLandingModel model, string ln, string r)
		{
			model.CurrentShownLeague = !string.IsNullOrWhiteSpace(ln) ? ln : model.Leagues.FirstOrDefault();

			model.AllRounds =_seasonRepo.GetSeasonByYear(model.SeasonYearStart).LeagueSeasons
				.First(x => x.League.Name.Equals(model.CurrentShownLeague)).Rounds
				.Where(y => y.RoundName.CompareTo(model.RoundName) <= 0)
				.Select(z => z.RoundName)
				.OrderBy(k => k)
				.ToList();
			
			model.CurrentShownRound = !string.IsNullOrWhiteSpace(r) ? r : model.AllRounds.First();

			return CurrentTemplate(model);
		}
	}
}